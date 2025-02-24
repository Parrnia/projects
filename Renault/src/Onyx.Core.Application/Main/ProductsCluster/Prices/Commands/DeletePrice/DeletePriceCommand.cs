using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Prices.Commands.DeletePrice;

public record DeletePriceCommand(int Id) : IRequest<Unit>;

public class DeletePriceCommandHandler : IRequestHandler<DeletePriceCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeletePriceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePriceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Prices
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Price), request.Id);
        }

        _context.Prices.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}