using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.CreateReturnOrderItem;
public record CreateReturnOrderItemCommand : IRequest<int>
{
    public double Quantity { get; init; }
    public bool IsAccepted { get; init; }
    public int ReturnOrderItemGroupId { get; init; }
    public string Details { get; init; } = null!;
    public ReturnOrderCustomerReasonType? CustomerType { get; init; }
    public ReturnOrderOrganizationReasonType? OrganizationType { get; init; }
    public int ReturnOrderId { get; init; }
}

public class CreateReturnOrderItemCommandHandler : IRequestHandler<CreateReturnOrderItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateReturnOrderItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateReturnOrderItemCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.ReturnOrderStateHistory)
            .Include(c => c.ItemGroups)
            .ThenInclude(c => c.ReturnOrderItems)
            .Include(c => c.Order)
            .ThenInclude(c => c.Items)
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

        if (request.CustomerType != null && request.OrganizationType != null)
        {
            throw new BadCommandException("با این اطلاعات");
        }

        var entity = new ReturnOrderItem()
        {
            Quantity = request.Quantity,
            IsAccepted =  request.IsAccepted,
            ReturnOrderItemGroupId =  request.ReturnOrderItemGroupId,
            ReturnOrderReason = new ReturnOrderReason()
            {
                Details = request.Details
            }
        };
        entity.ReturnOrderReason.SetReturnOrderReasonType(request.CustomerType, request.OrganizationType);

        returnOrder.AddReturnOrderItemWithoutValidation(entity, request.ReturnOrderItemGroupId);
        

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
