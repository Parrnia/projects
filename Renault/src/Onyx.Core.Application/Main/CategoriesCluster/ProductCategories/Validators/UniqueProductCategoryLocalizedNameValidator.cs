using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Validators;

public record UniqueProductCategoryLocalizedNameValidator : IRequest<bool>
{
    public int ProductCategoryId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueProductCategoryLocalizedNameValidatorHandler : IRequestHandler<UniqueProductCategoryLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductCategoryLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductCategoryLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductCategories.Where(c => c.Id != request.ProductCategoryId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
