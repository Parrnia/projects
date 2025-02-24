using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Validators;

public record UniqueProductStatusNameValidator : IRequest<bool>
{
    public int ProductStatusId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProductStatusNameValidatorHandler : IRequestHandler<UniqueProductStatusNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductStatusNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductStatusNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductStatuses.Where(c => c.Id != request.ProductStatusId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    