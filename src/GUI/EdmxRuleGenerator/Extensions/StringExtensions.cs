using System.Collections.Generic;
using System.Diagnostics;

namespace EdmxRuleGenerator.Extensions;

public static class StringExtensions
{
    /// <summary> Return null if the given value is empty.  Return the original value otherwise. </summary>
    [DebuggerStepThrough]
    public static string NullIfEmpty(this string str) { return string.IsNullOrEmpty(str) ? null : str; }

    /// <summary> Return null if the given value is whitespace or empty.  Return the original value otherwise. </summary>
    [DebuggerStepThrough]
    public static string NullIfWhitespace(this string str) { return string.IsNullOrWhiteSpace(str) ? null : str; }

    /// <summary> Indicates whether the specified string is null or an System.String.Empty string. </summary>
    [DebuggerStepThrough]
    public static bool IsNullOrEmpty(this string str) { return string.IsNullOrEmpty(str); }

    /// <summary> Indicates whether the specified string is null or an System.String.Empty string. </summary>
    [DebuggerStepThrough]
    public static bool HasCharacters(this string str) { return !string.IsNullOrEmpty(str); }

    /// <summary> Indicates whether a specified string is null, empty, or consists only of white-space characters. </summary>
    [DebuggerStepThrough]
    public static bool IsNullOrWhiteSpace(this string str) { return string.IsNullOrWhiteSpace(str); }

    /// <summary> Indicates whether a specified string is null, empty, or consists only of white-space characters. </summary>
    [DebuggerStepThrough]
    public static bool HasNonWhiteSpace(this string str) { return !string.IsNullOrWhiteSpace(str); }

    /// <summary>
    /// Concatenates the members of a constructed System.Collections.Generic.IEnumerable
    /// <T> collection of type System.String, using the specified separator between each member.
    /// </summary>
    [DebuggerStepThrough]
    public static string Join(this IEnumerable<string> strs, string separator = ", ")
    {
        return string.Join(separator, strs);
    }
}
