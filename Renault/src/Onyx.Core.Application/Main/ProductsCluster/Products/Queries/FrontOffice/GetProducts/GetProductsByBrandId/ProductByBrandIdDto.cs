using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByBrandId;

public class ProductByBrandIdModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductByBrandIdDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductByBrandIdDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByBrandIdDto>();
    }
}