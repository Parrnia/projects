namespace Onyx.Domain.Entities.OrdersCluster;
public class OrderAddress : BaseAuditableEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// شرکت
    /// </summary>
    public string? Company { get; set; }
    /// <summary>
    /// کشور
    /// </summary>
    public string Country { get; set; } = null!;
    /// <summary>
    /// جزئیات آدرس
    /// </summary>
    public string AddressDetails1 { get; set; } = null!;
    /// <summary>
    /// جزئیات بیشتر آدرس
    /// </summary>
    public string? AddressDetails2 { get; set; }
    /// <summary>
    /// شهر
    /// </summary>
    public string City { get; set; } = null!;
    /// <summary>
    /// منطقه
    /// </summary>
    public string State { get; set; } = null!;
    /// <summary>
    /// کد پستی
    /// </summary>
    public string Postcode { get; set; } = null!;
}