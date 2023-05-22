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
        var parserResult = new Parser(c => c.HelpWriter = null)
            .ParseArguments<ScaffoldOptions>(args);

        var displayService = new DisplayService();

        var result = parserResult
            .MapResult(
                async options =>
                {
                    await ResolveProviderAsync(options, displayService).ConfigureAwait(false);

                    var fileSystem = new FileSystem();

                    options.ConfigFile = options.ConfigFile ?? new FileInfo(fileSystem.Path.GetFullPath(Constants.ConfigFileName));

                    DisplayHeader(options, displayService);
                    var hostBuilder = new HostBuilder();
                    await hostBuilder.Configure()
                        .RegisterServices(displayService, fileSystem, options)
                        .StartAsync()
                        .ConfigureAwait(false);
                    return Environment.ExitCode;
                },
                async _ => await DisplayHelpAsync(parserResult).ConfigureAwait(false));

        var packageService = new PackageService(displayService);
        await packageService.CheckForPackageUpdateAsync().ConfigureAwait(false);

        return await result.ConfigureAwait(false);
    }

    private static async Task ResolveProviderAsync(ScaffoldOptions options, DisplayService displayService)
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
                displayService.Error("Unable to resolve provider based on connection string, please specify the 'provider'");
                displayService.Error("Supported providers: mssql, postgres, sqlite, oracle, mysql, firebird");
            }

            Environment.Exit(1);
        }
    }

    private static void DisplayHeader(ScaffoldOptions options, DisplayService displayService)
    {
        displayService.Title("EF Core Power Tools");
        displayService.MarkupLine(
            $"EF Core Power Tools CLI {PackageService.CurrentPackageVersion()} for EF Core {Constants.EfCoreVersion}",
            Color.Cyan1);
        displayService.MarkupLine("https://github.com/ErikEJ/EFCorePowerTools", Color.Blue, DisplayService.Link);
        displayService.MarkupLine();
        displayService.MarkupLine(
            () => displayService.Markup("config file:", Color.Green),
            () => displayService.Markup(options.ConfigFile.FullName, Decoration.Bold));
        displayService.MarkupLine();
    }

    private static Task<int> DisplayHelpAsync(ParserResult<ScaffoldOptions> parserResult)
    {
        Console.WriteLine(HelpText.AutoBuild(parserResult, h =>
        {
            h.AddPostOptionsLine("SAMPLES:");
            h.AddPostOptionsLine(@"  efcpt ""Server=(local);Database=Northwind;User=sa;Pwd=123;Encrypt=false"" mssql");
            h.AddPostOptionsLine(@"  efcpt ""Server=my.database.windows.net;Authentication=Active Directory Default;Database=myddb;User Id=user@domain.com;"" mssql");
            h.AddPostOptionsLine(@"  efcpt ""/temp/mydb.dacpac"" Microsoft.EntityFrameworkCore.SqlServer");
            return h;
        }));

        return Task.FromResult(1);
    }
}
