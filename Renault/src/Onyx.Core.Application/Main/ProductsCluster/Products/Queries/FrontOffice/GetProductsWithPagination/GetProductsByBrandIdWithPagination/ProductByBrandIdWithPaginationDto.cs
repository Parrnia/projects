using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByBrandIdWithPagination;

public class ProductByBrandIdWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductByBrandIdWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductByBrandIdWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByBrandIdWithPaginationDto>();
    }
}