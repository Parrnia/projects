using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddEmailAddress;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddLocationAddress;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddPhoneNumber;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddWorkingHour;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.CreateCorporationInfo;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.DeleteCorporationInfo;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveEmailAddress;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveLocationAddress;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemovePhoneNumber;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemoveWorkingHour;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.UpdateCorporationInfo;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.BackOffice;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.BackOffice.GetCorporationInfos;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.BackOffice.GetCorporationInfo;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.Export.ExportExcelCorporationInfos;

namespace Onyx.Web.BackOffice.Controllers.InfoCluster;


public class CorporationInfosController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CorporationInfoDto>>> GetAllCorporationInfos()
    {
        return await Mediator.Send(new GetAllCorporationInfosQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CorporationInfoDto?>> GetCorporationInfoById(int id)
    {
        return await Mediator.Send(new GetCorporationInfoByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCorporationInfosQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CorporationInfos.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCorporationInfoCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateCorporationInfoCommand command)
    {
        if (urlId != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("addEmailAddress{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> AddEmailAddress(int urlId, AddEmailAddressCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("removeEmailAddress{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> RemoveEmailAddress(int urlId, RemoveEmailAddressCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("addLocationAddress{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> AddLocationAddress(int urlId, AddLocationAddressCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("removeLocationAddress{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> RemoveLocationAddress(int urlId, RemoveLocationAddressCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("addPhoneNumber{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> AddPhoneNumber(int urlId, AddPhoneNumberCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("removePhoneNumber{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> RemovePhoneNumber(int urlId, RemovePhoneNumberCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("addWorkingHour{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> AddWorkingHour(int urlId, AddWorkingHourCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("removeWorkingHour{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> RemoveWorkingHour(int urlId, RemoveWorkingHourCommand command)
    {
        if (urlId != command.CorporationInfoId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCorporationInfoCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeCorporationInfo([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCorporationInfoCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
