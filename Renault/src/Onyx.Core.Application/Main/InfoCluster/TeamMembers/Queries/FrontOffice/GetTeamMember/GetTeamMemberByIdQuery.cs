using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMember;

public record GetTeamMemberByIdQuery(int Id) : IRequest<TeamMemberByIdDto?>;

public class GetTeamMemberByIdQueryHandler : IRequestHandler<GetTeamMemberByIdQuery, TeamMemberByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTeamMemberByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeamMemberByIdDto?> Handle(GetTeamMemberByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.TeamMembers
            .ProjectTo<TeamMemberByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
