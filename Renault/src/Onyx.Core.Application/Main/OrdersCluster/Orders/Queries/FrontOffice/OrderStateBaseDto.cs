using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice;

public class OrderStateBaseDto : IMapFrom<OrderStateBase>
{
    public int Id { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string Details { get; set; } = null!;
    public DateTime Created { get; set; }
}