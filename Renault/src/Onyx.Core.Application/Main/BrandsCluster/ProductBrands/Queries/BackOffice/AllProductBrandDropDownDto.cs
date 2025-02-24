using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice;
public class AllProductBrandDropDownDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}
