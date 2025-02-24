using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice;
public class AllVehicleBrandDropDownDto : IMapFrom<VehicleBrand>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}
