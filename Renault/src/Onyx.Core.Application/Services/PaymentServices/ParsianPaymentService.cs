using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onyx.Application.Services.PaymentServices.Responses;

namespace Onyx.Application.Services.PaymentServices;
public class ParsianPaymentService : IPaymentService
{
    public Task<StartPaymentResult> StartPayment(long amount, long invoiceId, string? mobile)
    {
        throw new NotImplementedException();
    }

    public Task<VerifyPaymentResult> VerifyPayment(long invoiceId)
    {
        throw new NotImplementedException();
    }
}
