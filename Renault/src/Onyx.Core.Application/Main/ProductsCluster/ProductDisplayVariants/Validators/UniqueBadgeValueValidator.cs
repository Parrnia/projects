using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Validators;

public record UniqueVariantNameValidator : IRequest<bool>
{
    public int VariantId { get; init; }
    public int ProductId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueVariantNameValidatorHandler : IRequestHandler<UniqueVariantNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueVariantNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueVariantNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductDisplayVariants
            .Where(c => c.Id != request.VariantId && c.ProductId == request.ProductId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
