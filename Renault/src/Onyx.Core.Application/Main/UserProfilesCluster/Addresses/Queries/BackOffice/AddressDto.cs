using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice;
public class AddressDto : IMapFrom<Address>
{ 
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Company { get; set; }
    public string AddressDetails1 { get; set; } = null!;
    public string? AddressDetails2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public bool Default { get; set; }
    public int? CountryId { get; set; }
    public string? CountryName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Address, AddressDto>()
            .ForMember(d => d.CountryId, opt => opt.MapFrom(s => s.CountryId))
            .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.Country.LocalizedName));
    }
}
