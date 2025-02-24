using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.DeleteProductOptionValueColor;

public class DeleteRangeProductOptionValueCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}

public class DeleteRangeProductOptionValueCommandHandler : IRequestHandler<DeleteRangeProductOptionValueCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductOptionValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductOptionValueCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductOptionValueColors
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductOptionValueColor), id);
            }

            _context.ProductOptionValueColors.Remove(entity);

            var products = await _context.Products
                .Where(c => c.ColorOptionId == entity.ProductOptionColorId)
                .Include(c => c.Attributes).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                var dbAttribute = product.Attributes.SingleOrDefault(c => c.Name == entity.Name);
                if (dbAttribute != null)
                {
                    _context.ProductAttributes.Remove(dbAttribute);
                }
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
