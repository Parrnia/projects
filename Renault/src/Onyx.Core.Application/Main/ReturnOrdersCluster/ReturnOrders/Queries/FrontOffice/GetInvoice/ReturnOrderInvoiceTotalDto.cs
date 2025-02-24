using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetInvoice;

public class ReturnOrderInvoiceTotalDto : IMapFrom<ReturnOrderTotal>
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public ReturnOrderTotalType Type { get; set; }
    public ReturnOrderTotalApplyType ReturnOrderTotalApplyType { get; set; }
}