using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.DeleteProductOptionMaterial;

public class DeleteRangeProductOptionMaterialCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeProductOptionMaterialCommandHandler : IRequestHandler<DeleteRangeProductOptionMaterialCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductOptionMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductOptionMaterialCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductOptionMaterials
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductOptionMaterial), id);
            }

            _context.ProductOptionMaterials.Remove(entity);


            var attributeGroup = await _context.ProductTypeAttributeGroups
                .SingleOrDefaultAsync(c => c.Name == entity.Name, cancellationToken);

            if (attributeGroup == null)
            {
                throw new NotFoundException(nameof(ProductTypeAttributeGroup), entity.Name);
            }

            _context.ProductTypeAttributeGroups.Remove(attributeGroup);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
