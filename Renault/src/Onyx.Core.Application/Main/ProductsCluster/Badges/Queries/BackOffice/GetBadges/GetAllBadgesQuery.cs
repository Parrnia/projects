using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice.GetBadges;
public record GetAllBadgesQuery : IRequest<List<BadgeDto>>;

public class GetAllBadgesQueryHandler : IRequestHandler<GetAllBadgesQuery, List<BadgeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBadgesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BadgeDto>> Handle(GetAllBadgesQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Badges.AsNoTracking()
            .OrderBy(x => x.Value)
            .ProjectTo<BadgeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
