using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.DeleteReturnOrderItem;

public record DeleteReturnOrderItemCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int ReturnOrderId { get; init; }
    public int ReturnOrderItemGroupId { get; init; }

};

public class DeleteReturnOrderItemCommandHandler : IRequestHandler<DeleteReturnOrderItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteReturnOrderItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteReturnOrderItemCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.ReturnOrderStateHistory)
            .Include(c => c.ItemGroups)
            .ThenInclude(c => c.ReturnOrderItems)
            .SingleOrDefaultAsync(e => e.Id == request.ReturnOrderId, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.ReturnOrderId);
        }
        if (returnOrder.GetCurrentReturnOrderState().ReturnOrderStatus != ReturnOrderStatus.Received)
        {
            throw new BadCommandException("به دلیل عدم همخوانی وضعیت سفارش بازگشت");
        }
        var returnOrderItemGroup = returnOrder.ItemGroups.SingleOrDefault(c => c.Id == request.ReturnOrderItemGroupId);

        if (returnOrderItemGroup == null)
        {
            throw new NotFoundException(nameof(ReturnOrderItemGroup), request.ReturnOrderItemGroupId);
        }

        var returnOrderItem = returnOrderItemGroup.ReturnOrderItems.SingleOrDefault(c => c.Id == request.Id);

        if (returnOrderItem == null)
        {
            throw new NotFoundException(nameof(ReturnOrderItem), request.Id);
        }

        returnOrder.RemoveReturnOrderItemWithoutValidation(returnOrderItem);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}