using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.UpdateReturnOrderItem;
public record UpdateReturnOrderItemCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public double Quantity { get; init; }
    public bool IsAccepted { get; set; }
    public int ReturnOrderItemGroupId { get; set; }
    public string Details { get; init; } = null!;
    public ReturnOrderCustomerReasonType? CustomerType { get; init; }
    public ReturnOrderOrganizationReasonType? OrganizationType { get; init; }
    public int ReturnOrderId { get; init; }
}

public class UpdateReturnOrderItemCommandHandler : IRequestHandler<UpdateReturnOrderItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateReturnOrderItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateReturnOrderItemCommand request, CancellationToken cancellationToken)
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

        if (request.CustomerType != null && request.OrganizationType != null)
        {
            throw new BadCommandException("با این اطلاعات");
        }

        returnOrder.UpdateReturnOrderItemWithoutValidation(
            request.Id,
            request.ReturnOrderItemGroupId,
            request.Quantity,
            request.Details,
            request.IsAccepted,
            request.CustomerType,
            request.OrganizationType
            );

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
