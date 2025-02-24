using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Validators;

public record UniqueProductStatusLocalizedNameValidator : IRequest<bool>
{
    public int ProductStatusId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueProductStatusLocalizedNameValidatorHandler : IRequestHandler<UniqueProductStatusLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductStatusLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductStatusLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductStatuses.Where(c => c.Id != request.ProductStatusId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
