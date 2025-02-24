using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.FrontOffice.GetBadges;
public record GetAllBadgesQuery : IRequest<List<AllBadgeDto>>;

public class GetAllBadgesQueryHandler : IRequestHandler<GetAllBadgesQuery, List<AllBadgeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBadgesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllBadgeDto>> Handle(GetAllBadgesQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Badges
            .Where(c => c.IsActive)
            .OrderBy(x => x.Value)
            .ProjectTo<AllBadgeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
