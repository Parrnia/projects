using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetNewProductsWithPagination;

public class NewProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public NewProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class NewProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, NewProductWithPaginationDto>();
    }
}