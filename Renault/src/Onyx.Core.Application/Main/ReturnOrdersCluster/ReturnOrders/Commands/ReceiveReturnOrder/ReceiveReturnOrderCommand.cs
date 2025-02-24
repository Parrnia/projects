using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReceiveReturnOrder;
public record ReceiveReturnOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
}

public class ReceiveReturnOrderCommandHandler : IRequestHandler<ReceiveReturnOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public ReceiveReturnOrderCommandHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(ReceiveReturnOrderCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.Order)
            .Include(c => c.ReturnOrderStateHistory)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.Id);
        }

        returnOrder.Receive(request.Details);
        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(returnOrder.Order.PhoneNumber, ".دریافت شد " + returnOrder.Number + "محموله سفارش بازگشت شما به شماره ");

        return Unit.Value;
    }
}
