using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.BackOffice.GetReturnOrderItemGroups;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.Export.ExportExcelReturnOrderItemGroups;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrderItemGroupsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemGroupDto>>> GetReturnOrderItemGroupsByReturnOrderId([FromQuery] GetReturnOrderItemGroupsByReturnOrderIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReturnOrderItemGroupsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReturnOrderItemGroups.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }
}
