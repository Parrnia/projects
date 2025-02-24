using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.CreateProductTypeAttributeGroupAttribute;
public record CreateProductTypeAttributeGroupAttributeCommand : IRequest<int>
{ 
    public string Value { get; init; } = null!;
    public int ProductTypeAttributeGroupId { get; init; }
}

public class CreateProductTypeAttributeGroupAttributeCommandHandler : IRequestHandler<CreateProductTypeAttributeGroupAttributeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductTypeAttributeGroupAttributeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductTypeAttributeGroupAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductTypeAttributeGroupAttribute(request.Value);

        var productTypeAttributeGroup = await _context.ProductTypeAttributeGroups
            .Include(c => c.ProductAttributeTypes)
            .ThenInclude(c => c.Products)
            .ThenInclude(c => c.Attributes)
            .FirstOrDefaultAsync(x => x.Id == request.ProductTypeAttributeGroupId, cancellationToken);

        if (productTypeAttributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), request.ProductTypeAttributeGroupId);
        }

        var attributes = new List<ProductTypeAttributeGroupAttribute> {entity};
        productTypeAttributeGroup.AddAttributes(attributes);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
