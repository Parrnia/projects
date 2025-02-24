using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BlogsCluster.Comments.Commands.CreateComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.DeleteWidgetComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.UpdateWidgetComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComment;
using Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComments;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared;


namespace Onyx.Web.FrontOffice.Controllers.BlogsCluster;


public class WidgetCommentsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<WidgetCommentDto?>> GetWidgetCommentById(int id)
    {
        return await Mediator.Send(new GetWidgetCommentByIdQuery(id));
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
    [HttpGet("selfCustomer")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<WidgetCommentDto>>> SelfGetWidgetCommentsByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetWidgetCommentsByCustomerIdQuery(UserInfo.UserId));
    }
    [HttpPut("self{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> SelfUpdate(int id, UpdateWidgetCommentCommand command)
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
        await Mediator.Send(new DeleteWidgetCommentCommand(id, UserInfo.UserId));

        return NoContent();
    }
}
