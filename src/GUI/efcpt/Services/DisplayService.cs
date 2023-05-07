using System;
using System.Linq;
using JetBrains.Annotations;
using Spectre.Console;

namespace ErikEJ.EFCorePowerTools.Services;

internal sealed class DisplayService
{
    public void Title(string message)
    {
        AnsiConsole.Write(
            new FigletText(message)
                .Centered()
                .Color(Color.Aqua));
    }

    public void MarkupLine(string message, Color color, [CanBeNull] Func<string, string> format)
    {
        AnsiConsole.MarkupLine($"[{color}]{format?.Invoke(message) ?? message}[/]");
    }

    public void MarkupLine(string message, Color color)
    {
        MarkupLine(message, color, null);
    }

    public void MarkupLine(params Func<string>[] messages)
    {
        if (messages?.Length < 1)
        {
            AnsiConsole.WriteLine();
            return;
        }

        AnsiConsole.MarkupLine(string.Join(' ', messages!.Select(func => func())));
    }

    public string Markup<TEnum>(string message, TEnum decoration)
        where TEnum : struct
    {
        return $"[{decoration}]{message}[/]";
    }

    public static string Link(string link)
    {
        return $"[link]{link}[/]";
    }

    public void Error(string message)
    {
        MarkupLine(() => Markup("error:", Color.Red), () => message);
    }

    public T Wait<T>(string message, [NotNull] Func<T> doFunc)
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
