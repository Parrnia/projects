using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetRelatedProductsWithPagination;

public class RelatedProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public RelatedProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class RelatedProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, RelatedProductWithPaginationDto>();
    }
}