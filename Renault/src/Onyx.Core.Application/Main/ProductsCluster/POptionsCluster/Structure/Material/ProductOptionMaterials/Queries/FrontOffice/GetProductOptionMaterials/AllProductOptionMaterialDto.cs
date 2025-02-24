using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.FrontOffice.GetProductOptionMaterials;
public class AllProductOptionMaterialDto : IMapFrom<ProductOptionMaterial>
{
    public AllProductOptionMaterialDto()
    {
        Values = new List<ProductOptionValueMaterialDto>();
        ProductOptionMaterialCustomFields = new List<ProductOptionMaterialCustomFieldDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueMaterialDto> Values { get; set; }
    public List<ProductOptionMaterialCustomFieldDto> ProductOptionMaterialCustomFields { get; set; }
}