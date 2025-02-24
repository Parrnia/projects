using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderForReturnById;

public class ProductAttributeOptionForReturnByIdDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public string? ProductNo { get; set; } = null!;
    public string ProductBrandName { get; set; } = null!;
    public string ProductCategory { get; set; } = null!;
    public Guid ProductImage { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductAttributeOption, ProductAttributeOptionForReturnByIdDto>()
            .ForMember(d => d.ProductName,
                opt =>
                    opt.MapFrom(s => s.Product.LocalizedName))
            .ForMember(d => d.ProductNo,
                opt =>
                    opt.MapFrom(s => s.Product.ProductNo))
            .ForMember(d => d.ProductBrandName,
                opt =>
                    opt.MapFrom(s => s.Product.ProductBrand.LocalizedName))
            .ForMember(d => d.ProductCategory,
                opt =>
                    opt.MapFrom(s => s.Product.ProductCategory.LocalizedName))
            .ForMember(d => d.ProductImage,
                opt =>
                    opt.MapFrom(s => s.Product.Images.OrderBy(c => c.Order).FirstOrDefault() != null ? s.Product.Images.OrderBy(c => c.Order).FirstOrDefault().Image : Guid.Empty));
    }
}