using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Validators;

public record UniqueThemeTitleValidator : IRequest<bool>
{
    public int ThemeId { get; init; }
    public string ThemeTitle { get; init; } = null!;
}

public class UniqueThemeTitleValidatorHandler : IRequestHandler<UniqueThemeTitleValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueThemeTitleValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueThemeTitleValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Themes
            .Where(c => c.Id != request.ThemeId)
            .AllAsync(e => e.Title != request.ThemeTitle, cancellationToken);
        return isUnique;
    }
}
