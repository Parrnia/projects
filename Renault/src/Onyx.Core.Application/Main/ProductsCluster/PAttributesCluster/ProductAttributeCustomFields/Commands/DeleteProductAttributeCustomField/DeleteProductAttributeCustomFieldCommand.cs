using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeCustomFields.Commands.DeleteProductAttributeCustomField;

public record DeleteProductAttributeCustomFieldCommand(int Id) : IRequest<Unit>;

public class DeleteProductAttributeCustomFieldCommandHandler : IRequestHandler<DeleteProductAttributeCustomFieldCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAttributeCustomFieldCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductAttributeCustomFieldCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeCustomFields
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeCustomField), request.Id);
        }

        _context.ProductAttributeCustomFields.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}