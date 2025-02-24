using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.DeleteProductAttribute;

public record DeleteProductAttributeCommand(int Id) : IRequest<Unit>;

public class DeleteProductAttributeCommandHandler : IRequestHandler<DeleteProductAttributeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAttributeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttribute), request.Id);
        }

        _context.ProductAttributes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}