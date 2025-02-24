using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;
using Order = Onyx.Domain.Entities.OrdersCluster.Order;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.RefundOrderCost;
public record RefundOrderCostCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
}

public class RefundOrderCostCommandHandler : IRequestHandler<RefundOrderCostCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public RefundOrderCostCommandHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(RefundOrderCostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .Include(c => c.PaymentMethods)
            .Include(c => c.OrderStateHistory)
            .Include(e => e.Items).ThenInclude(c => c.ProductAttributeOption)
            .Include(c => c.Customer)
            .ThenInclude(c => c.Credits)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        var customer = await _context.Customers
                           .Include(c => c.Credits)
                           .SingleOrDefaultAsync(
                       e => e.Id == entity.CustomerId, cancellationToken) ??
                   throw new NotFoundException(nameof(Customer), entity.CustomerId);


        if (entity.OrderPaymentType == OrderPaymentType.Credit)
        {
            var successFul = customer.AddCredit(entity.Total, entity.CustomerId, entity.CustomerFirstName + " " + entity.CustomerLastName, entity.Number);
            if (!successFul)
            {
                throw new BadCommandException("مقدار اعتبار نمی تواند کمتر از صفر باشد");
            }
        }
        else if (entity.OrderPaymentType == OrderPaymentType.CreditOnline)
        {
            var payment = entity.PaymentMethods
                .OfType<OnlinePayment>()
                .FirstOrDefault(e => e.Status == OnlinePaymentStatus.Completed);
            if (payment == null)
            {
                throw new BadCommandException("پرداخت آنلاین مشتری یافت نشد");
            }
            var paymentAmount = (decimal)(payment.Amount ?? 0);
            var successFul = entity.Customer.AddCreditWithoutLog(entity.Total - paymentAmount);
            if (!successFul)
            {
                throw new BadCommandException("مقدار اعتبار نمی تواند کمتر از صفر باشد");
            }
        }

        entity.RefundCost(request.Details);

        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(entity.PhoneNumber, request.Details + "\n" + "جزئیات بازگشت وجه به شرح زیر است:" + "\n" + " .به حساب شما بازگشت داده شد " + entity.Token + " با شماره پیگیری " + entity.Number + "مبلغ پرداختی سفارش شما به شماره ");

        return Unit.Value;
    }
}
