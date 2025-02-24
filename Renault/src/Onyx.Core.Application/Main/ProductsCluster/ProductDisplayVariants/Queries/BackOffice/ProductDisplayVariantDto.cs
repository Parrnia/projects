using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice;
public class ProductDisplayVariantDto : IMapFrom<ProductDisplayVariant>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int ProductId { get; set; }
    public bool IsBestSeller { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsLatest { get; set; }
    public bool IsNew { get; set; }
    public bool IsPopular { get; set; }
    public bool IsSale { get; set; }
    public bool IsSpecialOffer { get; set; }
    public bool IsTopRated { get; set; }
    public bool IsActive { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductDisplayVariant, ProductDisplayVariantDto>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.LocalizedName));
    }
}
