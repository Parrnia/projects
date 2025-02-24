using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice;
public class ProductOptionMaterialCustomFieldDto : IMapFrom<ProductOptionMaterialCustomField>
{
    public int Id { get; set; }
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
