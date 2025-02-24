using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class BrandForProductDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public byte[] BrandLogo { get; set; } = null!;
    public string? LocalizedName { get; set; }
    public string Name { get; set; } = null!;
    public int Code { get; set; }
    public string? Slug { get; set; }
    public CountryDto? Country { get; set; }
}
