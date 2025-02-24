using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.DeleteReturnOrderTotal;

public record DeleteRangeReturnOrderTotalCommand : IRequest<Unit>
{
    public List<int> Ids { get; init; } = new List<int>();
    public int ReturnOrderId { get; init; }
};

public class DeleteRangeReturnOrderTotalCommandHandler : IRequestHandler<DeleteRangeReturnOrderTotalCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeReturnOrderTotalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeReturnOrderTotalCommand request, CancellationToken cancellationToken)
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
        foreach (var id in request.Ids)
        {
            var returnOrderTotal = returnOrder.Totals.SingleOrDefault(c => c.Id == id);

            if (returnOrderTotal == null)
            {
                throw new NotFoundException(nameof(ReturnOrderTotal), id);
            }

            returnOrder.RemoveTotalWithoutValidation(returnOrderTotal);
        }
        

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}