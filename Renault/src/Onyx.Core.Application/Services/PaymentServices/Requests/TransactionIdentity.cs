namespace Onyx.Application.Services.PaymentServices.Requests
{
    public class TransactionIdentity
    {
        public int MerchantConfigurationId { get; set; }
        public string PayGateTranId { get; set; }
    }
}
