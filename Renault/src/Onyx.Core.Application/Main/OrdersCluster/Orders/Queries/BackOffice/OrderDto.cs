using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice;
public class OrderDto : IMapFrom<Order>
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string Number { get; set; } = null!;
    public double Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public double DiscountPercentForRole { get; set; }
    public decimal Total { get; set; }
    public string CreatedAt { get; set; } = null!;
    public OrderPaymentType OrderPaymentType { get; set; }
    public string OrderPaymentTypeName => EnumHelper<OrderPaymentType>.GetDisplayValue(OrderPaymentType);
    public bool IsPayed { get; set; }
    public string OrderAddress { get; set; } = null!;
    public OrderStatus CurrentOrderStateId { get; set; }
    public string CurrentOrderStateName => EnumHelper<OrderStatus>.GetDisplayValue(CurrentOrderStateId);
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public string CustomerTypeEnumName => EnumHelper<CustomerTypeEnum>.GetDisplayValue(CustomerTypeEnum);
    public string PhoneNumber { get; init; } = null!;
    public string FullCustomerName { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderDto>()
            .ForMember(d => d.CurrentOrderStateId,
                opt => 
                    opt.MapFrom(s => s.OrderStateHistory.OrderBy(e => e.Created).Last().OrderStatus))
            .ForMember(d => d.OrderAddress,
            opt =>
                opt.MapFrom(s => s.OrderAddress.AddressDetails1 + s.OrderAddress.AddressDetails2 + s.OrderAddress.Postcode))
            .ForMember(d => d.CreatedAt,
            opt =>
                opt.MapFrom(s => s.CreatedAt.ToPersianDate()))
            .ForMember(d => d.FullCustomerName,
            opt =>
                opt.MapFrom(s => s.PersonType == PersonType.Legal ? s.CustomerFirstName : s.CustomerFirstName + " " + s.CustomerLastName));
    }
}