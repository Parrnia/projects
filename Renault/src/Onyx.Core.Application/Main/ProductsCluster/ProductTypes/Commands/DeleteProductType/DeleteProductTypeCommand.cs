using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.DeleteProductType;

public record DeleteProductTypeCommand(int Id) : IRequest<Unit>;

public class DeleteProductTypeCommandHandler : IRequestHandler<DeleteProductTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductType), request.Id);
        }

        _context.ProductTypes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}