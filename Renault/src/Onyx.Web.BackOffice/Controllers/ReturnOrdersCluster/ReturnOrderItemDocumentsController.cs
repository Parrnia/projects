using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BrandsCluster.Models.Commands.DeleteModel;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.CreateReturnOrderItemDocument;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.DeleteReturnOrderItemDocument;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.UpdateReturnOrderItemDocument;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.BackOffice.GetReturnOrderItemDocuments;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.Export.ExportExcelReturnOrderItemDocuments;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrderItemDocumentsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemDocumentDto>>> GetReturnOrderItemDocumentsByReturnOrderItemId([FromQuery] GetReturnOrderItemDocumentsByReturnOrderItemIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReturnOrderItemDocumentsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReturnOrderItemDocuments.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateReturnOrderItemDocumentCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateReturnOrderItemDocumentCommand command)
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
    public async Task<ActionResult> Delete(int id, DeleteReturnOrderItemDocumentCommand command)
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
    public async Task<IActionResult> DeleteRange(DeleteRangeReturnOrderItemDocumentCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}
