using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.OpenApi.Extensions;

namespace AppCore.Extensions;

public static class EnumExtension
{
    public static List<EnumValue> GetValues<T>()
    {
        var values = new List<EnumValue>();
        foreach (var itemType in Enum.GetValues(typeof(T)))
        {
            var type = itemType.GetType();
            var field = type.GetField(itemType.ToString() ?? string.Empty);
            values.Add(new EnumValue
            {
                Value = (int)itemType,
                Name = Enum.GetName(typeof(T), itemType),
                DisplayName = field != null ? field.GetCustomAttribute<DisplayAttribute>()?.Name : null
            });
        }

        return values;
    }

    public static T GetEnum<T>(this string enumString) where T : Enum
    {
        try
        {
            var enumValue = Enum.Parse(typeof(T), enumString);
            return (T)enumValue;
        }
        catch
        {
            return default;
        }
    }

    public static string GetDisplayNameV2(this Enum enumValue)
    {
        return enumValue.GetAttributeOfType<DisplayAttribute>().GetName();
    }

    public static string GetDisplayName<T>(this T enumValue) where T : struct, Enum
    {
        var values = Enum.GetValues<T>().ToList();
        if (!values.Contains(enumValue))
            return string.Empty;
        return enumValue.GetAttributeOfType<DisplayAttribute>().GetName();
    }

    public class EnumValue
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}