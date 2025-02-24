using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice.GetFooterLinkContainersWithPagination;
public record GetFooterLinkContainersWithPaginationQuery : IRequest<PaginatedList<FooterLinkContainerDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFooterLinkContainersWithPaginationQueryHandler : IRequestHandler<GetFooterLinkContainersWithPaginationQuery, PaginatedList<FooterLinkContainerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFooterLinkContainersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FooterLinkContainerDto>> Handle(GetFooterLinkContainersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.FooterLinkContainers
            .OrderBy(x => x.Order)
            .ProjectTo<FooterLinkContainerDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
