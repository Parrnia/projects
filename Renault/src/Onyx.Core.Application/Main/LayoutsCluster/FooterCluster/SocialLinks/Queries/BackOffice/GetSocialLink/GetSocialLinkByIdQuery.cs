using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.BackOffice.GetSocialLink;

public record GetSocialLinkByIdQuery(int Id) : IRequest<SocialLinkDto?>;

public class GetSocialLinkByIdQueryHandler : IRequestHandler<GetSocialLinkByIdQuery, SocialLinkDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSocialLinkByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SocialLinkDto?> Handle(GetSocialLinkByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.SocialLinks
            .ProjectTo<SocialLinkDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
