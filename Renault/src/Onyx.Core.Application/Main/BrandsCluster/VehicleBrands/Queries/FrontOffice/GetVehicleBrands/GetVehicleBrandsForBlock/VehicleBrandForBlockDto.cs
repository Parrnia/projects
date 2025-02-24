using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrands.GetVehicleBrandsForBlock;

public class VehicleBrandForBlockDto : IMapFrom<VehicleBrand>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public Guid? BrandLogo { get; set; }
    public string CountryName { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<VehicleBrand, VehicleBrandForBlockDto>()
            .ForMember(d => d.CountryName,
                opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : ""));
    }
}
