using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice;
public class AllProductOptionColorDropDownDto : IMapFrom<ProductOptionColor>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
