using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.DeleteProductAttributeType;


public record DeleteProductAttributeTypeCommand(int Id) : IRequest<Unit>;

public class DeleteProductAttributeTypeCommandHandler : IRequestHandler<DeleteProductAttributeTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAttributeTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductAttributeTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeTypes
            .Include(c => c.AttributeGroups)
            .ThenInclude(c => c.Attributes)
            .Include(c => c.Products)
            .ThenInclude(c => c.Attributes)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeType), request.Id);
        }

        entity.HandleRemoveProductAttributeType();

        _context.ProductAttributeTypes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}