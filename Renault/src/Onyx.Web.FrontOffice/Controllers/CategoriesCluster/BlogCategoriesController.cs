using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice.GetBlogCategories;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice.GetBlogCategory;

namespace Onyx.Web.FrontOffice.Controllers.CategoriesCluster;

public class BlogCategoriesController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<BlogCategoryDto>>> GetAllBlogCategories()
    {
        return await Mediator.Send(new GetAllBlogCategoriesQuery());
    }

    [HttpGet("blogParentCategory")]
    public async Task<ActionResult<List<BlogCategoryDto>>> GetBlogCategoriesByBlogParentCategoryId([FromQuery] GetBlogCategoriesByBlogParentCategoryIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BlogCategoryDto?>> GetBlogCategoryById(int id)
    {
        return await Mediator.Send(new GetBlogCategoryByIdQuery(id));
    }

    [HttpGet("slug")]
    public async Task<ActionResult<BlogCategoryDto?>> GetBlogCategoryBySlug([FromQuery] GetBlogCategoryBySlugQuery query)
    {
        return await Mediator.Send(query);
    }
}
