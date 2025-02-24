using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice;
public class ProductCustomFieldDto : IMapFrom<ProductCustomField>
{
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
