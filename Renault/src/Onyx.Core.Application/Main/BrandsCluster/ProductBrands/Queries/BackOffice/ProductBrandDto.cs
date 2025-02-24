using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice;
public class ProductBrandDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public Guid? BrandLogo { get; set; }
    public string? LocalizedName { get; set; }
    public string Name { get; set; } = null!;
    public int Code { get; set; }
    public string? Slug { get; set; }
    public int? CountryId { get; set; }
    public string? CountryName { get; set; }
    public bool IsActive { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductBrand, ProductBrandDto>()
            .ForMember(d => d.CountryId, opt => opt.MapFrom(s => s.CountryId))
            .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : null));
    }
}
