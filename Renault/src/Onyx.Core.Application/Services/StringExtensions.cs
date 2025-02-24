namespace Onyx.Application.Services;
public static class StringExtensions
{
    public static bool Match(this string? searchText, string? value)
    {
        if (searchText == null || value == null)
        {
            return true;
        }
        return String.Equals(searchText, value, StringComparison.CurrentCultureIgnoreCase);
    }
}
