using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Validators;

public record UniqueBlogCategoryNameValidator : IRequest<bool>
{
    public int BlogCategoryId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueBlogCategoryNameValidatorHandler : IRequestHandler<UniqueBlogCategoryNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueBlogCategoryNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueBlogCategoryNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.BlogCategories.Where(c => c.Id != request.BlogCategoryId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    