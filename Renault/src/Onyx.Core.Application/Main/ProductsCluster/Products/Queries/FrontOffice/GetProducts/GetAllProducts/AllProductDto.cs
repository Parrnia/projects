using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetAllProducts;

public class AllProductModelHelperDto : IMapFrom<ProductModelHelper>
{
    public AllProductDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class AllProductDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, AllProductDto>();
    }
}
