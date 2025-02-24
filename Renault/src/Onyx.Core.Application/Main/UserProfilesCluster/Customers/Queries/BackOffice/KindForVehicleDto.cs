using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice;

public class KindForVehicleDto : IMapFrom<Kind>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BrandName { get; set; } = null!;
    public string BrandLocalizedName { get; set; } = null!;
    public string FamilyName { get; set; } = null!;
    public string FamilyLocalizedName { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string ModelLocalizedName { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Kind, KindForVehicleDto>()
            .ForMember(d => d.BrandName,
                opt => opt.MapFrom(s => s.Model.Family.VehicleBrand.Name))
            .ForMember(d => d.BrandLocalizedName,
            opt => opt.MapFrom(s => s.Model.Family.VehicleBrand.LocalizedName))
            .ForMember(d => d.FamilyName,
                opt => opt.MapFrom(s => s.Model.Family.Name))
            .ForMember(d => d.FamilyLocalizedName,
                opt => opt.MapFrom(s => s.Model.Family.LocalizedName))
            .ForMember(d => d.ModelName,
                opt => opt.MapFrom(s => s.Model.Name))
            .ForMember(d => d.ModelLocalizedName,
                opt => opt.MapFrom(s => s.Model.LocalizedName));
    }
}