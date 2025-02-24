using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMembersWithPagination;
public record GetTeamMembersWithPaginationQuery : IRequest<PaginatedList<TeamMemberWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTeamMembersWithPaginationQueryHandler : IRequestHandler<GetTeamMembersWithPaginationQuery, PaginatedList<TeamMemberWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTeamMembersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TeamMemberWithPaginationDto>> Handle(GetTeamMembersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.TeamMembers
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<TeamMemberWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
