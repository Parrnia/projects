using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Commands.DeleteBadge;

public class DeleteRangeBadgeFromAttributeOptionCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
    public int ProductAttributeOptionId { get; set; }
}

public class DeleteRangeBadgeFromAttributeOptionCommandHandler : IRequestHandler<DeleteRangeBadgeFromAttributeOptionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeBadgeFromAttributeOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeBadgeFromAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var productAttributeOption =
            await _context.ProductAttributeOptions
                .Include(c => c.Badges)
                .SingleOrDefaultAsync(c => c.Id == request.ProductAttributeOptionId,
                cancellationToken);
        if (productAttributeOption == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOption), request.ProductAttributeOptionId);
        }

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = productAttributeOption.Badges.SingleOrDefault(c => c.Id == id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Badge), id);
            }

            productAttributeOption.Badges.Remove(entity);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
