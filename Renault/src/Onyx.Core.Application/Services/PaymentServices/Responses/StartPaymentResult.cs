namespace Onyx.Application.Services.PaymentServices.Responses;
public class StartPaymentResult
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public string? PaymentUrl { get; set; }
    public string? ErrorMessage { get; set; }
}
