namespace Onyx.Application.Common.Middlewares;
public class ResponseError
{

    public ResponseError(string message, int status)
    {
        Message = message;
        Status = status;
    }
    
    public string Message { get; set; }
    public int Status { get; set; }
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

    public ResponseError()
    {
    }

}
