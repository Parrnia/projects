using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.DeleteVariant;

public record DeleteVariantCommand(int Id) : IRequest<Unit>;

public class DeleteVariantCommandHandler : IRequestHandler<DeleteVariantCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteVariantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteVariantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductDisplayVariants
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductDisplayVariant), request.Id);
        }

        _context.ProductDisplayVariants.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}