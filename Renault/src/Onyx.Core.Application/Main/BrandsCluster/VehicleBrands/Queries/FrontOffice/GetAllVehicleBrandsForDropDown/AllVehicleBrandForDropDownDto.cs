using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetAllVehicleBrandsForDropDown;

public class AllVehicleBrandForDropDownDto : IMapFrom<VehicleBrand>
{
    public int Id { get; set; }
    public string? LocalizedName { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
}