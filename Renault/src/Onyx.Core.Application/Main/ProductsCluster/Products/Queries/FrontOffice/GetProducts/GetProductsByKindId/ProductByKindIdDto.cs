using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByKindId;

public class ProductByKindIdModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductByKindIdDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductByKindIdDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByKindIdDto>();
    }
}