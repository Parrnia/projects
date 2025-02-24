using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByProductCategoryId;

public class ProductByProductCategoryIdModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductByProductCategoryIdDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductByProductCategoryIdDto : MainProductDto, IMapFrom<Product>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByProductCategoryIdDto>();
    }
}