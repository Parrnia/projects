namespace Onyx.Application.Common.Exceptions;

public class BadCommandException : BaseException
{
    public BadCommandException(string command) 
        : base($"امکان اجرای این دستور \"{command}\" وجود ندارد.") 
    { }
}
