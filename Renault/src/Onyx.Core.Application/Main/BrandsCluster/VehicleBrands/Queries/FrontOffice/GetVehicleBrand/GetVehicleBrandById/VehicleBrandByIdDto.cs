using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrand.GetVehicleBrandById;
public class VehicleBrandByIdDto : IMapFrom<VehicleBrand>
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