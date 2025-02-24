using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice;
public class AllProductTypeDropDownDto : IMapFrom<ProductType>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}
