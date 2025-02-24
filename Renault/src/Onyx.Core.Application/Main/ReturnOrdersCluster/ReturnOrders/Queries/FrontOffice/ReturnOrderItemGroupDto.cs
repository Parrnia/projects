using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice;

public class ReturnOrderItemGroupDto : IMapFrom<ReturnOrderItemGroup>
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public double TotalDiscountPercent { get; set; }
    public string ProductLocalizedName { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ProductNo { get; set; }
    public int ProductAttributeOptionId { get; set; }
    public double TotalQuantity { get; set; }
    public List<ReturnOrderItemGroupProductAttributeOptionValueDto> OptionValues { get; set; } = new List<ReturnOrderItemGroupProductAttributeOptionValueDto>();
    public List<ReturnOrderItemDto> ReturnOrderItems { get; set; } = new List<ReturnOrderItemDto>();

}