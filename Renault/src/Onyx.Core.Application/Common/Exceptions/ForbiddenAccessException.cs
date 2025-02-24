namespace Onyx.Application.Common.Exceptions;

public class ForbiddenAccessException : BaseException
{
    public ForbiddenAccessException(string name) 
        : base($"شما به \"{name}\" دسترسی ندارید.") 
    { }
}
