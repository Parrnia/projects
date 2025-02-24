using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.RequestsCluster.RequestLogs.Commands.DeleteRequest;
using Onyx.Application.Main.RequestsCluster.RequestLogs.Commands.ResendAllRequests;
using Onyx.Application.Main.RequestsCluster.RequestLogs.Commands.ResendRequests;
using Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice;
using Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice.GetRequestLogsWithPagination;
using Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.Export.ExportExcelRequestLogs;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.RequestsCluster;


public class RequestLogsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredRequestLogDto>> GetRequestLogsWithPagination([FromQuery] GetRequestLogsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelRequestLogsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "RequestLogs.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost("all")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> ResendAllCancelOrderRequests()
    {
        return await Mediator.Send(new ResendAllCancelOrderRequestsCommand());
    }

    [HttpPost("some")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> ResendCancelOrderRequests(ResendCancelOrderRequestsCommand resendCancelOrderRequestsCommand)
    {
        return await Mediator.Send(resendCancelOrderRequestsCommand);
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeRequestLog([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeRequestLogCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
