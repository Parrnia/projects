using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetInvoice;

public class ReturnOrderInvoiceItemDto : IMapFrom<ReturnOrderItem>
{
    public double Quantity { get; set; }
    public decimal Total { get; set; }
    public bool IsAccepted { get; set; }
}