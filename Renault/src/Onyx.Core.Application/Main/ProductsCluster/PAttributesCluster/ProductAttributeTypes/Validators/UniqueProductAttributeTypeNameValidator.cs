using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Validators;

public record UniqueProductAttributeTypeNameValidator : IRequest<bool>
{
    public int ProductAttributeTypeId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProductAttributeTypeNameValidatorHandler : IRequestHandler<UniqueProductAttributeTypeNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductAttributeTypeNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductAttributeTypeNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductAttributeTypes.Where(c => c.Id != request.ProductAttributeTypeId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    