using System;

namespace EdmxRuleGenerator.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Returns <see cref="Environment.TickCount"/> as a start time in milliseconds as a <see cref="uint"/>.
    /// Use to measure timeouts like this:
    /// uint startTime = 0;
    /// if (millisecondsTimeout != 0 && millisecondsTimeout != Timeout.Infinite) startTime = DateTimeHelper.GetTime();
    /// if (millisecondsTimeout != Timeout.Infinite)
    ///     if (millisecondsTimeout &lt;= (DateTimeHelper.GetTime() - startTime)) return false;
    /// <see cref="Environment.TickCount"/> rolls over from positive to negative every ~25 days, then ~25 days to back to positive again.
    /// <see cref="uint"/> is used to ignore the sign and double the range to 50 days.
    /// </summary>
    public static uint GetTime()
    {
        return (uint)Environment.TickCount;
    }

    /// <summary>
    /// Returns <see cref="Environment.TickCount"/> as a start time in milliseconds as a <see cref="uint"/>.
    /// Use to measure timeouts like this:
    /// uint startTime = 0;
    /// if (millisecondsTimeout != 0 && millisecondsTimeout != Timeout.Infinite) startTime = DateTimeHelper.GetTime();
    /// if (millisecondsTimeout != Timeout.Infinite)
    ///     if (millisecondsTimeout &lt;= (DateTimeHelper.GetTime() - startTime)) return false;
    /// <see cref="Environment.TickCount"/> rolls over from positive to negative every ~25 days, then ~25 days to back to positive again.
    /// <see cref="uint"/> is used to ignore the sign and double the range to 50 days.
    /// </summary>
    public static uint GetTime(this object o)
    {
        return (uint)Environment.TickCount;
    }
}
