using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Validators;

public record UniqueProductBrandLocalizedNameValidator : IRequest<bool>
{
    public int ProductBrandId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueProductBrandLocalizedNameValidatorHandler : IRequestHandler<UniqueProductBrandLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductBrandLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductBrandLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductBrands.Where(c => c.Id != request.ProductBrandId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
