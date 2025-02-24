using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetSpecialOffersProductsWithPagination;

public class SpecialOffersProductWithPaginationModelHelperDto : IMapFrom<ProductModelHelper>
{
    public SpecialOffersProductWithPaginationDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class SpecialOffersProductWithPaginationDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, SpecialOffersProductWithPaginationDto>();
    }
}