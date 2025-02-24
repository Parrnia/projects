using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Validators;

public record UniqueProductTypeAttributeGroupNameValidator : IRequest<bool>
{
    public int ProductTypeAttributeGroupId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueProductTypeAttributeGroupNameValidatorHandler : IRequestHandler<UniqueProductTypeAttributeGroupNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueProductTypeAttributeGroupNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueProductTypeAttributeGroupNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.ProductTypeAttributeGroups.Where(c => c.Id != request.ProductTypeAttributeGroupId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
    