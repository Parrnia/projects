using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.SelfDeleteOrder;
public record SelfDeleteOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public Guid CustomerId { get; set; }
}

public class SelfDeleteOrderCommandHandler : IRequestHandler<SelfDeleteOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;

    public SelfDeleteOrderCommandHandler(IApplicationDbContext context, ISevenSoftService sevenSoftService)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
    }

    public async Task<Unit> Handle(SelfDeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(c => c.OrderStateHistory)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (order == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        if (order.CustomerId != request.CustomerId)
        {
            throw new OrderException("شما دسترسی لازم را ندارید");
        }

        if (order.GetCurrentOrderState().OrderStatus != OrderStatus.PendingForRegister)
        {
            throw new OrderException("حذف این سفارش امکان پذیر نیست");
        }

        _context.Orders.Remove(order);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}