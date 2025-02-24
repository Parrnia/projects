using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Providers.Validators;

public record UniqueProviderLocalizedNameValidator : IRequest<bool>
{
    public int ProviderId { get; init; }
    public string LocalizedName { get; init; } = null!;
}

public class UniqueProviderLocalizedNameValidatorHandler : IRequestHandler<UniqueProviderLocalizedNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProviderLocalizedNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProviderLocalizedNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Providers.Where(c => c.Id != request.ProviderId)
            .AllAsync(e => e.LocalizedName != request.LocalizedName, cancellationToken);
        return isUnique;
    }
}
