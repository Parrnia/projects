using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByKindIdWithPagination;

public class ProductByKindIdWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductByKindIdWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductByKindIdWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByKindIdWithPaginationDto>();
    }
}