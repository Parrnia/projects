using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice;
public class MainReturnOrderDto : IMapFrom<ReturnOrder>
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public string Number { get; set; } = null!;
    public double Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public CostRefundType CostRefundType { get; set; }
    public string CostRefundTypeName => EnumHelper<CostRefundType>.GetDisplayValue(CostRefundType);
    public List<ReturnOrderStateBaseDto> ReturnOrderStateHistory { get; set; } = new List<ReturnOrderStateBaseDto>();
    public ReturnOrderStateBaseDto CurrentReturnOrderState { get; set; } = null!;
    public ReturnOrderTransportationType ReturnOrderTransportationType { get; set; }
    public string ReturnOrderTransportationTypeName => EnumHelper<ReturnOrderTransportationType>.GetDisplayValue(ReturnOrderTransportationType);
    public List<ReturnOrderItemGroupDto> ItemGroups { get; set; } = new List<ReturnOrderItemGroupDto>();
    public List<ReturnOrderTotalDto> Totals { get; set; } = new List<ReturnOrderTotalDto>();
    public string OrderNumber { get; set; } = null!;
    public string? CustomerAccountInfo { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, MainReturnOrderDto>()
            .ForMember(d => d.OrderNumber,
                opt =>
                    opt.MapFrom(s => s.Order.Number))
            //.ForMember(d => d.CreatedAt,
            //    opt =>
            //        opt.MapFrom(s => s.CreatedAt.ToPersianDate()))
            .ForMember(d => d.CurrentReturnOrderState,
                opt =>
                    opt.MapFrom(s => s.ReturnOrderStateHistory.OrderBy(c => c.Created).Last()));
    }
}