using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinksWithPagination;
public record GetFooterLinksWithPaginationQuery : IRequest<PaginatedList<FooterLinkDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFooterLinksWithPaginationQueryHandler : IRequestHandler<GetFooterLinksWithPaginationQuery, PaginatedList<FooterLinkDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFooterLinksWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FooterLinkDto>> Handle(GetFooterLinksWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.FooterLinks
            .OrderBy(x => x.Title)
            .ProjectTo<FooterLinkDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
