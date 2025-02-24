using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Commands.DeleteKind;

public class DeleteRangeKindsCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}



public class DeleteRangeKindsCommandHandler : IRequestHandler<DeleteRangeKindsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeKindsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeKindsCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Kinds
                .Include(c => c.Products)
                .ThenInclude(c => c.ProductDisplayVariants)
                .Include(c => c.Products)
                .ThenInclude(c => c.ProductBrand)
                .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Kind), id);
            }

            _context.Kinds.Remove(entity);

            var model = await _context.Models
                            .Include(c => c.Family)
                            .SingleOrDefaultAsync(c => c.Id == entity.ModelId, cancellationToken)
                        ?? throw new NotFoundException(nameof(Model), entity.ModelId);

            var productDisplayVariants = await _context.ProductDisplayVariants.ToListAsync(cancellationToken);

            foreach (var product in entity.Products)
            {
                var productDisplayVariant = productDisplayVariants.SingleOrDefault(c =>
                    c.ProductId == product.Id && c.Name == product.ProductBrand.LocalizedName + " " +
                    model.Family.LocalizedName + " " + product.LocalizedName);
                if (productDisplayVariant != null)
                {
                    product.ProductDisplayVariants.Remove(productDisplayVariant);
                }
            }
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}