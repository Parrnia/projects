using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMember;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMembers;
using Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMembersWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.InfoCluster;


public class TeamMembersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<TeamMemberWithPaginationDto>>> GetTeamMembersWithPagination([FromQuery] GetTeamMembersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllTeamMemberDto>>> GetAllTeamMembers()
    {
        return await Mediator.Send(new GetAllTeamMembersQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamMemberByIdDto?>> GetTeamMemberById(int id)
    {
        return await Mediator.Send(new GetTeamMemberByIdQuery(id));
    }
}
