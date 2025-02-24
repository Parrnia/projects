using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice;
public class ProductOptionMaterialDto : IMapFrom<ProductOptionMaterial>
{
    public ProductOptionMaterialDto()
    {
        Values = new List<ProductOptionValueMaterialDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueMaterialDto> Values { get; set; }
}
