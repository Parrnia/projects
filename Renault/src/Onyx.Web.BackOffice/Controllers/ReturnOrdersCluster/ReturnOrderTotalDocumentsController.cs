using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.CreateReturnOrderTotalDocument;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.DeleteReturnOrderTotalDocument;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.UpdateReturnOrderTotalDocument;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.BackOffice.GetReturnOrderTotalDocuments;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.Export.ExportExcelReturnOrderTotalDocuments;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.DeleteReturnOrderTotal;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrderTotalDocumentsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderTotalDocumentDto>>> GetReturnOrderTotalDocumentsByReturnOrderTotalId([FromQuery] GetReturnOrderTotalDocumentsByReturnOrderTotalIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReturnOrderTotalDocumentsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReturnOrderTotalDocuments.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateReturnOrderTotalDocumentCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateReturnOrderTotalDocumentCommand command)
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
    public async Task<ActionResult> Delete(int id, DeleteReturnOrderTotalDocumentCommand command)
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
    public async Task<IActionResult> DeleteRange(DeleteRangeReturnOrderTotalDocumentCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}
