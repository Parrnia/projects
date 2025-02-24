using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinks;

public record GetFooterLinksByFooterLinkContainerIdQuery(int FooterLinkContainerId) : IRequest<List<FooterLinkDto>>;

public class GetFooterLinksByFooterLinkContainerIdQueryHandler : IRequestHandler<GetFooterLinksByFooterLinkContainerIdQuery, List<FooterLinkDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFooterLinksByFooterLinkContainerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FooterLinkDto>> Handle(GetFooterLinksByFooterLinkContainerIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.FooterLinks
            .Where(x => x.FooterLinkContainerId == request.FooterLinkContainerId)
            .OrderBy(x => x.Title)
            .ProjectTo<FooterLinkDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
