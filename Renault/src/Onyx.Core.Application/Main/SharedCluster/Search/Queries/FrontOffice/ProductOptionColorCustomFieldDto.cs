using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice;
public class ProductOptionColorCustomFieldDto : IMapFrom<ProductOptionColorCustomField>
{
    public int Id { get; set; }
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
