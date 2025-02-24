using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetPopularProductsWithPagination;

public class PopularProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public PopularProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class PopularProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, PopularProductWithPaginationDto>();
    }
}