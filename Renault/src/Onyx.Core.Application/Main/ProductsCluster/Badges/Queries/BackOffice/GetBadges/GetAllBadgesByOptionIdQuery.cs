using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice.GetBadges;

public record GetAllBadgesByOptionIdQuery(int OptionId) : IRequest<List<BadgeDto>>;

public class GetAllBadgesByProductIdQueryHandler : IRequestHandler<GetAllBadgesByOptionIdQuery, List<BadgeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBadgesByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BadgeDto>> Handle(GetAllBadgesByOptionIdQuery request, CancellationToken cancellationToken)
    {
        var attributeOption = await _context.ProductAttributeOptions
            .Include(c => c.Badges)
            .SingleOrDefaultAsync(c => c.Id == request.OptionId, cancellationToken);

        if (attributeOption == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOption), request.OptionId);
        }

        var badges = attributeOption.Badges.Select(c => new BadgeDto()
        {
            Id = c.Id,
            Value = c.Value
        }).ToList();

        return badges;
    }
}