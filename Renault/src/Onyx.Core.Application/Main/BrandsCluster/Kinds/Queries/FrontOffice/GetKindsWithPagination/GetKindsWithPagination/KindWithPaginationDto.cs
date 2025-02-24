using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKindsWithPagination.GetKindsWithPagination;
public class KindWithPaginationDto : IMapFrom<Kind>
{
    public KindWithPaginationDto()
    {
        Products = new List<ProductDto>();
        Vehicles = new List<VehicleDto>();
    }
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IList<ProductDto> Products { get; set; }
    public IList<VehicleDto> Vehicles { get; set; }
}
public class ProductDto : IMapFrom<Product>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string? ProductNo { get; set; }
    public string? OldProductNo { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ProductCatalog { get; set; }
    public decimal OrderRate { get; set; }
    public decimal? Height { get; set; }
    public decimal? Width { get; set; }
    public decimal? Length { get; set; }
    public decimal? NetWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public decimal? VolumeWeight { get; set; }
    public int? Mileage { get; set; }
    public int? Duration { get; set; }
    public bool DirectSalesLicense { get; set; }
    public string Excerpt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Sku { get; set; }
    public bool IsActive { get; set; }
}
public class VehicleDto : IMapFrom<Vehicle>
{
    public int Id { get; set; }
    public string VinNumber { get; set; } = null!;
    public bool IsActive { get; set; }
}