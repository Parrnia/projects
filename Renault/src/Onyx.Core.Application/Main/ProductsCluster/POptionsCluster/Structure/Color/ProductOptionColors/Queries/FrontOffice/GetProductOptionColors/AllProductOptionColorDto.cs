using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.FrontOffice.GetProductOptionColors;
public class AllProductOptionColorDto : IMapFrom<ProductOptionColor>
{
    public AllProductOptionColorDto()
    {
        Values = new List<ProductOptionValueColorDto>();
        ProductOptionColorCustomFields = new List<ProductOptionColorCustomFieldDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueColorDto> Values { get; set; }
    public List<ProductOptionColorCustomFieldDto> ProductOptionColorCustomFields { get; set; }
}