using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetFeaturedProductsByProductCategoryIdWithPagination;

public class FeaturedProductByProductCategoryIdWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public FeaturedProductByProductCategoryIdWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class FeaturedProductByProductCategoryIdWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, FeaturedProductByProductCategoryIdWithPaginationDto>();
    }
}