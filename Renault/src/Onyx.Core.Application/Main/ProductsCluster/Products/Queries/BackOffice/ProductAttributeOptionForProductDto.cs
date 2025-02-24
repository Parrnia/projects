using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class ProductAttributeOptionForProductDto : IMapFrom<ProductAttributeOption>
{
    public ProductAttributeOptionForProductDto()
    {
        Prices = new List<PriceDto>();
        OptionValues = new List<ProductAttributeOptionValueDto>();
        Badges = new List<BadgeDto>();
        ProductAttributeOptionRoles = new List<ProductAttributeOptionRoleDto>();
    }
    public int Id { get; set; }
    public double TotalCount { get; set; }
    public double SafetyStockQty { get; set; }
    public double MinStockQty { get; set; }
    public double MaxStockQty { get; set; }
    public List<PriceDto> Prices { get; set; }
    public double? MaxSalePriceNonCompanyProductPercent { get; set; }
    public List<BadgeDto> Badges { get; set; }
    public List<ProductAttributeOptionValueDto> OptionValues { get; set; }
    public bool IsDefault { get; set; }
    public int ProductId { get; set; }
    public List<ProductAttributeOptionRoleDto> ProductAttributeOptionRoles { get; set; }
}
