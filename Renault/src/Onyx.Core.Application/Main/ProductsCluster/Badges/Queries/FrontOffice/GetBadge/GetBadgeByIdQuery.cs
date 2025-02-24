using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.FrontOffice.GetBadge;

public record GetBadgeByIdQuery(int Id) : IRequest<BadgeByIdDto?>;

public class GetBadgeByIdQueryHandler : IRequestHandler<GetBadgeByIdQuery, BadgeByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBadgeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BadgeByIdDto?> Handle(GetBadgeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Badges
            .ProjectTo<BadgeByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
