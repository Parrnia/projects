using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice;
public class TeamMemberDto : IMapFrom<TeamMember>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public Guid Avatar { get; set; }
    public string AboutUsTitle { get; set; } = null!;
    public bool IsActive { get; set; }
    public int AboutUsId { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<TeamMember, TeamMemberDto>()
            .ForMember(d => d.AboutUsTitle, opt => opt.MapFrom(s => s.AboutUs.Title));
    }
}
