using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinks;
public record GetAllFooterLinksQuery : IRequest<List<FooterLinkDto>>;

public class GetAllFooterLinksQueryHandler : IRequestHandler<GetAllFooterLinksQuery, List<FooterLinkDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllFooterLinksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FooterLinkDto>> Handle(GetAllFooterLinksQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.FooterLinks
            .OrderBy(x => x.Title)
            .ProjectTo<FooterLinkDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
