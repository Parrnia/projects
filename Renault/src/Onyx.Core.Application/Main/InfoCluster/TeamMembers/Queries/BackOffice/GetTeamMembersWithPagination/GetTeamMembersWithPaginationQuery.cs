using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice.GetTeamMembersWithPagination;
public record GetTeamMembersWithPaginationQuery : IRequest<PaginatedList<TeamMemberDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTeamMembersWithPaginationQueryHandler : IRequestHandler<GetTeamMembersWithPaginationQuery, PaginatedList<TeamMemberDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTeamMembersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TeamMemberDto>> Handle(GetTeamMembersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.TeamMembers
            .OrderBy(x => x.Name)
            .ProjectTo<TeamMemberDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
