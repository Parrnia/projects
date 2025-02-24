using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetInvoice;

public class OrderInvoiceTotalDto : IMapFrom<OrderTotal>
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public OrderTotalType Type { get; set; }
}