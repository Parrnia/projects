using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPost;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPosts;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPostsWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.BlogsCluster;


public class PostsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<PostDto>>> GetPostsWithPagination([FromQuery] GetPostsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<PostDto>>> GetAllPosts()
    {
        return await Mediator.Send(new GetAllPostsQuery());
    }

    [HttpGet("blogSubCategory{blogSubCategoryId}")]
    public async Task<ActionResult<List<PostDto>>> GetPostsByBlogSubCategoryId(int blogSubCategoryId)
    {
        return await Mediator.Send(new GetPostsByBlogSubCategoryIdQuery(blogSubCategoryId));
    }

    [HttpGet("user{userId}")]
    public async Task<ActionResult<List<PostDto>>> GetPostsByUserId(Guid userId)
    {
        return await Mediator.Send(new GetPostsByUserIdQuery(userId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto?>> GetPostById(int id)
    {
        return await Mediator.Send(new GetPostByIdQuery(id));
    }
}