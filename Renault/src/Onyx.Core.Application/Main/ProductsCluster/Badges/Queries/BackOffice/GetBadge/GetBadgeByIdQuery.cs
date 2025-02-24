using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice.GetBadge;

public record GetBadgeByIdQuery(int Id) : IRequest<BadgeDto?>;

public class GetBadgeByIdQueryHandler : IRequestHandler<GetBadgeByIdQuery, BadgeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBadgeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BadgeDto?> Handle(GetBadgeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Badges
            .ProjectTo<BadgeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
