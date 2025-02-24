using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.FrontOffice.GetProductAttributeTypes;
public class AllProductAttributeTypeDto : IMapFrom<ProductAttributeType>
{
    public AllProductAttributeTypeDto()
    {
        AttributeGroups = new List<ProductTypeAttributeGroupDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public IList<ProductTypeAttributeGroupDto> AttributeGroups { get; set; }
}
public class ProductTypeAttributeGroupDto : IMapFrom<ProductTypeAttributeGroup>
{
    public ProductTypeAttributeGroupDto()
    {
        ProductTypeAttributeGroupCustomFields = new List<ProductTypeAttributeGroupCustomFieldDto>();
        Attributes = new List<ProductTypeAttributeGroupAttributeDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public List<ProductTypeAttributeGroupCustomFieldDto> ProductTypeAttributeGroupCustomFields { get; set; }
    public List<ProductTypeAttributeGroupAttributeDto> Attributes { get; set; }
}
public class ProductTypeAttributeGroupAttributeDto : IMapFrom<ProductTypeAttributeGroupAttribute>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
}