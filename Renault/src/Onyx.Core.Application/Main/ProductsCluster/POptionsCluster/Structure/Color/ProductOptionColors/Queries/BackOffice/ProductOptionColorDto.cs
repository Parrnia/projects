using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice;
public class ProductOptionColorDto : IMapFrom<ProductOptionColor>
{
    public ProductOptionColorDto()
    {
        Values = new List<ProductOptionValueColorDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueColorDto> Values { get; set; }
}
