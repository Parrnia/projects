using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Onyx.Domain.Enums;
public static class EnumHelper<T>
    where T : struct, Enum 
{
    public static IList<T> GetValues(Enum value)
    {
        var enumValues = new List<T>();

        foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
        }
        return enumValues;
    }

    public static T Parse(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static IList<string> GetNames(Enum value)
    {
        return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
    }

    public static IList<string> GetDisplayValues(Enum value)
    {
        return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
    }

    private static string lookupResource(Type resourceManagerProvider, string resourceKey)
    {
        var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
            BindingFlags.Static | BindingFlags.Public, null, typeof(string),
            new Type[0], null);
        if (resourceKeyProperty != null)
        {
            return (string)resourceKeyProperty.GetMethod.Invoke(null, null);
        }

        return resourceKey; // Fallback with the key name
    }

    public static string? GetDisplayValue(T? value)
    {
        if (value == null)
        {
            return null;
        }
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = fieldInfo?.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

        Type? resourceManagerProvider = descriptionAttributes?[0].ResourceType;
        if (resourceManagerProvider != null)
        {
            string? resourceKey = descriptionAttributes?[0].Name;
            if (resourceKey != null)
            {
                return lookupResource(resourceManagerProvider, resourceKey);
            }
        }

        if (descriptionAttributes == null) return string.Empty;
        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }
}