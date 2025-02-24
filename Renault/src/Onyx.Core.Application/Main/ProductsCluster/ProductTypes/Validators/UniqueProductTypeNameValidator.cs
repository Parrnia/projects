using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Validators;

public record UniqueProductTypeNameValidator : IRequest<bool>
{
    public int ProductTypeId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProductTypeNameValidatorHandler : IRequestHandler<UniqueProductTypeNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductTypeNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductTypeNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductTypes.Where(c => c.Id != request.ProductTypeId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    