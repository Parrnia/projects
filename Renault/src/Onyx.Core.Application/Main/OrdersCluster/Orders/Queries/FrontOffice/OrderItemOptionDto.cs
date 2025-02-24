using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice;

public class OrderItemOptionDto : IMapFrom<OrderItemOption>
{

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}