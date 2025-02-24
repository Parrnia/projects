using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.RequestsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Services.SevenSoftServices;

public class SevenSoftService : ISevenSoftService
{
    private string? _accessToken;
    private readonly IApplicationDbContext _context;
    private readonly ApplicationSettings _applicationSettings;
    public SevenSoftService(IApplicationDbContext context,
        IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _applicationSettings = applicationSettings.Value;
    }

    #region CreateSevenSoftOrder
    public async Task<string?> CreateSevenSoftOrder(SevenSoftCommand sevenSoftCommand, CancellationToken cancellationToken)
    {
        await Authenticate(cancellationToken);
        using HttpClient client = new HttpClient();
        var completeUrl = new Uri(_applicationSettings.SevenSoftBaseUrl + "Sale/InsertSale");

        var command = JsonSerializer.Serialize(sevenSoftCommand, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
        var content = new StringContent(command, Encoding.UTF8, "application/json");

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return null;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var orderResponse = await client.PostAsync(completeUrl, content, cancellationToken);

        var sevenSoftOrderCreateResponse = new SevenSoftCreateOrderResponse();
        if (orderResponse.IsSuccessStatusCode)
        {
            var json = await orderResponse.Content.ReadAsStringAsync(cancellationToken);
            sevenSoftOrderCreateResponse = JsonSerializer.Deserialize<SevenSoftCreateOrderResponse>(json);
        }

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl.ToString(),
            ResponseStatus = orderResponse.StatusCode,
            RequestType = RequestType.InsertOrder,
            Created = DateTime.Now,
            ApiType = ApiType.Post,
            RequestBody = command,
            ErrorMessage = sevenSoftOrderCreateResponse?.Message
        };

        if (orderResponse.IsSuccessStatusCode && (sevenSoftOrderCreateResponse == null || sevenSoftOrderCreateResponse.AddStatus > 0))
        {
            requestLog.ResponseStatus = HttpStatusCode.BadRequest;
        }

        await _context.RequestLogs.AddAsync(requestLog, cancellationToken);

        if (sevenSoftOrderCreateResponse == null || sevenSoftOrderCreateResponse.AddStatus > 0)
        {
            return null;
        }


        return sevenSoftOrderCreateResponse.ReturnKey;
    }

    #endregion

    #region CancelSevenSoftOrder
    public async Task<bool> CancelSevenSoftOrder(string token, CancellationToken cancellationToken, bool isResendable)
    {
        await Authenticate(cancellationToken);
        using HttpClient client = new HttpClient();
        var completeUrl = _applicationSettings.SevenSoftBaseUrl + "Sale/SpExchangeCancellation";
        completeUrl +=
            $"?spExchangeId={token}";

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return false;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage cancelOrderResponse = await client.GetAsync(completeUrl, cancellationToken);

        var sevenSoftOrderCancelResponse = new SevenSoftCancelOrderResponse();
        if (cancelOrderResponse.IsSuccessStatusCode)
        {
            var json = await cancelOrderResponse.Content.ReadAsStringAsync(cancellationToken);
            sevenSoftOrderCancelResponse = JsonSerializer.Deserialize<SevenSoftCancelOrderResponse>(json);
        }

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl,
            ResponseStatus = cancelOrderResponse.StatusCode,
            Created = DateTime.Now,
            ApiType = ApiType.Post,
            RequestBody = token,
            ErrorMessage = sevenSoftOrderCancelResponse?.Message
        };
        requestLog.RequestType = isResendable ? RequestType.ResendableCancelOrder : RequestType.CancelOrder;

        if (cancelOrderResponse.IsSuccessStatusCode && (sevenSoftOrderCancelResponse == null || sevenSoftOrderCancelResponse.IsValid == false))
        {
            requestLog.ResponseStatus = HttpStatusCode.BadRequest;
        }

        await _context.RequestLogs.AddAsync(requestLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (sevenSoftOrderCancelResponse == null || sevenSoftOrderCancelResponse.IsValid == false)
        {
            return false;
        }

        return true;

    }

