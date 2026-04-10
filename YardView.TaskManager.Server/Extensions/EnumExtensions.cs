namespace YardView.TaskManager.Server.Extensions;

public static class EnumExtensions
{
    public static TEnum? ToEnum<TEnum>(this string value) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var lowerValue = value.Trim().ToLowerInvariant();
        if (Enum.TryParse<TEnum>(lowerValue, true, out var result))
            return result;

        return null;
    }
 
}
