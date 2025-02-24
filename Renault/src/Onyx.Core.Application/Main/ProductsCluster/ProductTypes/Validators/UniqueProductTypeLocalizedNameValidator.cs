using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Validators;

public record UniqueProductTypeLocalizedNameValidator : IRequest<bool>
{
    public int ProductTypeId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueProductTypeLocalizedNameValidatorHandler : IRequestHandler<UniqueProductTypeLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductTypeLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductTypeLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductTypes.Where(c => c.Id != request.ProductTypeId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
