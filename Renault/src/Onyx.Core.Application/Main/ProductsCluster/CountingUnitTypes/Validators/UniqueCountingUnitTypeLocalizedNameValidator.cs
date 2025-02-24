using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Validators;

public record UniqueCountingUnitTypeLocalizedNameValidator : IRequest<bool>
{
    public int CountingUnitTypeId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueCountingUnitTypeLocalizedNameValidatorHandler : IRequestHandler<UniqueCountingUnitTypeLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueCountingUnitTypeLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCountingUnitTypeLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.CountingUnitTypes.Where(c => c.Id != request.CountingUnitTypeId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
