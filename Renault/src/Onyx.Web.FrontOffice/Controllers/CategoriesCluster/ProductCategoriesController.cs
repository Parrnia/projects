
using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategories.GetAllFirstLayerCategories;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategories.GetAllProductCategories;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategories.GetProductCategoriesByProductParentCategoryId;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetAllProductCategoriesWithPagination;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetFeaturedFirstProductCategoriesWithPagination;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetPopularFirstCategoriesWithPagination;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategory.GetProductCategoryById;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategory.GetProductCategoryBySlug;

namespace Onyx.Web.FrontOffice.Controllers.CategoriesCluster;

public class ProductCategoriesController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllProductCategoryDto>>> GetAllProductCategories()
    {
        return await Mediator.Send(new GetAllProductCategoriesQuery());
    }

    [HttpGet("allFirstLayer")]
    public async Task<ActionResult<List<AllFirstLayerProductCategoryDto>>> GetAllFirstLayerProductCategories()
    {
        return await Mediator.Send(new GetAllFirstLayerProductCategoriesQuery());
    }

    [HttpGet("allWithPagination")]
    public async Task<ActionResult<PaginatedList<AllProductCategoryWithPaginationDto>>> GetAllProductCategoriesWithPagination([FromQuery] GetAllProductCategoriesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("featuredFirstWithPagination")]
    public async Task<ActionResult<PaginatedList<FeaturedFirstProductCategoryWithPaginationDto>>> GetFeaturedFirstProductCategoriesWithPagination([FromQuery] GetFeaturedFirstProductCategoriesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("popularFirstWithPagination")]
    public async Task<ActionResult<PaginatedList<PopularFirstProductCategoryWithPaginationDto>>> GetPopularFirstProductCategoriesWithPagination([FromQuery] GetPopularFirstProductCategoriesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("productParentCategory")]
    public async Task<ActionResult<List<ProductCategoryByProductParentCategoryIdDto>>> GetProductCategoriesByProductParentCategoryId([FromQuery] GetProductCategoriesByProductParentCategoryIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductCategoryByIdDto?>> GetProductCategoryById(int id)
    {
        return await Mediator.Send(new GetProductCategoryByIdQuery(id));
    }

    [HttpGet("slug")]
    public async Task<ActionResult<ProductCategoryBySlugDto?>> GetProductCategoryBySlug([FromQuery] GetProductCategoryBySlugQuery query)
    {
        return await Mediator.Send(query);
    }
}
