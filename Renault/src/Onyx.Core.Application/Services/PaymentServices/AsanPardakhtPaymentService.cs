using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Irony.Parsing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Onyx.Application.Services.PaymentServices.Requests;
using Onyx.Application.Services.PaymentServices.Responses;
using Onyx.Application.Settings;

namespace Onyx.Application.Services.PaymentServices;

public class AsanPardakhtPaymentService : IPaymentService
{
    private readonly AsanPardakhtSettings _asanPardakhtSettings;
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public AsanPardakhtPaymentService(IOptions<AsanPardakhtSettings> asanPardakhtSettings,
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(AppConstants.AsanPardakhtHttpClient);
        _asanPardakhtSettings = asanPardakhtSettings.Value;
    }

    public async Task<StartPaymentResult> StartPayment(long amount, long invoiceId, string? mobile)
    {
        try
        {
            var time = (await GetTime()).Result ?? "";
            var request = new TokenRequest()
            {
                ServiceTypeId = 1, // sale: 1
                AmountInRials = amount,
                CallbackURL = $"{_asanPardakhtSettings.CallbackUrl}?paymentId={invoiceId}",
                LocalDate = time,
                LocalInvoiceId = invoiceId,
                MerchantConfigurationId = _asanPardakhtSettings.MerchantConfigId,
                PaymentId = "0",
                AdditionalData = "",
            };

            var tokenResponse = await Token(request);
            if (!tokenResponse.IsSuccess || string.IsNullOrEmpty(tokenResponse.Result))
                throw new Exception(tokenResponse.GetErrorMessage());

            var result = new StartPaymentResult()
            {
                IsSuccess = true,
                Token = tokenResponse.Result,
                PaymentUrl = CreatePaymentUrl(tokenResponse.Result, mobile),
                ErrorMessage = null
            };

            return result;
        }
        catch (Exception ex)
        {
            return new StartPaymentResult()
            {
                PaymentUrl = null,
                Token = null,
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<VerifyPaymentResult> VerifyPayment(long invoiceId)
    {
        try
        {
            var transactionResult = await TransactionResult(invoiceId);
            if (!transactionResult.IsSuccess || transactionResult.Result is null)
                throw new Exception(transactionResult.GetErrorMessage());

            var transactionIdentity = new TransactionIdentity()
            {
                MerchantConfigurationId = _asanPardakhtSettings.MerchantConfigId,
                PayGateTranId = transactionResult.Result.PayGateTranID
            };

            var verifyResult = await Verify(transactionIdentity);
            if (verifyResult.IsSuccess)
            {
                var settlementResult = await Settlement(transactionIdentity);
                if (!settlementResult.IsSuccess)
                {
                    await Cancel(transactionIdentity);
                    throw new Exception(settlementResult.GetErrorMessage());
                }
            }
            else
            {
                await Reverse(transactionIdentity);
                throw new Exception(verifyResult.GetErrorMessage());
            }

            var result = new VerifyPaymentResult()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Amount = string.IsNullOrEmpty(transactionResult.Result.Amount) ? null : (long)double.Parse(transactionResult.Result.Amount),
                CardNumber = transactionResult.Result.CardNumber,
                PayGateTranId = transactionResult.Result.PayGateTranID,
                RefId = transactionResult.Result.RefID,
                Rrn = transactionResult.Result.Rrn,
                SalesOrderId = string.IsNullOrEmpty(transactionResult.Result.SalesOrderID) ? null : long.Parse(transactionResult.Result.SalesOrderID),
                ServiceTypeId = transactionResult.Result.ServiceTypeId
            };
            return result;
        }
        catch (Exception ex)
        {
            return new VerifyPaymentResult()
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
            };
        }
    }


    #region AsanPardakht

    public async Task<AsanPardakhtResponse<string>> GetTime()
    {
        var response = await HttpSend<string, string>("/v1/Time",
            HttpMethod.Get, null, null);
        return response;
    }

    private async Task<AsanPardakhtResponse<string>> Token(TokenRequest request)
    {
        var response = await HttpSend<TokenRequest, string>("/v1/Token", HttpMethod.Post,
            request, AuthHeaders());
        return response;
    }

    private async Task<AsanPardakhtResponse<TransactionResult>> TransactionResult(long invoiceId)
    {
        var queryString = $"merchantConfigurationId={_asanPardakhtSettings.MerchantConfigId}&localInvoiceId={invoiceId}";
        var response = await HttpSend<string, TransactionResult>($"/v1/TranResult?{queryString}", HttpMethod.Get,
           null, AuthHeaders());
        return response;
    }

    private async Task<AsanPardakhtResponse<string>> Verify(TransactionIdentity transaction)
    {
        var response = await HttpSend<TransactionIdentity, string>($"/v1/Verify", HttpMethod.Post,
            transaction, AuthHeaders());
        return response;
    }

    private async Task<AsanPardakhtResponse<string>> Settlement(TransactionIdentity transaction)
    {
        var response = await HttpSend<TransactionIdentity, string>($"/v1/Settlement", HttpMethod.Post,
            transaction, AuthHeaders());
        return response;
    }

    private async Task<AsanPardakhtResponse<string>> Reverse(TransactionIdentity transaction)
    {
        var response = await HttpSend<TransactionIdentity, string>($"/v1/Reverse", HttpMethod.Post,
            transaction, AuthHeaders());
        return response;
    }

    private async Task<AsanPardakhtResponse<string>> Cancel(TransactionIdentity transaction)
    {
        var response = await HttpSend<TransactionIdentity, string>($"/v1/Cancel", HttpMethod.Post,
            transaction, AuthHeaders());
        return response;
    }

    #endregion


    #region Helpers

    private Dictionary<string, string> AuthHeaders()
    {
        return new Dictionary<string, string>()
        {
            ["usr"] = _asanPardakhtSettings.UserName,
            ["pwd"] = _asanPardakhtSettings.Password
        };
    }

    private string? CreatePaymentUrl(string token, string? mobile)
    {
        var mobileAp = string.IsNullOrEmpty(mobile) ? "" : $"mobileap={mobile}";
        return $"{_asanPardakhtSettings.PaymentUrl}?RefId={token}&{mobileAp}";
    }

    private async Task<AsanPardakhtResponse<TResponse>> HttpSend<TRequest, TResponse>(string url, HttpMethod method,
        TRequest? request, Dictionary<string, string>? headers)
    {
        try
        {
            var httpRequest = new HttpRequestMessage(method, url);
            if (headers is not null)
            {
                foreach (var header in headers)
                    httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            if (request is not null)
            {
                var requestBody = JsonSerializer.Serialize(request, JsonOptions);
                httpRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            }

            var httpResponse = await _httpClient.SendAsync(httpRequest);

            string? responseBody = null;
            if (httpResponse.Content.Headers.ContentLength is null or > 0)
                responseBody = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                ErrorResponse? error = null;
                if (responseBody is not null)
                    error = JsonSerializer.Deserialize<ErrorResponse>(responseBody, JsonOptions);
                error ??= new ErrorResponse();
                error.Error ??= new AsanPardakhtError();
                return new AsanPardakhtResponse<TResponse>()
                {
                    Result = default,
                    Error = error,
                    StatusCode = (int)httpResponse.StatusCode,
                };
            }

            TResponse? response = default;
            if (responseBody is not null)
                response = JsonSerializer.Deserialize<TResponse>(responseBody, JsonOptions);
            return new AsanPardakhtResponse<TResponse>()
            {
                Result = response,
                Error = default,
                StatusCode = (int)httpResponse.StatusCode,
            };

        }
        catch (Exception ex)
        {
            return new AsanPardakhtResponse<TResponse>()
            {
                Error = new ErrorResponse()
                {
                    Error = new AsanPardakhtError() { Message = ex.Message, Code = -1 }
                },
                Result = default,
                StatusCode = 0
            };
        }

    }

    #endregion

}
