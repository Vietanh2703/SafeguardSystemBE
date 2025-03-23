using System.ComponentModel;
using System.Reflection;

namespace SafeguardSystem.Common.Enums;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute == null ? enumValue.ToString() : attribute.Description;
    }
}