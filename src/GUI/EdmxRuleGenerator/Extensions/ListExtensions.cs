using System.Collections.Generic;
using System.Diagnostics;

namespace EdmxRuleGenerator.Extensions;

public static class ListExtensions
{
    [DebuggerStepThrough]
    public static bool IsNullOrEmpty<T>(this IList<T> list) { return list == null || list.Count == 0; }

    [DebuggerStepThrough]
    public static T TryGetElement<T>(this IList<T> list, int index)
        where T : class
    {
        return list?.Count > index ? list[index] : default;
    }
}
