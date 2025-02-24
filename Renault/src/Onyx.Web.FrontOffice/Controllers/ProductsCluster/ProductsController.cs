using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductById;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductBySlug;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetAllProducts;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByBrandId;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByKindId;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByProductCategoryId;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByProductCategorySlug;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetBestSellerProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetFeaturedProductsByProductCategoryIdWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetFeaturedProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetLatestProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetNewProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetPopularProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByBrandIdWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByKindIdWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByProductCategoryIdWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetRelatedProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetSalesProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetSpecialOffersProductsWithPagination;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetTopRatedProductsWithPagination;
using Onyx.Domain.Enums;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;

public class ProductsController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductDto>>> GetAllProducts([FromQuery] GetAllProductsQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("productCategoryId")]
    public async Task<ActionResult<PaginatedList<ProductByProductCategoryIdWithPaginationDto>>> GetProductsByProductCategoryIdWithPagination([FromQuery] GetProductsByProductCategoryIdWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("featuredProductCategoryId")]
    public async Task<ActionResult<PaginatedList<FeaturedProductByProductCategoryIdWithPaginationDto>>> GetFeaturedProductsByProductCategoryIdWithPagination([FromQuery] GetFeaturedProductsByProductCategoryIdWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductWithPaginationDto>>> GetProductsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("kind")]
    public async Task<ActionResult<List<ProductByKindIdDto>>> GetProductsByKindId([FromQuery] GetProductsByKindIdQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("productSubCategoryId")]
    public async Task<ActionResult<List<ProductByProductCategoryIdDto>>> GetProductsByProductSubCategoryId([FromQuery] GetProductsByProductCategoryIdQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("productSubCategorySlug")]
    public async Task<ActionResult<List<ProductByProductCategorySlugDto>>> GetProductsByProductSubCategorySlug([FromQuery] GetProductsByProductCategorySlugQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("brand")]
    public async Task<ActionResult<List<ProductByBrandIdDto>>> GetProductsByBrandId([FromQuery] GetProductsByBrandIdQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("slug")]
    public async Task<ActionResult<ProductBySlugDto?>> GetProductBySlug([FromQuery] GetProductBySlugQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("ById")]
    public async Task<ActionResult<ProductByIdDto?>> GetProductById([FromQuery] GetProductByIdQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("new")] //تازه ها
    public async Task<ActionResult<PaginatedList<NewProductWithPaginationDto>>> GetNewProductsWithPagination([FromQuery] GetNewProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("popular")] //محبوب
    public async Task<ActionResult<PaginatedList<PopularProductWithPaginationDto>>> GetPopularProductsWithPagination([FromQuery] GetPopularProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("sales")] //حراجی
    public async Task<ActionResult<PaginatedList<SalesProductWithPaginationDto>>> GetSalesProductsWithPagination([FromQuery] GetSalesProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("topRated")] //بالاترین امتیاز
    public async Task<ActionResult<PaginatedList<TopRatedProductWithPaginationDto>>> GetTopRatedProductsWithPagination([FromQuery] GetTopRatedProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("specialOffers")] //پیشنها ویژه
    public async Task<ActionResult<PaginatedList<SpecialOffersProductWithPaginationDto>>> GetSpecialOffersProductsWithPagination([FromQuery] GetSpecialOffersProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }

    [HttpGet("bestSellers")] //پر فروش ها
    public async Task<ActionResult<PaginatedList<BestSellerProductWithPaginationDto>>> GetBestSellersProductsWithPagination([FromQuery] GetBestSellerProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }
    [HttpGet("featured")] //ویژه
    public async Task<ActionResult<PaginatedList<FeaturedProductWithPaginationDto>>> GetFeaturedProductsWithPagination([FromQuery] GetFeaturedProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }
    [HttpGet("latest")] //آخرین
    public async Task<ActionResult<PaginatedList<LatestProductWithPaginationDto>>> GetLatestProductsWithPagination([FromQuery] GetLatestProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }
    [HttpGet("related")] //مرتبط
    public async Task<ActionResult<PaginatedList<RelatedProductWithPaginationDto>>> GetRelatedProductsWithPagination([FromQuery] GetRelatedProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }
    [HttpGet("brandIdWithPagination")]
    public async Task<ActionResult<PaginatedList<ProductByBrandIdWithPaginationDto>>> GetProductsByBrandIdWithPagination([FromQuery] GetProductsByBrandIdWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }
    [HttpGet("kindIdWithPagination")]
    public async Task<ActionResult<PaginatedList<ProductByKindIdWithPaginationDto>>> GetProductsByKindIdWithPagination([FromQuery] GetProductsByKindIdWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }
}
