using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.DeleteOrder;
public record DeleteOrderCommand(int Id) : IRequest<Unit>;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;

    public DeleteOrderCommandHandler(IApplicationDbContext context, ISevenSoftService sevenSoftService)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        if (entity.GetCurrentOrderState().OrderStatus != OrderStatus.PendingForRegister)
        {
            throw new OrderException("حذف این سفارش امکان پذیر نیست");
        }

        _context.Orders.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}