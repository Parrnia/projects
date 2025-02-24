using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByProductCategorySlug;

public class ProductByProductCategorySlugModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductByProductCategorySlugDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductByProductCategorySlugDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByProductCategorySlugDto>();
    }
}