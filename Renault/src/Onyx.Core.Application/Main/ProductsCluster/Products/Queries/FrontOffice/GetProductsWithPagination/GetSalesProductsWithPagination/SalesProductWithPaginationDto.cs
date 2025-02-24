using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetSalesProductsWithPagination;

public class SalesProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public SalesProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class SalesProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, SalesProductWithPaginationDto>();
    }
}