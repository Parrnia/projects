using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.HeaderCluster.Links.Queries.FrontOffice.GetLinks.GetAllLinks;
public record GetAllLinksQuery : IRequest<List<AllLinkDto>>;

public class GetAllLinksQueryHandler : IRequestHandler<GetAllLinksQuery, List<AllLinkDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllLinksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllLinkDto>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
    {
        var links = await _context.Links
            .Include(c => c.ChildLinks)
            .ThenInclude(c => c.ChildLinks)
            .Include(c => c.ParentLink)
            .ThenInclude(c => c.ParentLink)
            .Where(c => c.IsActive)
            .OrderBy(x => x.Title)
            .ProjectTo<AllLinkDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return links;
    }
}
