using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application.Services.PaymentServices.Responses;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.OnlinePay;

public record CreateOnlineOrderPayCommand : IRequest<StartPaymentResult>
{
    public int OrderId { get; init; }
    public Guid CustomerId { get; set; }
    public PaymentServiceType PaymentService { get; set; }
};

public class CreateOnlineOrderPayCommandHandler : IRequestHandler<CreateOnlineOrderPayCommand, StartPaymentResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;
    private readonly ICreateOrderHelper _createOrderHelper;
    private readonly ApplicationSettings _applicationSettings;
    private readonly PaymentServiceFactory _paymentServiceFactory;


    public CreateOnlineOrderPayCommandHandler(
        IApplicationDbContext context,
        ICreateOrderHelper createOrderHelper, ISevenSoftService sevenSoftService,
        IOptions<ApplicationSettings> applicationSettings,
        PaymentServiceFactory paymentServiceFactory)
    {
        _context = context;
        _createOrderHelper = createOrderHelper;
        _sevenSoftService = sevenSoftService;
        _paymentServiceFactory = paymentServiceFactory;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<StartPaymentResult> Handle(CreateOnlineOrderPayCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(c => c.PaymentMethods)
            .Include(c => c.OrderStateHistory)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.AttributeOptions).ThenInclude(c => c.OptionValues)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.ColorOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.MaterialOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .Include(c => c.Totals)
            .SingleOrDefaultAsync(e => e.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        if (order.CustomerId != request.CustomerId)
        {
            throw new OrderException("شما دسترسی لازم را ندارید");
        }

        var products = await _createOrderHelper.GetProducts(order.Items.Select(c => c.ProductAttributeOption.ProductId).ToList(), cancellationToken);

        await _createOrderHelper.CheckAndSyncWithSeven(products, order.Items.ToList(), cancellationToken);
        var productsIsAvailable = _createOrderHelper.CheckProductsAvailability(products, order.Items.ToList(), order.CustomerTypeEnum);
        if (!productsIsAvailable)
        {
            throw new OrderException("موجودی یک یا چند کالا در این سفارش تغییر یافته است برای به روزرسانی سبد خرید بر روی دکمه به روزرسانی کلیک نمایید");
        }

        var customerType = await _createOrderHelper.GetCustomerType(order.CustomerTypeEnum, cancellationToken);


        var sevenSoftOrderProductCommands = new List<SevenSoftOrderProductCommand>();
        var discountPercentForCustomerType = customerType.DiscountPercent;

        foreach (var orderItem in order.Items)
        {
            var product = _createOrderHelper.GetProduct(orderItem.ProductAttributeOption.ProductId, products);
            var productAttributeOption = _createOrderHelper.GetProductAttributeOption(orderItem.ProductAttributeOptionId, product.AttributeOptions);
            var productAttributeOptionRole = _createOrderHelper.GetProductAttributeOptionRole(order.CustomerTypeEnum, productAttributeOption.ProductAttributeOptionRoles);
            var price = productAttributeOption.GetPrice();
            var discountPercentForProduct = productAttributeOptionRole.DiscountPercent;
            var totalDiscountPercent = discountPercentForProduct + discountPercentForCustomerType;

            var sevenSoftOrderProductCommand = new SevenSoftOrderProductCommand
            {
                PartId = product.Related7SoftProductId,
                ExchangeNumber = orderItem.Quantity,
                UnitPrice = price,
                ExtraPrice = Decimal.Zero,
                DiscountValue = _createOrderHelper.CalculateDiscount(orderItem.Total, totalDiscountPercent),
                DiscountPercent = totalDiscountPercent,
                IsPayableForCustomer = true,
                ItemTypeId = "1"
            };

            sevenSoftOrderProductCommands.Add(sevenSoftOrderProductCommand);
        }

        var deliveryTotalPrice = _createOrderHelper.CalculateDeliveryCost(order.Subtotal, order.Totals);
        var deliveryOrderTotalPrice = order.Totals.SingleOrDefault(c => c.Type == OrderTotalType.Shipping)?.Price;
        if (deliveryTotalPrice != deliveryOrderTotalPrice)
        {
            throw new OrderException("در هزینه ارسال کالا مغایرت وجود دارد برای به روزرسانی سفارش بر روی دکمه به روزرسانی کلیک کنید");
        }

        var sevenSoftOrderDeliveryCommand = new SevenSoftOrderProductCommand
        {
            PartId = null,
            ExchangeNumber = 1,
            UnitPrice = deliveryTotalPrice,
            ExtraPrice = decimal.Zero,
            DiscountValue = decimal.Zero,
            DiscountPercent = 0,
            IsPayableForCustomer = true,
            ItemTypeId = "2"
        };
        sevenSoftOrderProductCommands.Add(sevenSoftOrderDeliveryCommand);


        var payment = (OnlinePayment)OrderPayment.Create(PaymentType.Online, (long)order.Total, request.PaymentService);
        order.AddOrderPayment(payment, OrderPaymentType.Online);

        var successfulRegister = order.RegisterOrder("فرایند پرداخت طی شده و سفارش ثبت شده است", order.CustomerTypeEnum);
        if (!successfulRegister)
        {
            throw new OrderException("موجودی یک یا چند کالا در این سفارش تغییر یافته است برای به روزرسانی سبد خرید بر روی دکمه به روزرسانی کلیک نمایید");
        }

        var sevenSoftCommand = _createOrderHelper.CreateSevenSoftCommand(order, sevenSoftOrderProductCommands);
        
        var sevenSoftOrderCreationResult = await _sevenSoftService.CreateSevenSoftOrder(sevenSoftCommand, cancellationToken);
        if (sevenSoftOrderCreationResult == null)
        {
            throw new SevenException("با عرض پوزش، مشکلی در ارتباط با سرور به وجود آمده است، لطفا دقایقی بعد دوباره تلاش نمایید");
        }
        order.Token = sevenSoftOrderCreationResult;

        
        await _context.SaveChangesAsync(cancellationToken);

        var paymentService = _paymentServiceFactory.Create(request.PaymentService);
        var result = await paymentService.StartPayment(payment.Amount!.Value,
            payment.Id, order.PhoneNumber);

        if (result.IsSuccess)
        {
            payment.Authority = result.Token;
            payment.Status = OnlinePaymentStatus.Waiting;
        }
        else
        {
            payment.Error = result.ErrorMessage;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return result;
    }
}
