using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetFilterResults;
public class ProductForFilterResultModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductForFilterResultDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductForFilterResultDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductForFilterResultDto>();
    }
}