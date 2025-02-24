using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetBestSellerProductsWithPagination;

public class BestSellerProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public BestSellerProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class BestSellerProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, BestSellerProductWithPaginationDto>();
    }
}