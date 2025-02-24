using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.FrontOffice.GetProductTypeAttributeGroupAttribute;
public class ProductTypeAttributeGroupAttributeByIdDto : IMapFrom<ProductTypeAttributeGroupAttribute>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
}
