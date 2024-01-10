using System;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using ErikEJ.EFCorePowerTools.HostedServices;
using ErikEJ.EFCorePowerTools.Services;
using Microsoft.Extensions.Hosting;
using RevEng.Core;
using Spectre.Console;

namespace ErikEJ.EFCorePowerTools;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            return await MainAsync(args).ConfigureAwait(false);
        }
#pragma warning disable CA1031
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex.Demystify(), ExceptionFormats.ShortenPaths);
            return 1;
        }
#pragma warning restore CA1031
    }

    public static async Task<int> MainAsync(string[] args)
    {
        using var parser = new Parser(c =>
        {
            c.HelpWriter = null;
            c.CaseInsensitiveEnumValues = true;
        });

        var parserResult = parser.ParseArguments<ScaffoldOptions>(args);

        var result = parserResult
            .MapResult(
                async options =>
                {
                    ResolveProvider(options);

                    var fileSystem = new FileSystem();

                    options.ConfigFile = options.ConfigFile ?? new FileInfo(fileSystem.Path.GetFullPath(Constants.ConfigFileName));

                    DisplayHeader(options);
                    var hostBuilder = new HostBuilder();
                    await hostBuilder.Configure()
                        .RegisterServices(fileSystem, options)
                        .StartAsync()
                        .ConfigureAwait(false);
                    return Environment.ExitCode;
                },
                async _ => await DisplayHelpAsync(parserResult).ConfigureAwait(false));

        await PackageService.CheckForPackageUpdateAsync().ConfigureAwait(false);

        return await result.ConfigureAwait(false);
    }

    private static void ResolveProvider(ScaffoldOptions options)
    {
        if (string.IsNullOrEmpty(options.Provider))
        {
            var resolver = new ConnectionStringResolver(options.ConnectionString);
            var providers = resolver.ResolveAlias();

            if (providers.Count == 1)
            {
                options.Provider = providers[0];
            }

            if (providers.Count != 1)
            {
                DisplayService.Error("Unable to resolve provider based on connection string, please specify the 'provider'");
                DisplayService.Error("Supported providers: mssql, postgres, sqlite, oracle, mysql, firebird");
                if (providers.Count > 1)
                {
                    DisplayService.Error($"Potential providers: '{string.Join(", ", providers)}'");
                }

                Environment.Exit(1);
            }
        }
    }

    private static void DisplayHeader(ScaffoldOptions options)
    {
        DisplayService.Title("EF Core Power Tools");
        DisplayService.MarkupLine(
            $"EF Core Power Tools CLI {PackageService.CurrentPackageVersion()} for EF Core {Constants.Version}",
            Color.Cyan1);
        DisplayService.MarkupLine("https://github.com/ErikEJ/EFCorePowerTools", Color.Blue, DisplayService.Link);
        DisplayService.MarkupLine();
        DisplayService.MarkupLine(
            () => DisplayService.Markup("config file:", Color.Green),
            () => DisplayService.Markup(options.ConfigFile.FullName, Decoration.Bold));
        DisplayService.MarkupLine();
    }

    private static Task<int> DisplayHelpAsync(ParserResult<ScaffoldOptions> parserResult)
    {
        Console.WriteLine(HelpText.AutoBuild(parserResult, h =>
        {
            h.AddPostOptionsLine("SAMPLES:");
            h.AddPostOptionsLine(@"  efcpt ""Data Source=.;Initial Catalog=Chinook;User=sa;Pwd=123""");
            h.AddPostOptionsLine(@"  efcpt ""/temp/mydb.dacpac"" mssql");
            h.AddPostOptionsLine(@"  efcpt ""Server=my.database.windows.net;Authentication=Active Directory Default;Database=myddb;User Id=user@domain.com;"" mssql");
            return h;
        }));

        return Task.FromResult(1);
    }
}
