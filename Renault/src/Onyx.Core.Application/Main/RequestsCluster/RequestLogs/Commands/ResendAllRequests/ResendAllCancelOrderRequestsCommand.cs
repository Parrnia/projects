using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Application.Settings;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Commands.ResendAllRequests;

public record ResendAllCancelOrderRequestsCommand : IRequest<bool>;


public class ResendAllCancelOrderRequestsCommandHandler : IRequestHandler<ResendAllCancelOrderRequestsCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;
    private readonly ApplicationSettings _applicationSettings;

    public ResendAllCancelOrderRequestsCommandHandler(
        IApplicationDbContext context,
        ISevenSoftService sevenSoftService,
        IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<bool> Handle(ResendAllCancelOrderRequestsCommand request, CancellationToken cancellationToken)
    {
        var requestLogs = await _context.RequestLogs.Where(c =>
            c.ResponseStatus != HttpStatusCode.OK
            && c.RequestType == RequestType.ResendableCancelOrder
        ).ToListAsync(cancellationToken);
        //var orders = await _context.Orders.Where(c => requestLogs.Select(e => e.RequestBody).Contains(c.Token)).ToListAsync(cancellationToken);
        var taskResult = true;
        var accessTokenValue = await _sevenSoftService.Authenticate(cancellationToken);
        var accessToken = accessTokenValue?.Substring(1, accessTokenValue.Length - 2);
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

            HttpResponseMessage cancelOrderResponse = await client.GetAsync(completeUrl, cancellationToken);


            var sevenSoftOrderCancelResponse = new SevenSoftCancelOrderResponse();
            if (cancelOrderResponse.IsSuccessStatusCode)
            {
                var json = await cancelOrderResponse.Content.ReadAsStringAsync(cancellationToken);
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

        return taskResult;
    }
}
