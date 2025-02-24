using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Commands.DeleteProductStatus;

public record DeleteProductStatusCommand(int Id) : IRequest<Unit>;

public class DeleteProductStatusCommandHandler : IRequestHandler<DeleteProductStatusCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductStatuses
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductStatus), request.Id);
        }

        _context.ProductStatuses.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}