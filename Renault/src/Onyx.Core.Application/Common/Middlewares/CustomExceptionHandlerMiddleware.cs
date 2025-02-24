using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onyx.Application.Common.Exceptions;
using Onyx.Domain.Exceptions;

namespace Onyx.Application.Common.Middlewares;

public class CustomExceptionHandlerMiddleware : IMiddleware
{
    private ILogger<CustomExceptionHandlerMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            CreateLog(exception);
            if (context.Response.HasStarted)
                return;

            var result = CreateErrorResponse(exception);
            await SetResponse(context, result);
        }
    }

    private void CreateLog(Exception exception)
    {
        var response = CreateErrorResponse(exception);
        if (response.Status < 500)
        {
            _logger.LogWarning(exception.Message);
        }
        else
        {
            _logger.LogError(exception, exception.Message);
        }
    }

    private static ResponseError CreateErrorResponse(Exception mainException)
    {
        ResponseError result;
        var exception = mainException;
        if (exception is AggregateException aggregated)
            exception = aggregated.InnerException;

        switch (exception)
        {
            case OrderException customerCreditIsInsufficientException:
                result = new ResponseError(exception.Message, 400) 
                    { Errors = customerCreditIsInsufficientException.Errors };
                break;
            case ForbiddenAccessException _:
                result = new ResponseError(exception.Message, 403);
                break;
            case NotFoundException _:
                result = new ResponseError(exception.Message, 404);
                break;
            case ValidationException validationException:
                result = new ResponseError(validationException.Message, 400) 
                    {Errors =validationException.Errors};
                break;
            case ServiceException _:
                result = new ResponseError(exception.Message, 400);
                break;
            case SevenException _:
                result = new ResponseError(exception.Message, 400);
                break;
            case DomainException _:
                result = new ResponseError(exception.Message, 400);
                break;
            case BadCommandException _:
                result = new ResponseError(exception.Message, 400);
                break;
            case ReturnOrderException _:
                result = new ResponseError(exception.Message, 400);
                break;
            default:
                result = new ResponseError("خطایی در سمت سرور رخ داده است.", 500);
                break;
        }

        return result;
    }


    private static async Task SetResponse(HttpContext context, ResponseError error)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.StatusCode = error.Status;
        context.Response.ContentType = "application/json";
        var content = JsonSerializer.SerializeToUtf8Bytes(error, JsonOptions);
        await context.Response.Body.WriteAsync(content);
    }
}