using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByProductCategoryIdWithPagination;

public class ProductByProductCategoryIdWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductByProductCategoryIdWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductByProductCategoryIdWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByProductCategoryIdWithPaginationDto>();
    }
}