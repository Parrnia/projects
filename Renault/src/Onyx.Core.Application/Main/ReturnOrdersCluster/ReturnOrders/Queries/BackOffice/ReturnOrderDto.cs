using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice;
public class ReturnOrderDto : IMapFrom<ReturnOrder>
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public string Number { get; set; } = null!;
    public double Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string CreatedAt { get; set; } = null!;
    public CostRefundType CostRefundType { get; set; }
    public string CostRefundTypeName => EnumHelper<CostRefundType>.GetDisplayValue(CostRefundType);
    public List<ReturnOrderStateBaseDto> ReturnOrderStateHistory { get; set; } = new List<ReturnOrderStateBaseDto>();
    public ReturnOrderStatus CurrentReturnOrderState { get; set; }
    public string CurrentReturnOrderStateName => EnumHelper<ReturnOrderStatus>.GetDisplayValue(CurrentReturnOrderState);
    public ReturnOrderTransportationType ReturnOrderTransportationType { get; set; }
    public string ReturnOrderTransportationTypeName => EnumHelper<ReturnOrderTransportationType>.GetDisplayValue(ReturnOrderTransportationType);
    public string OrderNumber { get; set; } = null!;
    public string? CustomerAccountInfo { get; set; }
    public int OrderId { get; set; }
    public string PhoneNumber { get; init; } = null!;
    public string FullCustomerName { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, ReturnOrderDto>()
            .ForMember(d => d.OrderNumber,
            opt =>
                opt.MapFrom(s => s.Order.Number))
            .ForMember(d => d.CurrentReturnOrderState,
            opt =>
                opt.MapFrom(s => s.ReturnOrderStateHistory.OrderBy(e => e.Created).Last().ReturnOrderStatus))
            .ForMember(d => d.PhoneNumber,
            opt =>
                opt.MapFrom(s => s.Order.PhoneNumber))
            .ForMember(d => d.CreatedAt,
            opt =>
                opt.MapFrom(s => s.CreatedAt.ToPersianDate()))
            .ForMember(d => d.FullCustomerName,
            opt =>
                opt.MapFrom(s => s.Order.PersonType == PersonType.Legal ? s.Order.CustomerFirstName : s.Order.CustomerFirstName + " " + s.Order.CustomerLastName));
    }
}