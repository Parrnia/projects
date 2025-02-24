using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application.Services.PaymentServices.Responses;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.StartPayment;
public class StartPaymentCommand : IRequest<StartPaymentResult>
{
    public int OrderId { get; set; }
    public int PaymentId { get; set; }
    public string? PhoneNumber { get; set; } = null!;
    public PaymentServiceType PaymentService { get; set; }
}

public class StartPaymentCommandHandler : IRequestHandler<StartPaymentCommand, StartPaymentResult>
{
    private readonly PaymentServiceFactory _paymentServiceFactory;
    private readonly IApplicationDbContext _context;


    public StartPaymentCommandHandler(IApplicationDbContext context,
        PaymentServiceFactory paymentServiceFactory)
    {
        _context = context;
        _paymentServiceFactory = paymentServiceFactory;
    }

    public async Task<StartPaymentResult> Handle(StartPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(c => c.PaymentMethods)
            .Include(c => c.OrderStateHistory)
            .FirstOrDefaultAsync(e => e.Id == request.OrderId, cancellationToken);

        if (order == null)
            throw new NotFoundException(nameof(Order), request.OrderId);

        var payment = order.PaymentMethods
            .OfType<OnlinePayment>()
            .FirstOrDefault(i => i.Id == request.PaymentId);
        if (payment is null)
            throw new Exception("نوع پرداخت نامعتبر است.");

        if (payment.Status != OnlinePaymentStatus.Waiting)
            throw new Exception("وضعیت پرداخت نامعتبر است.");

        var paymentService = _paymentServiceFactory.Create(request.PaymentService);
        var result = await paymentService.StartPayment((long)order.Total, payment.Id, request.PhoneNumber);

        if (result.IsSuccess)
        {
            payment.Authority = result.Token;
            payment.Status = OnlinePaymentStatus.Waiting;
        }
        else
        {
            payment.Error = result.ErrorMessage;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return result;
    }
}
