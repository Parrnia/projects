using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Validators;

public record UniqueCountingUnitNameValidator : IRequest<bool>
{
    public int CountingUnitId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueCountingUnitNameValidatorHandler : IRequestHandler<UniqueCountingUnitNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueCountingUnitNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCountingUnitNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.CountingUnits.Where(c => c.Id != request.CountingUnitId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    