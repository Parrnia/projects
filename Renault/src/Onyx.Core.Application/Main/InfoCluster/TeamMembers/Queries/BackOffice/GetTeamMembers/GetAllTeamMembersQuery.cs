using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice.GetTeamMembers;
public record GetAllTeamMembersQuery : IRequest<List<TeamMemberDto>>;

public class GetAllTeamMembersQueryHandler : IRequestHandler<GetAllTeamMembersQuery, List<TeamMemberDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllTeamMembersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TeamMemberDto>> Handle(GetAllTeamMembersQuery request, CancellationToken cancellationToken)
    {
        return await _context.TeamMembers
            .OrderBy(x => x.Name)
            .ProjectTo<TeamMemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
