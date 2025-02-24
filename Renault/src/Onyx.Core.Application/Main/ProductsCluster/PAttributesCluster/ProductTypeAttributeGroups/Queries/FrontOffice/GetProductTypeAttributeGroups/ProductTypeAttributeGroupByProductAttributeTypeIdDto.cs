using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.FrontOffice.GetProductTypeAttributeGroups;
public class ProductTypeAttributeGroupByProductAttributeTypeIdDto : IMapFrom<ProductTypeAttributeGroup>
{
    public ProductTypeAttributeGroupByProductAttributeTypeIdDto()
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


