using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Prices.Commands.DeletePrice;


public class DeleteRangePricesCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; }
}
public class DeleteRangeProductPricesCommandHandler : IRequestHandler<DeleteRangePricesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeProductPricesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangePricesCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Prices
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Price), id);
            }

            _context.Prices.Remove(entity);

            //entity.AddDomainEvent(new KindsDe(entity));
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}