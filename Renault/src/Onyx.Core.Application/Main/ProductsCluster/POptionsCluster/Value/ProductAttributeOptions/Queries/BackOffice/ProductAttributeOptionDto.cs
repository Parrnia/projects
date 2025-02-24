using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.BackOffice;

public class ProductAttributeOptionDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public double TotalCount { get; set; }
    public double SafetyStockQty { get; set; }
    public double MinStockQty { get; set; }
    public double MaxStockQty { get; set; }
    public double? MaxSalePriceNonCompanyProductPercent { get; set; }
    public bool IsDefault { get; set; }
    public List<ProductAttributeOptionValueDto> OptionValues { get; set; } = new List<ProductAttributeOptionValueDto>();
}
public class ProductAttributeOptionValueDto : IMapFrom<ProductAttributeOptionValue>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}

