namespace Onyx.Application.Common.Exceptions;

public class BaseException : Exception
{
    public BaseException()
        : base()
    {
        Errors = new Dictionary<string, string[]>();
    }

    public BaseException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
        Errors.Add("خطا" ,new []{message});
    }

    public BaseException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public IDictionary<string, string[]> Errors { get; }

}
