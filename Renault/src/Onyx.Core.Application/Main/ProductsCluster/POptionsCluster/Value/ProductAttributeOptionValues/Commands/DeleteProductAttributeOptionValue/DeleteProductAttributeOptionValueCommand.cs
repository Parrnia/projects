using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Commands.DeleteProductAttributeOptionValue;

public record DeleteProductAttributeOptionValueCommand(int Id) : IRequest<Unit>;

public class DeleteProductAttributeOptionValueCommandHandler : IRequestHandler<DeleteProductAttributeOptionValueCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAttributeOptionValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductAttributeOptionValueCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeOptionValues
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOptionValue), request.Id);
        }

        _context.ProductAttributeOptionValues.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}