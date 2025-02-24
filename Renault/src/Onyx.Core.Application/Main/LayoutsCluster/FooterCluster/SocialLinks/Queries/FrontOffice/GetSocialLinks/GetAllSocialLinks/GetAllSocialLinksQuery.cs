using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.FrontOffice.GetSocialLinks.GetAllSocialLinks;
public record GetAllSocialLinksQuery : IRequest<List<AllSocialLinkDto>>;

public class GetAllSocialLinksQueryHandler : IRequestHandler<GetAllSocialLinksQuery, List<AllSocialLinkDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllSocialLinksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllSocialLinkDto>> Handle(GetAllSocialLinksQuery request, CancellationToken cancellationToken)
    {
        var socialLinks = await _context.SocialLinks
            .Where(c => c.IsActive)
            .OrderBy(x => x.Url)
            .ProjectTo<AllSocialLinkDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return socialLinks;
    }
}
