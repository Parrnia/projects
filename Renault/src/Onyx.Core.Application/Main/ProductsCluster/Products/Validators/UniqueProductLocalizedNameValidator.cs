using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Products.Validators;

public record UniqueProductLocalizedNameValidator : IRequest<bool>
{
    public int ProductId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueProductLocalizedNameValidatorHandler : IRequestHandler<UniqueProductLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Products.Where(c => c.Id != request.ProductId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
