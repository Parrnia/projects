using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Queries.FrontOffice.GetOptionValueMaterials.GetAllOptionValueMaterials;
public class AllProductOptionValueMaterialDto : IMapFrom<ProductOptionValueMaterial>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
}
