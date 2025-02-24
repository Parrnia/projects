using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services.PaymentServices;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Enums;
using Onyx.Infrastructure.Persistence;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManagePayments;
public class ManagePaymentsHandler
{
    private readonly ApplicationDbContext _context;
    private string? _accessToken;
    private readonly ApplicationSettings _applicationSettings;
    private readonly SharedService _sharedService;
    private readonly PaymentServiceFactory _paymentServiceFactory;
    private readonly ISmsService _smsService;
    private readonly ISevenSoftService _sevenSoftService;

    public ManagePaymentsHandler(
        IOptions<ApplicationSettings> applicationSettings,
        ApplicationDbContext context,
        SharedService sharedService,
        ISmsService smsService,
        ISevenSoftService sevenSoftService, PaymentServiceFactory paymentServiceFactory)
    {
        _context = context;
        _sharedService = sharedService;
        _smsService = smsService;
        _sevenSoftService = sevenSoftService;
        _paymentServiceFactory = paymentServiceFactory;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<bool> VerifyWaitingPayments()
    {
        _accessToken = await _sharedService.Authenticate();

        var halfHour = TimeSpan.FromMinutes(30);
        var currentTime = DateTime.Now;

        var payments = await _context.OrderPayments
            .OfType<OnlinePayment>()
            .Include(c => c.Order)
            .Where(c =>
                !c.Order.IsPayed &&
                c.Status == OnlinePaymentStatus.Waiting).ToListAsync();

        foreach (var order in payments.Where(c => c.Created - currentTime > halfHour).Select(c => c.Order))
        {
            if (order.PaymentMethods.OrderBy(e => e.Created).LastOrDefault() is OnlinePayment payment)
            {
                var paymentService = _paymentServiceFactory.Create(payment.PaymentServiceType);
                var result = await paymentService.VerifyPayment(payment.Id);

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
                    await _smsService.SendSms(order.PhoneNumber, ".ثبت شد " + order.Token + " با شماره پیگیری " + order.Number + "سفارش شما به شماره ");
                }
                else
                {
                    payment.Status = OnlinePaymentStatus.Expired;
                    payment.Error = result.ErrorMessage;
                    await _sevenSoftService.CancelSevenSoftOrder(order.Token, _accessToken);
                    order.UnRegisterOrder("پرداخت ناموفق کاربر");
                }
            }
        }
        await _context.SaveChangesAsync();

        return true;
    }
}