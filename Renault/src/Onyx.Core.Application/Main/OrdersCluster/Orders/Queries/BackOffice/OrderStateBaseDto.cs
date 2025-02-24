using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice;

public class OrderStateBaseDto : IMapFrom<OrderStateBase>
{
    public int Id { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string OrderStatusName => EnumHelper<OrderStatus>.GetDisplayValue(OrderStatus);
    public string Details { get; set; } = null!;
    public string Created { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderStateBase, OrderStateBaseDto>()
            .ForMember(d => d.Created,
                opt =>
                    opt.MapFrom(s => s.Created.ToPersianDate()));
    }
}