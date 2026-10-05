using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PortalDoPublicador.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

        var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute is not null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
        {
            return displayAttribute.Name;
        }

        return enumValue.ToString();
    }
}
