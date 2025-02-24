namespace Onyx.Application.Services.PaymentServices.Requests;

public class TokenRequest
{
    public int ServiceTypeId { get; set; }
    public int MerchantConfigurationId { get; set; }
    public long LocalInvoiceId { get; set; }
    public long AmountInRials { get; set; }
    public string LocalDate { get; set; }
    public string AdditionalData { get; set; }
    public string CallbackURL { get; set; }
    public string PaymentId { get; set; }
}