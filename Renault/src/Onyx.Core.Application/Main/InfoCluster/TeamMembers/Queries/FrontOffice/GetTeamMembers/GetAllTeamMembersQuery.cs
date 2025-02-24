using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMembers;
public record GetAllTeamMembersQuery : IRequest<List<AllTeamMemberDto>>;

public class GetAllTeamMembersQueryHandler : IRequestHandler<GetAllTeamMembersQuery, List<AllTeamMemberDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllTeamMembersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllTeamMemberDto>> Handle(GetAllTeamMembersQuery request, CancellationToken cancellationToken)
    {
        return await _context.TeamMembers
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllTeamMemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
