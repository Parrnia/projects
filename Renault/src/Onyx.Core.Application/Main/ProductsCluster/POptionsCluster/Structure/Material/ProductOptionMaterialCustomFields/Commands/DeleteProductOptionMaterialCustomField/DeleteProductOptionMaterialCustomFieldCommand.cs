using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterialCustomFields.Commands.DeleteProductOptionMaterialCustomField;

public record DeleteProductOptionMaterialCustomFieldCommand(int Id) : IRequest<Unit>;

public class DeleteProductOptionMaterialCustomFieldCommandHandler : IRequestHandler<DeleteProductOptionMaterialCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductOptionMaterialCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductOptionMaterialCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductOptionMaterialCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductOptionMaterialCustomField), request.Id);
        }

        _context.ProductOptionMaterialCustomFields.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}