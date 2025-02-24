using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetTopRatedProductsWithPagination;

public class TopRatedProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public TopRatedProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class TopRatedProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, TopRatedProductWithPaginationDto>();
    }
}