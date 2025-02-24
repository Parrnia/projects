using Onyx.Application.Services.PaymentServices.Responses;

namespace Onyx.Application.Services.PaymentServices;

public interface IPaymentService
{
    Task<StartPaymentResult> StartPayment(long amount, long invoiceId, string? mobile);
    Task<VerifyPaymentResult> VerifyPayment(long invoiceId);
}