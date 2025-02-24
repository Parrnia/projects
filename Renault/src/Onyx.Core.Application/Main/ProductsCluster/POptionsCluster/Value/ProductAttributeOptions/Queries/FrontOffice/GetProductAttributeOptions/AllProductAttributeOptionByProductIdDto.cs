using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.FrontOffice.GetProductAttributeOptions;

public class AllProductAttributeOptionByProductIdDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public double TotalCount { get; set; }
    public double SafetyStockQty { get; set; }
    public double MinStockQty { get; set; }
    public double MaxStockQty { get; set; }
    public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    public double? MaxSalePriceNonCompanyProductPercent { get; set; }
    public List<BadgeDto> Badges { get; set; } = new List<BadgeDto>();
    public List<ProductAttributeOptionValueDto> OptionValues { get; set; } = new List<ProductAttributeOptionValueDto>();
    public bool IsDefault { get; set; }
    public int ProductId { get; set; }
    public List<ProductAttributeOptionRoleDto> ProductAttributeOptionRoles { get; set; } = new List<ProductAttributeOptionRoleDto>();
}
public class PriceDto : IMapFrom<Price>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal MainPrice { get; set; }
}
public class BadgeDto : IMapFrom<Badge>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public bool IsActive { get; set; }
}
public class ProductAttributeOptionValueDto : IMapFrom<ProductAttributeOptionValue>
{
    public int Id { get; set; }
    public int ProductAttributeOptionId { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}
public class ProductAttributeOptionRoleDto : IMapFrom<ProductAttributeOptionRole>
{
    public int Id { get; set; }
    public double MinimumStockToDisplayProductForThisType { get; set; }
    public AvailabilityEnum Availability { get; set; }
    public double MainMaxOrderQty { get; set; }
    public double CurrentMaxOrderQty { get; set; }
    public double MainMinOrderQty { get; set; }
    public double CurrentMinOrderQty { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public double DiscountPercent { get; set; }
}