using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.TeamMembers.Commands.CreateTeamMember;
using Onyx.Application.Main.InfoCluster.TeamMembers.Commands.DeleteTeamMember;
using Onyx.Application.Main.InfoCluster.TeamMembers.Commands.UpdateTeamMember;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice.GetTeamMember;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice.GetTeamMembers;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice.GetTeamMembersWithPagination;
using Onyx.Application.Main.InfoCluster.TeamMembers.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.Export.ExportExcelTeamMembers;

namespace Onyx.Web.BackOffice.Controllers.InfoCluster;


public class TeamMembersController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<TeamMemberDto>>> GetTeamMembersWithPagination([FromQuery] GetTeamMembersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<TeamMemberDto>>> GetAllTeamMembers()
    {
        return await Mediator.Send(new GetAllTeamMembersQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<TeamMemberDto?>> GetTeamMemberById(int id)
    {
        return await Mediator.Send(new GetTeamMemberByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelTeamMembersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "TeamMembers.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateTeamMemberCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateTeamMemberCommand command)
    {
        if (urlId != command.Id)
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
        await Mediator.Send(new DeleteTeamMemberCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeTeamMember([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeTeamMemberCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueTeamMemberName([FromQuery] UniqueTeamMemberNameValidator query)
    {
        return await Mediator.Send(query);
    }
    
}
