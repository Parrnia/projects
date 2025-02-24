using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMembersWithPagination;
public class TeamMemberWithPaginationDto : IMapFrom<TeamMember>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public Guid Avatar { get; set; }
}
