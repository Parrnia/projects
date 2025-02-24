using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.AcceptReturnOrder;
public record AcceptReturnOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
}

public class AcceptReturnOrderCommandHandler : IRequestHandler<AcceptReturnOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public AcceptReturnOrderCommandHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(AcceptReturnOrderCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.Order)
            .Include(c => c.ReturnOrderStateHistory)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.Id);
        }

        returnOrder.Accept(request.Details);
        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(returnOrder.Order.PhoneNumber, ".تایید شد " + returnOrder.Number + "سفارش بازگشت شما به شماره ");

        return Unit.Value;
    }
}
