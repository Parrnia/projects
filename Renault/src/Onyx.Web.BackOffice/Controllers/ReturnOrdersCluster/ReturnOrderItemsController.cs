using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.InfoCluster.Questions.Commands.DeleteQuestion;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.DeleteReturnOrderItemDocument;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.CreateReturnOrderItem;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.DeleteReturnOrderItem;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.UpdateReturnOrderItem;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice.GetReturnOrderItems;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.Export.ExportExcelReturnOrderItems;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrderItemsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemDto>>> GetReturnOrderItemsByReturnOrderItemGroupId([FromQuery] GetReturnOrderItemsByReturnOrderItemGroupIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReturnOrderItemsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReturnOrderItems.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateReturnOrderItemCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateReturnOrderItemCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id, DeleteReturnOrderItemCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange(DeleteRangeReturnOrderItemCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}
