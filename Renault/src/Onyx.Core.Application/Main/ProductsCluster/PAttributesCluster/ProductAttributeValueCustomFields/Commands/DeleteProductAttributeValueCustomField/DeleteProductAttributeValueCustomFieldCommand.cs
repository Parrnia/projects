using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeValueCustomFields.Commands.DeleteProductAttributeValueCustomField;

public record DeleteProductAttributeValueCustomFieldCommand(int Id) : IRequest<Unit>;

public class DeleteProductAttributeValueCustomFieldCommandHandler : IRequestHandler<DeleteProductAttributeValueCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAttributeValueCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductAttributeValueCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeValueCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeValueCustomField), request.Id);
        }

        _context.ProductAttributeValueCustomFields.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}