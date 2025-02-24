using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.HeaderCluster.Links.Queries.FrontOffice.GetLinks.GetFirstLayerLinks;
public record GetFirstLayerLinksQuery : IRequest<List<FirstLayerLinkDto>>;

public class GetFirstLayerLinksQueryHandler : IRequestHandler<GetFirstLayerLinksQuery, List<FirstLayerLinkDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFirstLayerLinksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FirstLayerLinkDto>> Handle(GetFirstLayerLinksQuery request, CancellationToken cancellationToken)
    {
        var links = await _context.Links
            .Include(c => c.ChildLinks)
            .ThenInclude(c => c.ChildLinks)
            .ThenInclude(c => c.ChildLinks)
            .Where(c => c.ParentLinkId == null && c.IsActive)
            .OrderBy(x => x.Title)
            .ProjectTo<FirstLayerLinkDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return links;
    }
}
