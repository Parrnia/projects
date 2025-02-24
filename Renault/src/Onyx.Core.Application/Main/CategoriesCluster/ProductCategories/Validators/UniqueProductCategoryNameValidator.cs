using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Validators;

public record UniqueProductCategoryNameValidator : IRequest<bool>
{
    public int ProductId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProductCategoryNameValidatorHandler : IRequestHandler<UniqueProductCategoryNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductCategoryNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductCategoryNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Products.Where(c => c.Id != request.ProductId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    