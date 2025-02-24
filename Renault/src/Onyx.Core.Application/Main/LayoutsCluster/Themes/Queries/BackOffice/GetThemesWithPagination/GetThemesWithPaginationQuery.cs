using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetThemesWithPagination;
public record GetThemesWithPaginationQuery : IRequest<PaginatedList<ThemeDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetThemesWithPaginationQueryHandler : IRequestHandler<GetThemesWithPaginationQuery, PaginatedList<ThemeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetThemesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ThemeDto>> Handle(GetThemesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Themes
            .OrderBy(c => c.Title)
            .ProjectTo<ThemeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return result;
    }
}
