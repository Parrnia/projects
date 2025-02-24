namespace Onyx.Application.Services.PaymentServices.Responses;

public class TransactionResult
{
    public string CardNumber { get; set; }
    public string Rrn { get; set; }
    public string RefID { get; set; }
    public string Amount { get; set; }
    public string PayGateTranID { get; set; }
    public string SalesOrderID { get; set; }
    public int ServiceTypeId { get; set; }

    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
}