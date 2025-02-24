using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetAllVehicles;
public class AllVehicleDto : IMapFrom<Vehicle>
{
    public int Id { get; set; }
    public string VinNumber { get; set; } = null!;
    public KindDto Kind { get; set; } = null!;
    public List<CustomerDto> Customers { get; set; } = new List<CustomerDto>();
}
public class KindDto : IMapFrom<Kind>
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
        profile.CreateMap<Kind, KindDto>()
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
public class CustomerDto : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public Guid? Avatar { get; set; }
}