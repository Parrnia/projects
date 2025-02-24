using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.BackOffice;
public class OrderTotalDto : IMapFrom<OrderTotal>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public OrderTotalType Type { get; set; }
    public string TypeName => EnumHelper<OrderTotalType>.GetDisplayValue(Type);
}
