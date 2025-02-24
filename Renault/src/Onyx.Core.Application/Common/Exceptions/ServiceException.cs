namespace Onyx.Application.Common.Exceptions;
public class ServiceException : Exception
{
    public ServiceException()
    {
    }

    public ServiceException(string? message) : base(message)
    {
    }
}
