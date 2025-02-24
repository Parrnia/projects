using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.ConfirmOrder;
public record ConfirmOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
}

public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public ConfirmOrderCommandHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .Include(c => c.PaymentMethods)
            .Include(c => c.OrderStateHistory)
            .Include(e => e.Items).ThenInclude(c => c.ProductAttributeOption)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }
        entity.ConfirmOrder(request.Details);

        
        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(entity.PhoneNumber, ".تایید شده و در حال آماده سازی است " + entity.Token + " با شماره پیگیری " + entity.Number + "سفارش شما به شماره ");

        return Unit.Value;
    }
}
