using Microsoft.Extensions.DependencyInjection;
using Onyx.Domain.Enums;

namespace Onyx.Application.Services.PaymentServices;

public class PaymentServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentService Create(PaymentServiceType paymentServiceType)
    {
        switch (paymentServiceType)
        {
            case PaymentServiceType.AsanPardakht:
                return _serviceProvider.GetRequiredService<AsanPardakhtPaymentService>();
            case PaymentServiceType.Parsian:
                return _serviceProvider.GetRequiredService<ParsianPaymentService>();
            default:
                throw new ArgumentOutOfRangeException($"no payment service defined for type {paymentServiceType}");
        }
    }
}
