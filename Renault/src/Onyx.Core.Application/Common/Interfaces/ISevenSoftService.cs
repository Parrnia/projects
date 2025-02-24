using Onyx.Application.Services.SevenSoftServices;

namespace Onyx.Application.Common.Interfaces;
public interface ISevenSoftService
{
    public Task<string?> CreateSevenSoftOrder(
        SevenSoftCommand sevenSoftCommand,
        CancellationToken cancellationToken);

    public Task<bool> CancelSevenSoftOrder(
        string token,
        CancellationToken cancellationToken,
        bool isResendable);

    public Task<bool> CancelSevenSoftOrder(
        string token,
        string accessToken);

    public Task<CheckProductPriceResult> CheckPrices(
        List<ProductPrice> dbProductPrices,
        CancellationToken cancellationToken);

    public Task<CheckProductCountResult> CheckCounts(
        List<ProductCount> dbProductCounts,
        CancellationToken cancellationToken);

    public Task<SyncProductPriceResult> SyncPrices(
        List<Guid> productIds,
        CancellationToken cancellationToken);

    public Task<SyncProductCountResult> SyncCounts(
        List<Guid> productIds,
        CancellationToken cancellationToken);

    public Task<string?> Authenticate(
        CancellationToken cancellationToken);

    public Task<SevenSoftGetInvoiceResponse?> GetOrderInvoice(
        string token,
        CancellationToken cancellationToken);

    //public Task<SevenSoftGetInvoiceResponse?> GetReturnOrderInvoice(
    //    string token,
    //    CancellationToken cancellationToken);
}
