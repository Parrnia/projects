namespace Onyx.Application.Services.PaymentServices.Responses;

public class VerifyPaymentResult
{
    public string? CardNumber { get; set; }
    public string? Rrn { get; set; }
    public string? RefId { get; set; }
    public long? Amount { get; set; }
    public string? PayGateTranId { get; set; }
    public long? SalesOrderId { get; set; }
    public int ServiceTypeId { get; set; }

    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public int? OrderId { get; set; }
    public string? SiteUrl { get; set; }

}