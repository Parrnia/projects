using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Commands.DeleteProductOptionValueMaterial;
public class DeleteRangeProductOptionValueMaterialCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}

public class DeleteRangeProductOptionValueMaterialCommandHandler : IRequestHandler<DeleteRangeProductOptionValueMaterialCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductOptionValueMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductOptionValueMaterialCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductOptionValueMaterials
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductOptionValueMaterial), id);
            }

            _context.ProductOptionValueMaterials.Remove(entity);

            //entity.AddDomainEvent(new KindsDe(entity));
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
