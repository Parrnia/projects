using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice;
public class CustomerForVehicleDto : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public byte[]? Avatar { get; set; }
}