namespace Onyx.Application.Services.SevenSoftServices;
public class SevenSoftOrderCommand
{
    /// <summary>
    /// نام مشتری
    /// </summary>
    public string? AddSpExchangesVmSubscriberName { get; set; }
    /// <summary>
    /// کد ملی مشتری
    /// </summary>
    public string? AddSpExchangesVmSubscriberNationalCode { get; set; }
    /// <summary>
    /// تلفن مشتری
    /// </summary>
    public string? AddSpExchangesVmSubscriberTel { get; set; }
    /// <summary>
    /// توضیحات فروش
    /// </summary>
    public string? AddSpExchangesVmDescription { get; set; }
    public List<SevenSoftOrderProductCommand> AddSpExchangesVmSpExchangesParts { get; set; } = new List<SevenSoftOrderProductCommand>();
    /// <summary>
    /// مقدار کل تخفیف
    /// </summary>
    public decimal AddSpExchangesVmDiscountValue { get; set; }
    /// <summary>
    /// تخفیف درصدی
    /// </summary>
    public double AddSpExchangesVmDiscountPercent { get; set; }
    /// <summary>
    /// کد فاکتور فروش سمت سایت
    /// </summary>
    public string RelatedCode { get; set; } = null!;
}
