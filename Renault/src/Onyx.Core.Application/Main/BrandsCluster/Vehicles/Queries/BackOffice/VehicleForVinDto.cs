using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice;
public class VehicleForVinDto : IMapFrom<Vehicle>
{
    public int Id { get; set; }
    public string VinNumber { get; set; } = null!;
    public KindForVehicleDto Kind { get; set; } = null!;
}
