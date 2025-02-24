using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.DeleteVariant;

public class DeleteRangeVariantCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeVariantCommandHandler : IRequestHandler<DeleteRangeVariantCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeVariantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeVariantCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductDisplayVariants
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductDisplayVariant), id);
            }

            _context.ProductDisplayVariants.Remove(entity);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
