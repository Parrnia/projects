using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Validators;

public record UniqueBlogCategoryLocalizedNameValidator : IRequest<bool>
{
    public int BlogCategoryId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueBlogCategoryLocalizedNameValidatorHandler : IRequestHandler<UniqueBlogCategoryLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueBlogCategoryLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueBlogCategoryLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.BlogCategories.Where(c => c.Id != request.BlogCategoryId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
