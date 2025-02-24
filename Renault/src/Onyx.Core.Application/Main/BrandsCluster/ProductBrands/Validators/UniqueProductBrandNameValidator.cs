using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Validators;

public record UniqueProductBrandNameValidator : IRequest<bool>
{
    public int ProductBrandId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProductBrandNameValidatorHandler : IRequestHandler<UniqueProductBrandNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductBrandNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductBrandNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductBrands.Where(c => c.Id != request.ProductBrandId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    