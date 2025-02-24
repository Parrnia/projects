using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetFeaturedProductsWithPagination;

public class FeaturedProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public FeaturedProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class FeaturedProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, FeaturedProductWithPaginationDto>();
    }
}