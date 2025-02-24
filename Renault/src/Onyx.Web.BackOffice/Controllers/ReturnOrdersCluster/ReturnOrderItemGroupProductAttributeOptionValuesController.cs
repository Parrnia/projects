using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.BackOffice.GetOptionValues;
using Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.Export.ExportExcelOptionValues;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrderItemGroupProductAttributeOptionValuesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemGroupProductAttributeOptionValueDto>>> GetOptionValuesByReturnOrderItemGroupId([FromQuery] GetOptionValuesByReturnOrderItemGroupIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReturnOrderItemGroupOptionValuesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReturnOrderItemGroupProductAttributeOptionValues.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }
}
