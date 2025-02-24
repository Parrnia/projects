using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice;
public class ProductAttributeValueCustomFieldDto : IMapFrom<ProductAttributeValueCustomField>
{
    public int Id { get; set; }
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
