using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.DeleteProductTypeAttributeGroupAttribute;

public record DeleteProductTypeAttributeGroupAttributeCommand(int Id) : IRequest<Unit>;

public class DeleteProductTypeAttributeGroupAttributeCommandHandler : IRequestHandler<DeleteProductTypeAttributeGroupAttributeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductTypeAttributeGroupAttributeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductTypeAttributeGroupAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypeAttributeGroupAttributes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroupAttribute), request.Id);
        }

        var productTypeAttributeGroup = await _context.ProductTypeAttributeGroups
            .Include(c => c.ProductAttributeTypes)
            .ThenInclude(c => c.Products)
            .ThenInclude(c => c.Attributes)
            .FirstOrDefaultAsync(x => x.Id == entity.ProductTypeAttributeGroupId, cancellationToken);

        if (productTypeAttributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.ProductTypeAttributeGroupId);
        }

        var attributes = new List<ProductTypeAttributeGroupAttribute> {entity};
        productTypeAttributeGroup.RemoveAttributes(attributes);

        _context.ProductTypeAttributeGroupAttributes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}