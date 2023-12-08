using System.Linq;
using JetBrains.Annotations;

namespace ErikEJ.EFCorePowerTools.Extensions;

internal static class ObjectExtensions
{
    internal static void CopyTo<T>(this T source, T destination)
        where T : class
    {
        var properties = source.GetType().GetProperties().Where(p => p.CanWrite);

        foreach (var property in properties)
        {
            property.SetValue(destination, property.GetValue(source));
        }
    }
}
