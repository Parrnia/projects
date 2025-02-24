using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.ConfirmOrderPayment;
public record ConfirmOrderPaymentCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
}

public class ConfirmOrderPaymentCommandHandler : IRequestHandler<ConfirmOrderPaymentCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public ConfirmOrderPaymentCommandHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(ConfirmOrderPaymentCommand request, CancellationToken cancellationToken)
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
        entity.ConfirmOrderPayment(request.Details);


        await _context.SaveChangesAsync(cancellationToken);

        if (entity.PaymentMethods.Any(c => c.PaymentType == PaymentType.Online))
        {
            await _smsService.SendSms(entity.PhoneNumber, ".تایید شده و در انتظار تایید انبار است " + entity.Token + " با شماره پیگیری " + entity.Number + "پرداخت سفارش شما، به شماره ");
        }

        return Unit.Value;
    }
}
