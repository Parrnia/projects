using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Products.Validators;

public record UniqueProductNameValidator : IRequest<bool>
{
    public int ProductId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProductNameValidatorHandler : IRequestHandler<UniqueProductNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Products.Where(c => c.Id != request.ProductId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    