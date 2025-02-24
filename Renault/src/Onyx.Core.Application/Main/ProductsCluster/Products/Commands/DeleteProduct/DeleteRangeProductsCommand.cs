using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Commands.DeleteProduct;

public class DeleteRangeProductsCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}

public class DeleteRangeProductsCommandHandler : IRequestHandler<DeleteRangeProductsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductsCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Products
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), id);
            }

            _context.Products.Remove(entity);

        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
