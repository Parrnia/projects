using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Commands.CreateReturnOrderTotal;
public record CreateReturnOrderTotalCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
    public ReturnOrderTotalType Type { get; init; }
    public ReturnOrderTotalApplyType ReturnOrderTotalApplyType { get; init; }
    public int ReturnOrderId { get; init; }
}

public class CreateReturnOrderTotalCommandHandler : IRequestHandler<CreateReturnOrderTotalCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateReturnOrderTotalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateReturnOrderTotalCommand request, CancellationToken cancellationToken)
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
        var entity = new ReturnOrderTotal()
        {
            Title = request.Title,
            Price = request.Price,
            Type =  request.Type,
            ReturnOrderTotalApplyType = request.ReturnOrderTotalApplyType,
        };

        returnOrder.AddTotalWithoutValidation(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
