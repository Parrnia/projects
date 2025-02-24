using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrands.GetProductBrandsForBlock;

public class ProductBrandForBlockDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public Guid? BrandLogo { get; set; }
    public string CountryName { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductBrand, ProductBrandForBlockDto>()
            .ForMember(d => d.CountryName,
                opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : ""))
            .ForMember(d => d.Name,
                opt => opt.MapFrom(s => s.LocalizedName));
    }
}
