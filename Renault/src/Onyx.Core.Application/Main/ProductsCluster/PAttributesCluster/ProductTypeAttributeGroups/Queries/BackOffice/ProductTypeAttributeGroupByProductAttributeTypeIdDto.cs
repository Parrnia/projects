using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice;
public class ProductTypeAttributeGroupByProductAttributeTypeIdDto : IMapFrom<ProductTypeAttributeGroup>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}


