namespace Onyx.Application.Helpers;
public static class ValidationHelperMethods
{
    public static bool BeAValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
