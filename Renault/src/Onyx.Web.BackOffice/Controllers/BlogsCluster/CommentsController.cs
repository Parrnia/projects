using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.CreateComment;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.DeleteComment;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.UpdateComment;
using Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice;
using Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComment;
using Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComments;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.BlogsCluster;

public class CommentsController : ApiControllerBase
{
    [HttpGet("parentComment{parentCommentId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsByParentCommentId(int parentCommentId)
    {
        return await Mediator.Send(new GetCommentsByParentCommentIdQuery(parentCommentId));
    }

    [HttpGet("post{postId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsByPostId(int postId)
    {
        return await Mediator.Send(new GetCommentsByPostIdQuery(postId));
    }
    [HttpGet("customer{customerId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetCommentsByCustomerIdQuery(customerId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CommentDto?>> GetCommentById(int id)
    {
        return await Mediator.Send(new GetCommentByIdQuery(id));
    }
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCommentCommand command)
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
    public async Task<ActionResult> Update(int id, UpdateCommentCommand command)
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
        await Mediator.Send(new DeleteCommentCommand(id,null));

        return NoContent();
    }
}
