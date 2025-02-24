using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;
public class ProductAttributeTypeDto : IMapFrom<ProductAttributeType>
{
    public ProductAttributeTypeDto()
    {
        AttributeGroups = new List<ProductTypeAttributeGroupDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public IList<ProductTypeAttributeGroupDto> AttributeGroups { get; set; }
}
