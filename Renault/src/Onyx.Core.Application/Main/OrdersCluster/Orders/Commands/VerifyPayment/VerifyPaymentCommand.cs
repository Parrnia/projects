using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application.Services.PaymentServices.Responses;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.VerifyPayment;
public class VerifyPaymentCommand : IRequest<VerifyPaymentResult>
{
    public int PaymentId { get; set; }
}

public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, VerifyPaymentResult>
{
    private readonly PaymentServiceFactory _paymentServiceFactory;
    private readonly AsanPardakhtSettings _asanPardakhtSettings;
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;
    private readonly ISevenSoftService _sevenSoftService;


    public VerifyPaymentCommandHandler(IApplicationDbContext context,
        ISmsService smsService, ISevenSoftService sevenSoftService,
        IOptions<AsanPardakhtSettings> asanPardakhtSettings,
        PaymentServiceFactory paymentServiceFactory)
    {
        _context = context;
        _smsService = smsService;
        _sevenSoftService = sevenSoftService;
        _paymentServiceFactory = paymentServiceFactory;
        _asanPardakhtSettings = asanPardakhtSettings.Value;
    }

    public async Task<VerifyPaymentResult> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(c => c.Customer)
            .ThenInclude(c => c.Credits)
            .Include(c => c.PaymentMethods)
            .Include(c => c.OrderStateHistory)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.AttributeOptions).ThenInclude(c => c.OptionValues)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.ColorOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.MaterialOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .FirstOrDefaultAsync(e => e.PaymentMethods.Any(i => i.Id == request.PaymentId), cancellationToken);

        if (order == null)
        {
            throw new NotFoundException(nameof(OrderPayment), request.PaymentId);
        }

        var payment = order.PaymentMethods
            .OfType<OnlinePayment>()
            .FirstOrDefault(e => e.Id == request.PaymentId);

        if (payment == null)
            throw new Exception("پرداخت نامعتبر است.");

        if (payment.Status != OnlinePaymentStatus.Waiting)
        {
            //todo: درخواست هایی که میاد رو لاگ بندازم چون نباید بیاد
            throw new Exception("وضعیت پرداخت نامعتبر است.");
        }

        var paymentService = _paymentServiceFactory.Create(payment.PaymentServiceType);
        var result = await paymentService.VerifyPayment(payment.Id);
        result.OrderId = payment.OrderId;
        result.SiteUrl = _asanPardakhtSettings.SiteUrl;
        if (result.IsSuccess)
        {
            payment.Status = OnlinePaymentStatus.Completed;
            payment.Amount = result.Amount;
            payment.CardNumber = result.CardNumber;
            payment.PayGateTranId = result.PayGateTranId;
            payment.RefId = result.RefId;
            payment.Rrn = result.Rrn;
            payment.SalesOrderId = result.SalesOrderId;
            payment.ServiceTypeId = result.ServiceTypeId;

            order.IsPayed = true;
            await _smsService.SendSms(order.PhoneNumber,
                ".ثبت شد " + order.Token + " با شماره پیگیری " + order.Number + "سفارش شما به شماره ");
        }
        else
        {
            if (order.OrderPaymentType == OrderPaymentType.CreditOnline)
            {
                var paymentAmount = (decimal)(payment.Amount ?? 0);
                var successFul = order.Customer.AddCreditWithoutLog(order.Total - paymentAmount);
                if (!successFul)
                {
                    throw new BadCommandException("مقدار اعتبار نمی تواند کمتر از صفر باشد");
                }
            }
            payment.Status = OnlinePaymentStatus.Failed;
            payment.Error = result.ErrorMessage;

            await _sevenSoftService.CancelSevenSoftOrder(order.Token, cancellationToken, true);
            order.UnRegisterOrder("پرداخت ناموفق کاربر");
        }

        await _context.SaveChangesAsync(cancellationToken);
        return result;
    }
}
