using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.CreateComment;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.DeleteComment;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.UpdateComment;
using Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice;
using Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComment;
using Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComments;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;


namespace Onyx.Web.FrontOffice.Controllers.BlogsCluster;


//[PermissionAuthorize]
public class CommentsController : ApiControllerBase
{
    [HttpGet("parentComment{parentCommentId}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsByParentCommentId(int parentCommentId)
    {
        return await Mediator.Send(new GetCommentsByParentCommentIdQuery(parentCommentId));
    }

    [HttpGet("post{postId}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsByPostId(int postId)
    {
        return await Mediator.Send(new GetCommentsByPostIdQuery(postId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto?>> GetCommentById(int id)
    {
        return await Mediator.Send(new GetCommentByIdQuery(id));
    }
    [HttpGet("selfCustomer")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<CommentDto>>> SelfGetCommentsByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetCommentsByCustomerIdQuery(UserInfo.UserId));
    }
    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> Create(CreateCommentCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.AuthorId = UserInfo.UserId;

        return await Mediator.Send(command);
    }
    [HttpPut("self{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> SelfUpdate(int id, UpdateCommentCommand command)
    {
        if (id != command.Id || UserInfo?.UserId != command.AuthorId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("self{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> SelfDelete(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        await Mediator.Send(new DeleteCommentCommand(id, UserInfo.UserId));

        return NoContent();
    }
}
