namespace Onyx.Application.Common.Exceptions;
public class OrderException : BaseException
{
    public OrderException()
    {
    }

    public OrderException(string message) : base(message)
    {
    }
}
