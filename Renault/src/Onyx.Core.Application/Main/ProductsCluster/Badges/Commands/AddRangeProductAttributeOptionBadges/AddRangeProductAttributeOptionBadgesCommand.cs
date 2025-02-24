using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Commands.AddRangeProductAttributeOptionBadges;


public record AddRangeProductAttributeOptionBadgesCommand : IRequest<List<int>>
{
    public List<int> BadgeIds { get; set; } = new List<int>();
    public int ProductAttributeOptionId { get; set; }
}

public class AddRangeProductAttributeOptionBadgesCommandHandler : IRequestHandler<AddRangeProductAttributeOptionBadgesCommand, List<int>>
{
    private readonly IApplicationDbContext _context;

    public AddRangeProductAttributeOptionBadgesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> Handle(AddRangeProductAttributeOptionBadgesCommand request, CancellationToken cancellationToken)
    {
        var productAttributeOption = await _context.ProductAttributeOptions
            .Include(c => c.Badges)
            .SingleOrDefaultAsync(c => c.Id == request.ProductAttributeOptionId, cancellationToken);

        if (productAttributeOption == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOption), request.ProductAttributeOptionId);
        }

        var badges = new List<Badge>();

        foreach (var badgeId in request.BadgeIds)
        {
            var badge = await _context.Badges
                .SingleOrDefaultAsync(c => c.Id == badgeId, cancellationToken) 
                        ?? throw new NotFoundException(nameof(Badge), badgeId); ;
            badges.Add(badge);
        }

        foreach (var badge in badges)
        {
            if (!productAttributeOption.Badges.Select(c => c.Id).Contains(badge.Id))
            {
                productAttributeOption.Badges.Add(badge);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return productAttributeOption.Badges.Select(c => c.Id).ToList();
    }
}
