using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetInvoice;

public class ReturnOrderInvoiceItemGroupDto : IMapFrom<ReturnOrderItemGroup>
{
    public decimal Price { get; set; }
    public double TotalDiscountPercent { get; set; }
    public string ProductLocalizedName { get; set; } = null!;
    public string? ProductNo { get; set; }
    public List<ReturnOrderItemGroupProductAttributeOptionValueDto> OptionValues { get; set; } = new List<ReturnOrderItemGroupProductAttributeOptionValueDto>();
    public List<ReturnOrderInvoiceItemDto> ReturnOrderItems { get; set; } = new List<ReturnOrderInvoiceItemDto>();
}