using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice;
public class ReturnOrderItemDto : IMapFrom<ReturnOrderItem>
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public decimal Total { get; set; }
    public ReturnOrderReasonDto ReturnOrderReason { get; set; } = null!;
    public bool IsAccepted { get; set; }
}
