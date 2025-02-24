namespace Onyx.Application.Services.SevenSoftServices;
public class SevenSoftOrderProductCommand
{
    /// <summary>
    /// شماره قطعه
    /// </summary>
    public Guid? PartId { get; set; }
    /// <summary>
    /// تعداد
    /// </summary>
    public double ExchangeNumber { get; set; }
    /// <summary>
    /// واحد قیمت
    /// </summary>
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// مبلغ اضافه
    /// </summary>
    public decimal ExtraPrice { get; set; }
    /// <summary>
    /// مقدار تخفیف
    /// </summary>
    public decimal DiscountValue { get; set; }
    /// <summary>
    /// درصد تخفیف
    /// </summary>
    public double DiscountPercent { get; set; }
    /// <summary>
    /// امکان پرداخت برای مشتری
    /// </summary>
    public bool IsPayableForCustomer { get; set; }
    /// <summary>
    /// نوع آیتم
    /// </summary>
    public string ItemTypeId { get; set; } = null!;
}
