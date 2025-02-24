using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;
public class AllProductAttributeTypeDropDownDto : IMapFrom<ProductAttributeType>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
