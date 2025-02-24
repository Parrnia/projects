using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class ProductCustomFieldDto : IMapFrom<ProductCustomField>
{
    public ProductCustomFieldDto()
    {
    }
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
