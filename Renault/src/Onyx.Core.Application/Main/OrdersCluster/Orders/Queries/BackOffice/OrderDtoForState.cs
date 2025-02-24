using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice;
public class OrderDtoForState : IMapFrom<Order>
{
    public int Id { get; set; }
    public OrderStateBase CurrentOrderStateBase { get; set; } = null!;
    public string CurrentOrderStateName => EnumHelper<OrderStatus>.GetDisplayValue(CurrentOrderStateBase.OrderStatus);
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderDtoForState>()
            .ForMember(d => d.CurrentOrderStateBase,
                opt =>
                    opt.MapFrom(s => s.GetCurrentOrderState()));
    }
}