using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Commands.DeleteOrderTotal;
public record DeleteOrderTotalCommand(int Id) : IRequest<Unit>;

public class DeleteOrderTotalCommandHandler : IRequestHandler<DeleteOrderTotalCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteOrderTotalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteOrderTotalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OrderTotals
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OrderTotal), request.Id);
        }

        _context.OrderTotals.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}