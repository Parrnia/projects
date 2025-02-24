using MediatR;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.SharedCommands;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.CreateOrder.CreateOrder;

public record CreateOrderCommand : IRequest<int>
{
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public PersonType PersonType { get; set; }
    public Guid CustomerId { get; set; }
    public double Quantity { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Total { get; init; }
    public string? Details { get; init; }
    public string PhoneNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string NationalCode { get; set; } = null!;
    public IList<CreateOrderItemCommandForOrder> OrderItems { get; set; } = new List<CreateOrderItemCommandForOrder>();
    public string Title { get; init; } = null!;
    public string? Company { get; init; }
    public int CountryId { get; init; }
    public string AddressDetails1 { get; init; } = null!;
    public string? AddressDetails2 { get; init; }
    public string City { get; init; } = null!;
    public string State { get; init; } = null!;
    public string Postcode { get; init; } = null!;
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICreateOrderHelper _createOrderHelper;
    private readonly ISmsService _smsService;
    private readonly ApplicationSettings _applicationSettings;


    public CreateOrderCommandHandler(
        IApplicationDbContext context,
        ICreateOrderHelper createOrderHelper,
        ISmsService smsService,
        IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _createOrderHelper = createOrderHelper;
        _smsService = smsService;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customerType = await _createOrderHelper.GetCustomerType(request.CustomerTypeEnum, cancellationToken);

        var products = await _createOrderHelper.GetProducts(request.OrderItems.Select(c => c.ProductId).ToList(), cancellationToken);
        
        await _createOrderHelper.CheckAndSyncWithSeven(products, request.OrderItems.ToList(), cancellationToken);

        var customer = await _createOrderHelper.GetCustomer(request.CustomerId, cancellationToken);

        //برای جلوگیری از ثبت سفارش های فیک
        if (customer.Orders.Count(c => c.GetCurrentOrderState().OrderStatus == OrderStatus.PendingForRegister) >= 10)
        {
            throw new OrderException("لطفا قبل از ثبت سفارش جدید، سفارش های قبلی خود را تعیین وضعیت نمایید");
        }

        var country = await _createOrderHelper.GetCountry(request.CountryId, cancellationToken);

        var address = new OrderAddress()
        {
            Title = request.Title,
            Company = request.Company,
            Country = country.LocalizedName,
            AddressDetails1 = request.AddressDetails1,
            AddressDetails2 = request.AddressDetails2,
            City = request.City,
            State = request.State,
            Postcode = request.Postcode
        };

        var order = new Order
        {
            Token = Guid.NewGuid().ToString(),
            DiscountPercentForRole = customerType.DiscountPercent,
            OrderAddress = address,
            CustomerId = request.CustomerId,
            CustomerTypeEnum = request.CustomerTypeEnum,
            PhoneNumber = request.PhoneNumber,
            CustomerFirstName = request.FirstName,
            CustomerLastName = request.LastName,
            PersonType = request.PersonType,
            NationalCode = request.NationalCode,
            Number = await _createOrderHelper.CreateOrderNumber(cancellationToken),
            TaxPercent = _applicationSettings.TaxPercent,
            OrderPaymentType = OrderPaymentType.Unspecified
        };
        

        foreach (var orderItemCommand in request.OrderItems)
        {
            var product = _createOrderHelper.GetProduct(orderItemCommand.ProductId, products);
            var productAttributeOption = _createOrderHelper.GetProductAttributeOption(orderItemCommand.ProductAttributeOptionId, product.AttributeOptions);
            var productAttributeOptionRole = _createOrderHelper.GetProductAttributeOptionRole(request.CustomerTypeEnum, productAttributeOption.ProductAttributeOptionRoles);
            var price = productAttributeOption.GetPrice();
            var discountPercentForProduct = productAttributeOptionRole.DiscountPercent;
            var optionValues = productAttributeOption.OptionValues.Select(optionValue => new OrderItemProductAttributeOptionValue() { Value = optionValue.Value, Name = optionValue.Name, }).ToList();
            
            var orderItem = new OrderItem
            {
                Price = price,
                DiscountPercentForProduct = discountPercentForProduct,
                Quantity = orderItemCommand.Quantity,
                ProductAttributeOption = productAttributeOption,
                Total = price * (decimal)orderItemCommand.Quantity ,
                ProductLocalizedName = productAttributeOption.Product.LocalizedName,
                ProductName = productAttributeOption.Product.Name,
                ProductNo = productAttributeOption.Product.ProductNo,
                ProductAttributeOptionId = orderItemCommand.ProductAttributeOptionId,
                OptionValues = optionValues
            };

            foreach (var orderItemOptionCommand in orderItemCommand.OrderItemOptions)
            {
                var orderItemOption = new OrderItemOption
                {
                    Name = orderItemOptionCommand.Name,
                    Value = orderItemOptionCommand.Value,
                };
                orderItem.Options.Add(orderItemOption);
            }
            order.Items.Add(orderItem);
        }

        order.SetQuantity();
        order.SetSubtotal();
        order.SetTaxTotal();
        order.SetDiscountOnProductTotal();
        order.SetDiscountOnCustomerTypeTotal();
        order.SetDeliveryTotal();
        order.SetTotal();

        _createOrderHelper.CheckRequestAndDatabase(request, order);

        order.OrderStateHistory.Add(new PendingForRegisterState()
        {
            OrderStatus = OrderStatus.PendingForRegister,
            Details = "درخواست ثبت سفارش دریافت شد"
        });

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(request.PhoneNumber, ".ثبت شده و آماده پرداخت است " + order.Number + "سفارش شما به شماره ");

        return order.Id;
    }
}
