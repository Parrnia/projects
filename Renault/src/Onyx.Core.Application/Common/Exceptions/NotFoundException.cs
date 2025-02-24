namespace Onyx.Application.Common.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base($"رکورد \"{name}\" ({key}) یافت نشد.")
    {
    }
}
