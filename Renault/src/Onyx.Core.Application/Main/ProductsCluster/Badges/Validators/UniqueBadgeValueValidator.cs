using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Badges.Validators;

public record UniqueBadgeValueValidator : IRequest<bool>
{
    public int BadgeId { get; init; }
    public string Value { get; init; } = null!;
}

public class UniqueBadgeValueValidatorHandler : IRequestHandler<UniqueBadgeValueValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueBadgeValueValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueBadgeValueValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Badges.Where(c => c.Id != request.BadgeId)
            .AllAsync(e => e.Value != request.Value, cancellationToken);
        return isUnique;
    }
}
