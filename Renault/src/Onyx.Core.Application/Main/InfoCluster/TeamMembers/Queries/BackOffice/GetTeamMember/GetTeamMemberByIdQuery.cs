using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice.GetTeamMember;

public record GetTeamMemberByIdQuery(int Id) : IRequest<TeamMemberDto?>;

public class GetTeamMemberByIdQueryHandler : IRequestHandler<GetTeamMemberByIdQuery, TeamMemberDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;


    public GetTeamMemberByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeamMemberDto?> Handle(GetTeamMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var list = _context.TeamMembers;
        var listDto = await list.ProjectTo<TeamMemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return listDto;

    }
}
