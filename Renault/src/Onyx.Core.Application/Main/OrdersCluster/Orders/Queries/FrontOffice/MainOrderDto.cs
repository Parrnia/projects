using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.OrdersCluster.OrderPayments.BackOffice.Queries;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice;
public class MainOrderDto : IMapFrom<Order>
{
    public MainOrderDto()
    {
        Items = new List<OrderItemDto>();
        Totals = new List<OrderTotalDto>();
        PaymentMethods = new List<OrderPaymentDto>();
        OrderStateHistory = new List<OrderStateBaseDto>();
    }
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string Number { get; set; } = null!;
    public double Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public double DiscountPercentForRole { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderPaymentType OrderPaymentType { get; set; }
    public bool IsPayed { get; set; }
    public List<OrderStateBaseDto> OrderStateHistory { get; set; }
    public OrderAddressDto OrderAddress { get; set; } = null!;
    public List<OrderItemDto> Items { get; set; }
    public List<OrderTotalDto> Totals { get; set; }
    public List<OrderPaymentDto> PaymentMethods { get; set; }

    public OrderStatus CurrentOrderStatus { get; set; }
    public string CurrentOrderStatusDetails { get; set; } = null!;
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public Guid CustomerId { get; set; }

    public void Mapping(Profile profile)
    {
        //profile.CreateMap<Order, MainOrderDto>()
        //    .ForMember(i => i.PaymentType, opt =>
        //        opt.MapFrom(s => s.PaymentMethods != null ? s.PaymentMethods.PaymentType : (PaymentType)(-1)));
    }
}