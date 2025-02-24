namespace Onyx.Web.FrontOffice;

public interface IResponseMessage
{
    string? Message { get; }
}

public class ResponseMessage : IResponseMessage
{
    public ResponseMessage(string message)
    {
        Message = message;
    }

    public string? Message { get; set; }

    public ResponseMessage()
    {
    }
}

public class ResponseMessage<T> : ResponseMessage
{
    public ResponseMessage(string message, T content)
    {
        Message = message;
        Content = content;
    }

    public T? Content { get; set; }

    public ResponseMessage()
    {
    }
}

