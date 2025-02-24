using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice;
public class FamilyDto : IMapFrom<Family>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int BrandId { get; set; }
    public string BrandName { get; set; } = null!;
    public bool IsActive { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Family, FamilyDto>()
            .ForMember(d => d.BrandId, opt => opt.MapFrom(s => s.VehicleBrandId))
            .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.VehicleBrand.LocalizedName));
    }
}
