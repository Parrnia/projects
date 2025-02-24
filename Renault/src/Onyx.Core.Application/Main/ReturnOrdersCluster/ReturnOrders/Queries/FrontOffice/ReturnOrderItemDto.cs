using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice;

public class ReturnOrderItemDto : IMapFrom<ReturnOrderItem>
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public decimal Total { get; set; }
    public ReturnOrderReasonDto ReturnOrderReason { get; set; } = null!;
    public List<ReturnOrderItemDocumentDto> ReturnOrderItemDocuments { get; set; } = new List<ReturnOrderItemDocumentDto>();
    public bool IsAccepted { get; set; }

}