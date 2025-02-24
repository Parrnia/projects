using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.FrontOffice.GetOptionValueColors.GetByColorId;
public class OptionValueColorByColorIdDto : IMapFrom<ProductOptionValueColor>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Color { get; set; } = null!;
}
