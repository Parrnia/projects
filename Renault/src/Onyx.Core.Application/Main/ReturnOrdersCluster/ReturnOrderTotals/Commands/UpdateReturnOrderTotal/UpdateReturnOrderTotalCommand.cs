using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.UpdateReturnOrderTotal;
public record UpdateReturnOrderTotalCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
    public ReturnOrderTotalType Type { get; init; }
    public ReturnOrderTotalApplyType ReturnOrderTotalApplyType { get; init; }
    public int ReturnOrderId { get; init; }
}

public class UpdateReturnOrderTotalCommandHandler : IRequestHandler<UpdateReturnOrderTotalCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateReturnOrderTotalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateReturnOrderTotalCommand request, CancellationToken cancellationToken)
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

        returnOrder.UpdateTotalWithoutValidation(request.Id, request.Title, request.Price, request.Type, request.ReturnOrderTotalApplyType);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
