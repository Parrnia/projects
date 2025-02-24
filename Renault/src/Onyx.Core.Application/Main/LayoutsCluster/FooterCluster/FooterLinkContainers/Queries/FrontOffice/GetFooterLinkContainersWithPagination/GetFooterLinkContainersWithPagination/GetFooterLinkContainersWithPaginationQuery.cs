using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.FrontOffice.GetFooterLinkContainersWithPagination.GetFooterLinkContainersWithPagination;
public record GetFooterLinkContainersWithPaginationQuery : IRequest<PaginatedList<FooterLinkContainerWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFooterLinkContainersWithPaginationQueryHandler : IRequestHandler<GetFooterLinkContainersWithPaginationQuery, PaginatedList<FooterLinkContainerWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFooterLinkContainersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FooterLinkContainerWithPaginationDto>> Handle(GetFooterLinkContainersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var res =  await _context.FooterLinkContainers
            .Where(c => c.IsActive)
            .OrderBy(x => x.Order)
            .ProjectTo<FooterLinkContainerWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        foreach (var footerLinkContainer in res.Items)
        {
            var list = footerLinkContainer.Links.Where(c => c.IsActive).ToList();
            footerLinkContainer.Links = list;
        }

        return res;
    }
}
