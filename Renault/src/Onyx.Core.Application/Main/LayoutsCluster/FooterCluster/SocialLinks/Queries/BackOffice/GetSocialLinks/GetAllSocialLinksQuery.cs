using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.BackOffice.GetSocialLinks;
public record GetAllSocialLinksQuery : IRequest<List<SocialLinkDto>>;

public class GetAllSocialLinksQueryHandler : IRequestHandler<GetAllSocialLinksQuery, List<SocialLinkDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllSocialLinksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SocialLinkDto>> Handle(GetAllSocialLinksQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.SocialLinks
            .OrderBy(x => x.Url)
            .ProjectTo<SocialLinkDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
