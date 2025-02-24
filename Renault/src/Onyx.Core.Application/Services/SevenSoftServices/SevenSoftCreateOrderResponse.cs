namespace Onyx.Application.Services.SevenSoftServices;
public class SevenSoftCreateOrderResponse
{
    /// <summary>
    /// وضعیت درخواست
    /// </summary>
    public int AddStatus { get; set; }
    /// <summary>
    /// پیام
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// اطلاعات بازگشتی
    /// </summary>
    public dynamic ReturnModel { get; set; }
    /// <summary>
    /// توکن سفارش
    /// </summary>
    public string ReturnKey { get; set; }
}
