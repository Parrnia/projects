using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrandsWithPagination.GetProductBrandsWithPagination;
public class ProductBrandWithPaginationDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public Guid? BrandLogo { get; set; } = null!;
    public string? LocalizedName { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public CountryDto? Country { get; set; }
}
public class CountryDto : IMapFrom<Country>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
}
public class ProductDto : IMapFrom<Product>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Sku { get; set; }
    public bool IsActive { get; set; }

}