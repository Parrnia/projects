
using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.Credits.Queries.Export.ExportExcelCredits;
using Onyx.Application.Main.UserProfilesCluster.Credits.Queries.GetCreditsWithPagination;
using Onyx.Application.Main.UserProfilesCluster.MaxCredits.Queries.Export.ExportExcelMaxCredits;
using Onyx.Application.Main.UserProfilesCluster.MaxCredits.Queries.GetMaxCreditsWithPagination;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.UserProfilesCluster;

public class CreditsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerCreditManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredCreditDto>> GetCreditsByCustomerIdWithPagination([FromQuery] GetCreditsByCustomerIdWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerCreditManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCreditsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Credits.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("max")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerCreditManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredMaxCreditDto>> GetMaxCreditsByCustomerIdWithPagination([FromQuery] GetMaxCreditsByCustomerIdWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcelMax")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerCreditManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportMaxExcelQuery([FromQuery] ExportExcelMaxCreditsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Credits.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }
}
