using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using RevEng.Common;
using RevEng.Common.Efcpt;
using RevEng.Core;
using Spectre.Console;

namespace ErikEJ.EfCorePowerTools;

public static class Program
{
    public const string ConfigName = "efcpt-config.json";

#if CORE60
    public const int EfCoreVersion = 6;
#else
    public const int EfCoreVersion = 7;
#endif

    public static async Task<int> Main(string[] args)
    {
        try
        {
            var parserResult = new Parser(c => c.HelpWriter = null)
                .ParseArguments<ScaffoldOptions>(args);

            var result = parserResult
              .MapResult(
                options => RunAndReturnExitCode(options),
                errs => DisplayHelp(parserResult));

            await CheckForPackageUpdateAsync();

            return result;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex.Demystify(), ExceptionFormats.ShortenPaths);
            return 1;
        }
    }

    private static int DisplayHelp(ParserResult<ScaffoldOptions> parserResult)
    {
        Console.WriteLine(HelpText.AutoBuild(parserResult, h =>
        {
            h.AddPostOptionsLine("SAMPLES:");
            h.AddPostOptionsLine(@"  efcpt ""Server=(local);Database=Northwind;User=sa;Pwd=123;Encrypt=false"" mssql");
            h.AddPostOptionsLine(@"  efcpt ""/temp/mydb.dacpac"" Microsoft.EntityFrameworkCore.SqlServer");
            return h;
        }));
        return 1;
    }

    private static int RunAndReturnExitCode(ScaffoldOptions options)
    {
        var configPath = options.ConfigFile?.FullName ?? Path.GetFullPath(ConfigName);
        var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        AnsiConsole.Write(
            new FigletText("EF Core Power Tools")
                .Centered()
                .Color(Color.Aqua));

        AnsiConsole.MarkupLine($"[cyan]EF Core Power Tools CLI {version} (preview) for EF Core {EfCoreVersion}[/]");
        AnsiConsole.MarkupLine("[blue][link]https://github.com/ErikEJ/EFCorePowerTools[/][/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[green]Using: '{configPath}'[/]");
        AnsiConsole.WriteLine();

        var dbType = Providers.GetDatabaseTypeFromProvider(options.Provider, options.IsDacpac);

        if (dbType == DatabaseType.Undefined)
        {
            Console.Error.WriteLine($"Unknown provider '{options.Provider}'");
            return 1;
        }

        var builder = new TableListBuilder((int)dbType, options.ConnectionString, Array.Empty<SchemaInfo>(), false);
        var buildResult = new List<TableModel>();

        AnsiConsole.Status()
            .Start("Getting database objects...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Ascii);
                ctx.SpinnerStyle(Style.Parse("green"));
                buildResult = builder.GetTableModels();
            });

        AnsiConsole.WriteLine($"{buildResult.Count} tables/views found");

        var procedures = builder.GetProcedures();
        buildResult.AddRange(procedures);
        if (procedures.Count > 0)
        {
            AnsiConsole.WriteLine($"{procedures.Count} stored procedures found");
        }

        var functions = builder.GetFunctions();
        buildResult.AddRange(functions);
        if (functions.Count > 0)
        {
            AnsiConsole.WriteLine($"{functions.Count} functions found");
        }

        if (EfcptConfigMapper.TryGetEfcptConfig(configPath, options.ConnectionString, dbType, buildResult, out EfcptConfig config))
        {
            var commandOptions = EfcptConfigMapper.ToOptions(config, options.ConnectionString, options.Provider, Directory.GetCurrentDirectory(), options.IsDacpac, configPath);

            AnsiConsole.WriteLine();

            if (commandOptions.UseT4 && EfCoreVersion > 6)
            {
                var t4Result = new T4Helper().DropT4Templates(commandOptions.ProjectPath);
                if (!string.IsNullOrEmpty(t4Result))
                {
                    AnsiConsole.WriteLine(t4Result);
                }
            }

            var result = new ReverseEngineerResult();

            Stopwatch sw = Stopwatch.StartNew();

            AnsiConsole.Status()
                .Start("Generating EF Core DbContext and entity classes...", ctx =>
                {
                    ctx.Spinner(Spinner.Known.Ascii);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    result = ReverseEngineerRunner.GenerateFiles(commandOptions);
                });

            sw.Stop();

            var paths = new List<string> { Path.GetDirectoryName(result.ContextFilePath) };

            paths = paths.Concat(result.ContextConfigurationFilePaths.Select(p => Path.GetDirectoryName(p)).Distinct()).ToList();

            paths = paths.Concat(result.EntityTypeFilePaths.Select(p => Path.GetDirectoryName(p)).Distinct()).ToList();

            AnsiConsole.WriteLine($"{result.EntityTypeFilePaths.Count + result.ContextConfigurationFilePaths.Count + 1} files generated in {sw.Elapsed.TotalSeconds:0.0} seconds");

            AnsiConsole.WriteLine();

            foreach (var path in paths.Distinct())
            {
                AnsiConsole.WriteLine($"output: {path}");
            }

            AnsiConsole.WriteLine();

            foreach (var error in result.EntityErrors)
            {
                AnsiConsole.MarkupLine($"[red]error:[/] {error}");
            }

            foreach (var warning in result.EntityWarnings)
            {
                AnsiConsole.MarkupLine($"[yellow]warning:[/] {warning}");
            }

            var readmePath = Providers.CreateReadme(options.Provider, commandOptions, EfCoreVersion);

            var fileUri = new Uri(new Uri("file://"), readmePath);

            AnsiConsole.MarkupLine("[cyan]Thank you for using EF Core Power Tools, please open the readme file for next steps:[/]");

            AnsiConsole.MarkupLine($"[blue][link]{fileUri}[/][/]");

            AnsiConsole.WriteLine();

            return 0;
        }

        return 1;
    }

    private static async Task CheckForPackageUpdateAsync()
    {
        try
        {
            var logger = new NuGet.Common.NullLogger();
            var cancellationToken = CancellationToken.None;

            using var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            var versions = await resource.GetAllVersionsAsync(
                "ErikEJ.EFCorePowerTools.CLI",
                cache,
                logger,
                cancellationToken);

            var latestVersion = versions.Where(v => v.Major == EfCoreVersion).OrderByDescending(v => v).FirstOrDefault();

            if (latestVersion > CurrentPackageVersion())
            {
                AnsiConsole.Markup("[yellow]Your are not using the latest version of the tool, please update to get the latest bug fixes, features and support.[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.Markup($"[yellow]Latest version is '{latestVersion}'.[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.Markup($"[yellow]Run 'dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version {EfCoreVersion}.0.*-*' to get the latest version.[/]");
            }
        }
        catch (Exception)
        {
            // Ignore
        }
    }

    private static NuGetVersion CurrentPackageVersion()
    {
        return new NuGetVersion("7.0.0-preview1");

        //return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }

    private sealed class ScaffoldOptions
    {
        [Value(0, MetaName = "connection", HelpText = "ADO.NET connection string for the source database (or .dacpac path)", Required = true)]
        public string ConnectionString { get; set; }

        [Value(1, MetaName = "provider", HelpText = "Name of EF Core provider, or use an abbreviation (mssql, postgres, sqlite, oracle, mysql)", Required = true)]
        public string Provider { get; set; }

        [Option('o', "output", HelpText = "Root output folder, defaults to current directory")]
        public string Output { get; set; }

        [Option('i', "input", HelpText = $"Full pathname to the {ConfigName} file, default is '{ConfigName}' in currrent directory")]
        public FileInfo ConfigFile { get; set; }

        public bool IsDacpac => ConnectionString?.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
