using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BlogsCluster.Posts.Validators;
using Onyx.Application.Main.BlogsCluster.Posts.Commands.CreatePost;
using Onyx.Application.Main.BlogsCluster.Posts.Commands.DeletePost;
using Onyx.Application.Main.BlogsCluster.Posts.Commands.UpdatePost;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPost;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPosts;
using Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPostsWithPagination;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;

namespace Onyx.Web.BackOffice.Controllers.BlogsCluster;

public class PostsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<PostDto>>> GetPostsWithPagination([FromQuery] GetPostsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<PostDto>>> GetAllPosts()
    {
        return await Mediator.Send(new GetAllPostsQuery());
    }

    [HttpGet("blogSubCategory{blogSubCategoryId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<PostDto>>> GetPostsByBlogSubCategoryId(int blogSubCategoryId)
    {
        return await Mediator.Send(new GetPostsByBlogSubCategoryIdQuery(blogSubCategoryId));
    }

    [HttpGet("user{userId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<PostDto>>> GetPostsByUserId(Guid userId)
    {
        return await Mediator.Send(new GetPostsByUserIdQuery(userId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PostDto?>> GetPostById(int id)
    {
        return await Mediator.Send(new GetPostByIdQuery(id));
    }
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreatePostCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.AuthorId = UserInfo.UserId;

        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdatePostCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePostCommand(id, null));

        return NoContent();
    }
    [HttpPut("self{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> SelfUpdate(int id, UpdatePostCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("self{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> SelfDelete(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        await Mediator.Send(new DeletePostCommand(id, UserInfo.UserId));

        return NoContent();
    }
    //Validators
    [HttpGet("isUniqueTitle")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniquePostTitle([FromQuery] UniquePostTitleValidator query)
    {
        return await Mediator.Send(query);
    }
}
