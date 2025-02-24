using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.DeleteReturnOrderTotal;

public record DeleteReturnOrderTotalCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int ReturnOrderId { get; init; }
};

public class DeleteReturnOrderTotalCommandHandler : IRequestHandler<DeleteReturnOrderTotalCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteReturnOrderTotalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteReturnOrderTotalCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.ReturnOrderStateHistory)
            .Include(c => c.Totals)
            .SingleOrDefaultAsync(e => e.Id == request.ReturnOrderId, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.ReturnOrderId);
        }
        if (returnOrder.GetCurrentReturnOrderState().ReturnOrderStatus != ReturnOrderStatus.Received)
        {
            throw new BadCommandException("به دلیل عدم همخوانی وضعیت سفارش بازگشت");
        }
        var returnOrderTotal = returnOrder.Totals.SingleOrDefault(c => c.Id == request.Id);

        if (returnOrderTotal == null)
        {
            throw new NotFoundException(nameof(ReturnOrderTotal), request.Id);
        }

        returnOrder.RemoveTotalWithoutValidation(returnOrderTotal);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}