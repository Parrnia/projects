using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.FrontOffice;
public class ProductTypeAttributeGroupCustomFieldDto : IMapFrom<ProductTypeAttributeGroupCustomField>
{
    public int Id { get; set; }
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
