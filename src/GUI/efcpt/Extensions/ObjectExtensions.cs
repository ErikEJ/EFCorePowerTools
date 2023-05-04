using System.Linq;
using JetBrains.Annotations;

namespace ErikEJ.EfCorePowerTools.Extensions;

internal static class ObjectExtensions
{
    internal static void CopyTo<T>([NotNull]this T source, [NotNull] T destination)
        where T : class
    {
        var properties = source.GetType().GetProperties().Where(p => p.CanWrite);

        foreach (var property in properties)
        {
            property.SetValue(destination, property.GetValue(source));
        }
    }
}
