using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.DeleteProductOptionMaterial;



public record DeleteProductOptionMaterialCommand(int Id) : IRequest<Unit>;

public class DeleteProductOptionMaterialCommandHandler : IRequestHandler<DeleteProductOptionMaterialCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductOptionMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductOptionMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionMaterials
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionMaterial), request.Id);
        }

        _context.ProductOptionMaterials.Remove(entity);

        var attributeGroup = await _context.ProductTypeAttributeGroups
            .SingleOrDefaultAsync(c => c.Name == entity.Name, cancellationToken);

        if (attributeGroup == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
        }

        _context.ProductTypeAttributeGroups.Remove(attributeGroup);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}