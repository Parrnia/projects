using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Validators;

public record UniqueCountingUnitTypeNameValidator : IRequest<bool>
{
    public int CountingUnitTypeId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueCountingUnitTypeNameValidatorHandler : IRequestHandler<UniqueCountingUnitTypeNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueCountingUnitTypeNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueCountingUnitTypeNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.CountingUnitTypes.Where(c => c.Id != request.CountingUnitTypeId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    