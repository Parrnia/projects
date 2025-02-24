using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditPay;

public record CreateCreditOrderPayCommand : IRequest<int>
{
    public int OrderId { get; init; }
    public Guid CustomerId { get; set; }
};

public class CreateCreditOrderPayCommandHandler : IRequestHandler<CreateCreditOrderPayCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICreateOrderHelper _createOrderHelper;
    private readonly ISevenSoftService _sevenSoftService;
    private readonly ISmsService _smsService;
    private readonly ApplicationSettings _applicationSettings;


    public CreateCreditOrderPayCommandHandler(IApplicationDbContext context,
        ICreateOrderHelper createOrderHelper,
        ISevenSoftService sevenSoftService,
        ISmsService smsService,
        IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _createOrderHelper = createOrderHelper;
        _sevenSoftService = sevenSoftService;
        _smsService = smsService;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<int> Handle(CreateCreditOrderPayCommand request, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(e => e.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }
        var customerType = await _createOrderHelper.GetCustomerType(order.CustomerTypeEnum, cancellationToken);

        if (order.CustomerTypeEnum == CustomerTypeEnum.Personal)
        {
            throw new OrderException("نوع کاربر با شیوه پرداخت نامتناسب است");
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
        var customer = await _createOrderHelper.GetCustomer(order.CustomerId, cancellationToken);

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
            ExtraPrice = Decimal.Zero,
            DiscountValue = Decimal.Zero,
            DiscountPercent = 0,
            IsPayableForCustomer = true,
            ItemTypeId = "2"
        };
        sevenSoftOrderProductCommands.Add(sevenSoftOrderDeliveryCommand);

        var successfulCreditPay = customer.UseCredit(order.Total, order.CustomerId, order.CustomerFirstName + " " + order.CustomerLastName, order.Number);
        if (!successfulCreditPay)
        {
            throw new OrderException("مبلغ سفارش از اعتبار مشتری بیشتر است");
        }

        var payment = OrderPayment.Create(PaymentType.Credit, (long)order.Total, null);
        order.AddOrderPayment(payment,OrderPaymentType.Credit);

        var successfulRegister = order.RegisterOrder("کاربر اعتبار کافی داشته و سفارش ثبت شده است", order.CustomerTypeEnum);
        if (!successfulRegister)
        {
            throw new OrderException("موجودی یک یا چند کالا در این سفارش تغییر یافته است برای به روزرسانی سبد خرید بر روی دکمه به روزرسانی کلیک نمایید");
        }
        order.IsPayed = true;

        var sevenSoftCommand = _createOrderHelper.CreateSevenSoftCommand(order, sevenSoftOrderProductCommands);

        var sevenSoftOrderCreationResult = await _sevenSoftService.CreateSevenSoftOrder(sevenSoftCommand, cancellationToken);
        if (sevenSoftOrderCreationResult == null)
        {
            throw new SevenException("با عرض پوزش، مشکلی در ارتباط با سرور به وجود آمده است، لطفا دقایقی بعد دوباره تلاش نمایید");
        }
        order.Token = sevenSoftOrderCreationResult;


        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(order.PhoneNumber, ".ثبت شد " + order.Token + " با شماره پیگیری " + order.Number + "سفارش شما به شماره ");

        return order.PaymentMethods.OrderBy(c => c.Created).LastOrDefault()!.Id;
    }

}
