using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice;
public class VehicleDto : IMapFrom<Vehicle>
{
    public int Id { get; set; }
    public string VinNumber { get; set; } = null!;
    public int KindId { get; set; }
    public string KindName { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Vehicle, VehicleDto>()
            .ForMember(d => d.KindId, opt => opt.MapFrom(s => s.KindId))
            .ForMember(d => d.KindName, opt => opt.MapFrom(s => s.Kind.LocalizedName))
            .ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.CustomerId));
    }

}
