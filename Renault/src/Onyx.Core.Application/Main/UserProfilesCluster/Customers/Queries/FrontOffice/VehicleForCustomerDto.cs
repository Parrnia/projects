using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice;
public class VehicleForCustomerDto : IMapFrom<Vehicle>
{
    public int Id { get; set; }
    public string VinNumber { get; set; } = null!;
    public KindForVehicleDto Kind { get; set; } = null!;
}