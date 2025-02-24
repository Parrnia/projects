using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.DeleteReturnOrderItem;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.CreateReturnOrderTotal;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.DeleteReturnOrderTotal;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.UpdateReturnOrderTotal;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotal.GetSendReturnOrderTotal;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotals;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.Export.ExportExcelReturnOrderTotals;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrderTotalsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderTotalDto>>> GetReturnOrderTotalsByReturnOrderId([FromQuery] GetReturnOrderTotalsByReturnOrderIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReturnOrderTotalsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReturnOrderTotals.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateReturnOrderTotalCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateReturnOrderTotalCommand command)
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
    public async Task<ActionResult> Delete(int id, DeleteReturnOrderTotalCommand command)
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
    public async Task<IActionResult> DeleteRange(DeleteRangeReturnOrderTotalCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}
