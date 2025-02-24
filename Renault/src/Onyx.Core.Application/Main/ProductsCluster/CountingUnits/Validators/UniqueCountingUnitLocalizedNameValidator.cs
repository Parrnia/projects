using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Validators;

public record UniqueCountingUnitLocalizedNameValidator : IRequest<bool>
{
    public int CountingUnitId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueCountingUnitLocalizedNameValidatorHandler : IRequestHandler<UniqueCountingUnitLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueCountingUnitLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCountingUnitLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.CountingUnits.Where(c => c.Id != request.CountingUnitId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
