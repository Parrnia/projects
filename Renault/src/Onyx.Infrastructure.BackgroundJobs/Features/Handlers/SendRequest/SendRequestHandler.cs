using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Application.Settings;
using Onyx.Domain.Enums;
using Onyx.Infrastructure.Persistence;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SendRequest;
public class SendRequestHandler
{
    private readonly ApplicationDbContext _context;
    private string? _accessToken;
    private readonly ApplicationSettings _applicationSettings;
    private readonly SharedService _sharedService;

    public SendRequestHandler(
        ApplicationDbContext context,
        IOptions<ApplicationSettings> applicationSettings,
        SharedService sharedService)
    {
        _context = context;
        _applicationSettings = applicationSettings.Value;
        _sharedService = sharedService;
    }

    public async Task<bool> SendFailedCancelOrderRequests()
    {
        _accessToken = await _sharedService.Authenticate();

        var requestLogs = _context.RequestLogs.Where(c =>
        c.ResponseStatus != HttpStatusCode.OK
        && c.RequestType == RequestType.ResendableCancelOrder).ToList();
        //var orders = _context.Orders.Where(c => requestLogs.Select(e => e.RequestBody).Contains(c.Token)).ToList();
        var taskResult = true;
        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return false;
        }
        using HttpClient client = new HttpClient();
        var completeUrl = _applicationSettings.SevenSoftBaseUrl + "ReturnSale/InsertReturnSale";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        foreach (var requestLog in requestLogs)
        {
            completeUrl += $"?spExchangeId={requestLog.RequestBody}";

            HttpResponseMessage cancelOrderResponse = await client.GetAsync(completeUrl);


            var sevenSoftOrderCancelResponse = new SevenSoftCancelOrderResponse();
            if (cancelOrderResponse.IsSuccessStatusCode)
            {
                var json = await cancelOrderResponse.Content.ReadAsStringAsync();
                sevenSoftOrderCancelResponse = JsonSerializer.Deserialize<SevenSoftCancelOrderResponse>(json);
            }
            if (sevenSoftOrderCancelResponse == null || sevenSoftOrderCancelResponse.IsValid == false)
            {
                taskResult = false;
            }
            else
            {
                requestLog.ResponseStatus = HttpStatusCode.OK;
                //var order = orders.SingleOrDefault(c => c.Token == requestLog.RequestBody);
                //if (order?.PaymentMethods?.PaymentType == PaymentType.Online &&
                //    order.OrderStateHistory.Last().OrderStatus == OrderStatus.OrderRegistered)
                //{
                //    _context.Orders.Remove(order);
                //}
            }
        }

        await _context.SaveChangesAsync();


        return taskResult;
    }

    public async Task<bool> RemoveExpiredLoggedRequests()
    {
        var requestLogs = await _context.RequestLogs.Where(c =>
            (c.ResponseStatus == HttpStatusCode.OK
            && c.Created < DateTime.Now.AddDays(-7))
        ).ToListAsync();

        _context.RemoveRange(requestLogs);
        await _context.SaveChangesAsync();
        return true;
    }
}