    public async Task<bool> CancelSevenSoftOrder(string token ,string accessToken)
    {
        using HttpClient client = new HttpClient();
        var completeUrl = _applicationSettings.SevenSoftBaseUrl + "Sale/SpExchangeCancellation";
        completeUrl +=
            $"?spExchangeId={token}";


        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage cancelOrderResponse = await client.GetAsync(completeUrl);

        var sevenSoftOrderCancelResponse = new SevenSoftCancelOrderResponse();
        if (cancelOrderResponse.IsSuccessStatusCode)
        {
            var json = await cancelOrderResponse.Content.ReadAsStringAsync();
            sevenSoftOrderCancelResponse = JsonSerializer.Deserialize<SevenSoftCancelOrderResponse>(json);
        }

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl,
            ResponseStatus = cancelOrderResponse.StatusCode,
            Created = DateTime.Now,
            ApiType = ApiType.Post,
            RequestBody = token,
            ErrorMessage = sevenSoftOrderCancelResponse?.Message
        };
        requestLog.RequestType = RequestType.ResendableCancelOrder;

        if (cancelOrderResponse.IsSuccessStatusCode && (sevenSoftOrderCancelResponse == null || sevenSoftOrderCancelResponse.IsValid == false))
        {
            requestLog.ResponseStatus = HttpStatusCode.BadRequest;
        }

