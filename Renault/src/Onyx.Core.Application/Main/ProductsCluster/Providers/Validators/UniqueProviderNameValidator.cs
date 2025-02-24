using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Providers.Validators;

public record UniqueProviderNameValidator : IRequest<bool>
{
    public int ProviderId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProviderNameValidatorHandler : IRequestHandler<UniqueProviderNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProviderNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProviderNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Providers.Where(c => c.Id != request.ProviderId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    