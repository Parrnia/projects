namespace Onyx.Application.Services.PaymentServices.Responses;

public class AsanPardakhtResponse<TResponse>
{
    public TResponse? Result { get; set; }
    public ErrorResponse? Error { get; set; }
    public bool IsSuccess => Error?.Error is null;
    public int StatusCode { get; set; }

    public string GetErrorMessage()
    {
        if (Error?.Error is not null)
            return $"{Error.Error.Message} ({Error.Error.Code})";

        return $"Error Status Code {StatusCode}";
    }
}

public class ErrorResponse
{
    public AsanPardakhtError? Error { get; set; }
}

public class AsanPardakhtError
{
    public int Code { get; set; }
    public string Message { get; set; }
    public Dictionary<string, object> Args { get; set; }
}
