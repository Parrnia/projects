namespace Onyx.Application.Services.SevenSoftServices;
public class SevenSoftCommand
{
    /// <summary>
    /// جزئیات سفارش
    /// </summary>
    public SevenSoftOrderCommand AddSpExchanges { get; set; } = null!;
    /// <summary>
    /// جزئیات مشتری
    /// </summary>
    public SevenSoftOrderCustomerCommand SubscriberComplete { get; set; } = null!;
}
