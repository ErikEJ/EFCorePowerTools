using System;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Spectre.Console;

namespace ErikEJ.EFCorePowerTools.Services;

internal static class DisplayService
{
    public static string Link(string link)
    {
        return $"[link]{link}[/]";
    }

    public static void Title(string message)
    {
        AnsiConsole.Write(
            new FigletText(message)
                .Centered()
                .Color(Color.Aqua));
    }

    public static void MarkupLine(string message, Color color, Func<string, string> format)
    {
        AnsiConsole.MarkupLine($"[{color}]{format?.Invoke(message) ?? message}[/]");
    }

    public static void MarkupLine(string message, Color color)
    {
        MarkupLine(message, color, null);
    }

    public static void MarkupLine(params Func<string>[] messages)
    {
        if (messages?.Length < 1)
        {
            AnsiConsole.WriteLine();
            return;
        }

        AnsiConsole.MarkupLine(string.Join(' ', messages!.Select(func => func())));
    }

    public static string Markup<TEnum>(string message, TEnum decoration)
        where TEnum : struct
    {
        return $"[{decoration}]{message}[/]";
    }

    public static void Error(string message)
    {
        AnsiConsole.MarkupLineInterpolated(CultureInfo.InvariantCulture, $"[red]error: {message}[/]");
    }

    public static T Wait<T>(string message, Func<T> doFunc)
    {
        T result = default;
        AnsiConsole.Status()
            .Start(message, ctx =>
            {
                ctx.Spinner(Spinner.Known.Ascii);
                ctx.SpinnerStyle(Style.Parse(Color.Green.ToString()));
                result = doFunc();
            });

        return result;
    }
}
