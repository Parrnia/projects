using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductKinds.Commands;


public class DeleteRangeProductKindsCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}
public class DeleteRangeProductKindsCommandHandler : IRequestHandler<DeleteRangeProductKindsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductKindsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeProductKindsCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.ProductKinds
                .Include(c => c.Product)
                .ThenInclude(c => c.ProductDisplayVariants)
                .Include(c => c.Product)
                .ThenInclude(c => c.ProductBrand)
                .Include(c => c.Kind)
                .ThenInclude(c => c.Model)
                .ThenInclude(c => c.Family)
                .SingleOrDefaultAsync(c => c.Id == id , cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductKind), id);
            }

            _context.ProductKinds.Remove(entity);
            var productDisplayVariant = await _context.ProductDisplayVariants
                .SingleOrDefaultAsync(c => c.Name == entity.Product.ProductBrand.LocalizedName + " " +
                                           entity.Kind.Model.Family.LocalizedName + " " +
                                           entity.Product.LocalizedName &&
                                           c.ProductId == entity.ProductId, cancellationToken);
            if (productDisplayVariant != null)
            {
                entity.Product.ProductDisplayVariants.Remove(productDisplayVariant);
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}