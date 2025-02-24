namespace Onyx.Application.Services.SevenSoftServices;
public class SevenSoftGetInvoiceCommand
{
    /// <summary>
    /// شناسه سفارش
    /// </summary>
    public SpExchanges SpExchanges { get; set; } = null!;
}

public class SpExchanges
{
    public string UniqueId { get; set; } = null!;
}