        await _context.RequestLogs.AddAsync(requestLog);
        return true;
    }

    #endregion

    #region CheckPrices
    public async Task<CheckProductPriceResult> CheckPrices(List<ProductPrice> dbProductPrices, CancellationToken cancellationToken)
    {
        await Authenticate(cancellationToken);
        using HttpClient client = new HttpClient();
        var completeUrl = new Uri(_applicationSettings.SevenSoftBaseUrl + "part/getpartspricebyid?salePriceType=2");

        var productIds = dbProductPrices.Select(c => c.PartId).ToList();
        var jsonIds = JsonSerializer.Serialize(productIds, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var content = new StringContent(jsonIds, Encoding.UTF8, "application/json");

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return new CheckProductPriceResult()
            {
                ProductPrices = null,
                IsValid = false
            };
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var pricesResponse = await client.PostAsync(completeUrl, content, cancellationToken);

        var productPrices = new List<ProductPrice>();
        if (pricesResponse.IsSuccessStatusCode)
        {
            var json = await pricesResponse.Content.ReadAsStringAsync(cancellationToken);
            productPrices = JsonSerializer.Deserialize<List<ProductPrice>>(json);
        }


        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl.ToString(),
            ResponseStatus = pricesResponse.StatusCode,
            RequestType = RequestType.PartPrice,
            Created = DateTime.Now,
            ApiType = ApiType.Post,
            RequestBody = jsonIds,
        };

        if (pricesResponse.IsSuccessStatusCode)
        {
            if (productPrices != null && !productPrices.Any())
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
        }
        await _context.RequestLogs.AddAsync(requestLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);




        foreach (var dbProductPrice in dbProductPrices)
        {
            if (productPrices.Where(productPrice => dbProductPrice.PartId == productPrice.PartId)
                .Any(productPrice => dbProductPrice.Price != productPrice.Price || dbProductPrice.OrderPrice != productPrice.Price))
            {
                return new CheckProductPriceResult()
                {
                    ProductPrices = productPrices,
                    IsValid = false
                };
            }
        }

        return new CheckProductPriceResult()
        {
            ProductPrices = null,
            IsValid = true
        };
    }

    #endregion

    #region CheckCounts
    public async Task<CheckProductCountResult> CheckCounts(List<ProductCount> dbProductCounts, CancellationToken cancellationToken)
    {
        await Authenticate(cancellationToken);
        using HttpClient client = new HttpClient();
        var completeUrl = new Uri(_applicationSettings.SevenSoftBaseUrl + "inventory/getInventorybyid");

        var productIds = dbProductCounts.Select(c => c.PartId).ToList();
        var jsonIds = JsonSerializer.Serialize(productIds, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var content = new StringContent(jsonIds, Encoding.UTF8, "application/json");

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return new CheckProductCountResult()
            {
                ProductCounts = null,
                IsValid = false
            };
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var countsResponse = await client.PostAsync(completeUrl, content, cancellationToken);

        var productCountsMain = new List<ProductCount>();
        var productCountsConverted = new List<ProductCount>();
        if (countsResponse.IsSuccessStatusCode)
        {
            var json = await countsResponse.Content.ReadAsStringAsync(cancellationToken);
            productCountsMain = JsonSerializer.Deserialize<List<ProductCount>>(json)?.ToList();
        }

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl.ToString(),
            ResponseStatus = countsResponse.StatusCode,
            RequestType = RequestType.PartCount,
            Created = DateTime.Now,
            RequestBody = jsonIds,
        };

        if (countsResponse.IsSuccessStatusCode)
        {
            if (productCountsMain != null && !productCountsMain.Any())
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
            else if (productCountsMain != null)
            {
                foreach (var partId in productCountsMain.Select(c => c.PartId).Distinct())
                {
                    var mainProductCount = productCountsMain.SingleOrDefault(c => c.PartId == partId && c.TransactionTypeId == 1);
                    var reservedProductCount = productCountsMain.SingleOrDefault(c => c.PartId == partId && c.TransactionTypeId == 3);
                    mainProductCount!.Number -= reservedProductCount?.Number ?? 0;
                    productCountsConverted.Add(mainProductCount);
                }
            }
        }
        await _context.RequestLogs.AddAsync(requestLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var dbProductCount in dbProductCounts)
        {
            if (productCountsConverted.Where(productCount => dbProductCount.PartId == productCount.PartId)
                .Any(productCount => dbProductCount.OrderQuantity > productCount.Number))
            {
                return new CheckProductCountResult()
                {
                    ProductCounts = productCountsConverted,
                    IsValid = false
                };
            }
        }

        foreach (var dbProductCount in dbProductCounts)
        {
            if (productCountsConverted.Where(productCount => dbProductCount.PartId == productCount.PartId)
                .Any(productCount => dbProductCount.OrderQuantity <= productCount.Number && dbProductCount.Number != productCount.Number))
            {
                return new CheckProductCountResult()
                {
                    ProductCounts = productCountsConverted,
                    IsValid = true
                };
            }
        }

        return new CheckProductCountResult()
        {
            ProductCounts = null,
            IsValid = true
        };
    }

    #endregion

    #region SyncPrices
    public async Task<SyncProductPriceResult> SyncPrices(List<Guid> productIds, CancellationToken cancellationToken)
    {
        await Authenticate(cancellationToken);
        using HttpClient client = new HttpClient();
        var completeUrl = new Uri(_applicationSettings.SevenSoftBaseUrl + "part/getpartspricebyid?salePriceType=2");

        var jsonIds = JsonSerializer.Serialize(productIds, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var content = new StringContent(jsonIds, Encoding.UTF8, "application/json");

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return new SyncProductPriceResult()
            {
                ProductPrices = null,
                IsSuccessful = false
            };
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var pricesResponse = await client.PostAsync(completeUrl, content, cancellationToken);

        if (!pricesResponse.IsSuccessStatusCode)
        {
            return new SyncProductPriceResult() { ProductPrices = null, IsSuccessful = false };
        }

        var json = await pricesResponse.Content.ReadAsStringAsync(cancellationToken);
        var productPrices = JsonSerializer.Deserialize<List<ProductPrice>>(json);

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl.ToString(),
            ResponseStatus = pricesResponse.StatusCode,
            RequestType = RequestType.PartPrice,
            Created = DateTime.Now,
            ApiType = ApiType.Post,
            RequestBody = jsonIds,
        };

        if (pricesResponse.IsSuccessStatusCode)
        {
            if (productPrices != null && !productPrices.Any())
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
        }
        await _context.RequestLogs.AddAsync(requestLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new SyncProductPriceResult()
        {
            ProductPrices = productPrices,
            IsSuccessful = true
        };

    }

    #endregion

    #region SyncCounts
    public async Task<SyncProductCountResult> SyncCounts(List<Guid> productIds, CancellationToken cancellationToken)
    {
        await Authenticate(cancellationToken);
        using HttpClient client = new HttpClient();
        var completeUrl = new Uri(_applicationSettings.SevenSoftBaseUrl + "inventory/getInventorybyid");

        var jsonIds = JsonSerializer.Serialize(productIds, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var content = new StringContent(jsonIds, Encoding.UTF8, "application/json");

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return new SyncProductCountResult()
            {
                ProductCounts = null,
                IsSuccessful = false
            };
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var countsResponse = await client.PostAsync(completeUrl, content, cancellationToken);


        var json = await countsResponse.Content.ReadAsStringAsync(cancellationToken);
        var productCountsMain = JsonSerializer.Deserialize<List<ProductCount>>(json)?.ToList();
        var productCountsConverted = new List<ProductCount>();

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl.ToString(),
            ResponseStatus = countsResponse.StatusCode,
            RequestType = RequestType.PartCount,
            Created = DateTime.Now,
            RequestBody = jsonIds,
        };

        if (countsResponse.IsSuccessStatusCode)
        {
            if (productCountsMain != null && !productCountsMain.Any())
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
            else if (productCountsMain != null)
            {
                foreach (var partId in productCountsMain.Select(c => c.PartId).Distinct())
                {
                    var mainProductCount = productCountsMain.SingleOrDefault(c => c.PartId == partId && c.TransactionTypeId == 1);
                    var reservedProductCount = productCountsMain.SingleOrDefault(c => c.PartId == partId && c.TransactionTypeId == 3);
                    mainProductCount!.Number -= reservedProductCount?.Number ?? 0;
                    productCountsConverted.Add(mainProductCount);
                }
            }
        }
        await _context.RequestLogs.AddAsync(requestLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (!countsResponse.IsSuccessStatusCode)
        {
            return new SyncProductCountResult() { ProductCounts = null, IsSuccessful = false };
        }

        return new SyncProductCountResult()
        {
            ProductCounts = productCountsConverted,
            IsSuccessful = true
        };
    }


    #endregion

    #region Shared
    public async Task<string?> Authenticate(CancellationToken cancellationToken)
    {
        var accessToken = _accessToken;
        var expirationTime = GetExpirationTime(accessToken);

        if (expirationTime.HasValue && DateTime.UtcNow < expirationTime.Value)
        {
            _accessToken = accessToken;
            return accessToken;
        }

        using var client = new HttpClient();

        var authenticate = new Authenticate()
        {
            UserName = _applicationSettings.SevenSoftUserName,
            Password = _applicationSettings.SevenSoftPassword
        };
        var response = await client.PostAsJsonAsync(_applicationSettings.SevenSoftBaseUrl + "Login/Authenticate/", authenticate, cancellationToken: cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            string resContent = await response.Content.ReadAsStringAsync(cancellationToken);
            accessToken = resContent;
        }

        var requestLog = new RequestLog
        {
            ApiAddress = client.BaseAddress + "Login/Authenticate/",
            ResponseStatus = response.StatusCode,
            RequestType = RequestType.Authenticate,
            Created = DateTime.Now,
            ApiType = ApiType.Post
        };
        await _context.RequestLogs.AddAsync(requestLog, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        _accessToken = accessToken;
        return accessToken;
    }

    private static DateTime? GetExpirationTime(string? accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return null;
        }
        var parts = accessToken.Split('.');

        if (parts.Length != 3)
        {
            return null;
        }

        var payload = parts[1];
        var payloadBytes = Convert.FromBase64String(payload);
        var payloadJson = Encoding.UTF8.GetString(payloadBytes);

        var payloadObject = JsonSerializer.Deserialize<JsonElement>(payloadJson);
        var expirationTimeUnix = payloadObject.GetProperty("exp").GetInt64();

        var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expirationTimeUnix).UtcDateTime;

        return expirationTime;
    }

    #endregion

    #region GetInvoice

    public async Task<SevenSoftGetInvoiceResponse?> GetOrderInvoice(string token, CancellationToken cancellationToken)
    {
        await Authenticate(cancellationToken);
        using HttpClient client = new HttpClient();
        var completeUrl = _applicationSettings.SevenSoftBaseUrlSecond + "SpExchangesInvoiceReport/getSpExchangesInformation";
        completeUrl +=
            $"?SpExchangeUniqueId={token}";

        var getInvoiceCommand = new SevenSoftGetInvoiceCommand() { SpExchanges = new SpExchanges() { UniqueId = token } };
        var command = JsonSerializer.Serialize(getInvoiceCommand, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
        var content = new StringContent(command, Encoding.UTF8, "application/json");

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
        if (accessToken == null)
        {
            return null;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage getInvoiceResponse = await client.PostAsync(completeUrl, content, cancellationToken);

        var sevenSoftGetInvoiceResponse = new SevenSoftGetInvoiceResponse();
        if (getInvoiceResponse.IsSuccessStatusCode)
        {
            var json = await getInvoiceResponse.Content.ReadAsStringAsync(cancellationToken);
            sevenSoftGetInvoiceResponse = JsonSerializer.Deserialize<SevenSoftGetInvoiceResponse>(json);
        }

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl,
            ResponseStatus = getInvoiceResponse.StatusCode,
            Created = DateTime.Now,
            ApiType = ApiType.Post,
            RequestBody = token,
            RequestType = RequestType.Invoice
        };

        if (getInvoiceResponse.IsSuccessStatusCode && sevenSoftGetInvoiceResponse == null)
        {
            requestLog.ResponseStatus = HttpStatusCode.BadRequest;
        }

        return sevenSoftGetInvoiceResponse;

    }


    //public async Task<SevenSoftGetInvoiceResponse?> GetReturnOrderInvoice(string token, CancellationToken cancellationToken)
    //{
    //    await Authenticate(cancellationToken);
    //    using HttpClient client = new HttpClient();
    //    var completeUrl = _applicationSettings.SevenSoftBaseUrlSecond + "SpExchangesInvoiceReport/getSpExchangesInformation";
    //    completeUrl +=
    //        $"?SpExchangeUniqueId={token}";

    //    var getInvoiceCommand = new SevenSoftGetInvoiceCommand() { SpExchanges = new SpExchanges() { UniqueId = token } };
    //    var command = JsonSerializer.Serialize(getInvoiceCommand, new JsonSerializerOptions
    //    {
    //        WriteIndented = true,
    //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    //    });
    //    var content = new StringContent(command, Encoding.UTF8, "application/json");

    //    var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);
    //    if (accessToken == null)
    //    {
    //        return null;
    //    }

    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    //    HttpResponseMessage getInvoiceResponse = await client.PostAsync(completeUrl, content, cancellationToken);

    //    var sevenSoftGetInvoiceResponse = new SevenSoftGetInvoiceResponse();
    //    if (getInvoiceResponse.IsSuccessStatusCode)
    //    {
    //        var json = await getInvoiceResponse.Content.ReadAsStringAsync(cancellationToken);
    //        sevenSoftGetInvoiceResponse = JsonSerializer.Deserialize<SevenSoftGetInvoiceResponse>(json);
    //    }

    //    var requestLog = new RequestLog
    //    {
    //        ApiAddress = completeUrl,
    //        ResponseStatus = getInvoiceResponse.StatusCode,
    //        Created = DateTime.Now,
    //        ApiType = ApiType.Post,
    //        RequestBody = token,
    //        RequestType = RequestType.Invoice
    //    };

    //    if (getInvoiceResponse.IsSuccessStatusCode && sevenSoftGetInvoiceResponse == null)
    //    {
    //        requestLog.ResponseStatus = HttpStatusCode.BadRequest;
    //    }

    //    return sevenSoftGetInvoiceResponse;

    //}

    #endregion
}