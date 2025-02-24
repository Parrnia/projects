using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.CreateComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.DeleteWidgetComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.UpdateWidgetComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComments;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;

namespace Onyx.Web.BackOffice.Controllers.BlogsCluster;

public class WidgetCommentsController : ApiControllerBase
{
    [HttpGet("customer{customerId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<WidgetCommentDto>>> GetWidgetCommentsByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetWidgetCommentsByCustomerIdQuery(customerId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<WidgetCommentDto?>> GetWidgetCommentById(int id)
    {
        return await Mediator.Send(new GetWidgetCommentByIdQuery(id));
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
    public async Task<ActionResult> Update(int id, UpdateWidgetCommentCommand command)
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
        await Mediator.Send(new DeleteWidgetCommentCommand(id,null));

        return NoContent();
    }
    [HttpGet("selfCustomer")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<WidgetCommentDto>>> SelfGetWidgetCommentsByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetWidgetCommentsByCustomerIdQuery(UserInfo.UserId));
    }
    [HttpPut("self{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BlogManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> SelfUpdate(int id, UpdateWidgetCommentCommand command)
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
        await Mediator.Send(new DeleteWidgetCommentCommand(id, UserInfo.UserId));

        return NoContent();
    }
}
