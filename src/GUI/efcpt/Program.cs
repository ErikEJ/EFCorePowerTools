using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;
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

    public static int Main(string[] args)
    {
        try
        {
            Console.OutputEncoding = Encoding.UTF8;

            var parserResult = new Parser(c => c.HelpWriter = null)
                .ParseArguments<ScaffoldOptions>(args);

            return parserResult
              .MapResult(
                options => RunAndReturnExitCode(options),
                errs => DisplayHelp(parserResult, errs));
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex.Demystify(), ExceptionFormats.ShortenPaths);
            return 1;
        }
    }

    private static int DisplayHelp(ParserResult<ScaffoldOptions> parserResult, IEnumerable<Error> errs)
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

        AnsiConsole.MarkupLine($"[cyan]EF Core Power Tools CLI {version} for EF Core {EfCoreVersion}[/]");
        AnsiConsole.MarkupLine("[blue][link]https://github.com/ErikEJ/EFCorePowerTools[/][/]");
        Console.WriteLine();
        AnsiConsole.MarkupLine($"[green]Using: '{configPath}'[/]");
        Console.WriteLine();

        var dbType = Providers.GetDatabaseTypeFromProvider(options.Provider, options.IsDacpac);

        if (dbType == DatabaseType.Undefined)
        {
            Console.Error.WriteLine($"Unknown provider '{options.Provider}'");
            return 1;
        }

        Console.WriteLine("Getting database objects...");
        var builder = new TableListBuilder((int)dbType, options.ConnectionString, Array.Empty<SchemaInfo>(), false);

        var buildResult = builder.GetTableModels();
        Console.WriteLine($"{buildResult.Count} tables/views");

        var procedures = builder.GetProcedures();
        buildResult.AddRange(procedures);
        if (procedures.Count > 0)
        {
            Console.WriteLine($"{procedures.Count} stored procedures");
        }

        var functions = builder.GetFunctions();
        buildResult.AddRange(functions);
        if (functions.Count > 0)
        {
            Console.WriteLine($"{functions.Count} functions");
        }

        if (EfcptConfigMapper.TryGetEfcptConfig(configPath, options.ConnectionString, dbType, buildResult, out EfcptConfig config))
        {
            var commandOptions = EfcptConfigMapper.ToOptions(config, options.ConnectionString, options.Provider, Directory.GetCurrentDirectory(), options.IsDacpac);

            Console.WriteLine();

            Console.WriteLine("Generating EF Core DbContext and entity classes...");

            if (commandOptions.UseT4 && EfCoreVersion > 6)
            {
                var t4Result = new T4Helper().DropT4Templates(commandOptions.ProjectPath);
                if (!string.IsNullOrEmpty(t4Result))
                {
                    AnsiConsole.WriteLine(t4Result);
                }
            }

            Stopwatch sw = Stopwatch.StartNew();

            var result = ReverseEngineerRunner.GenerateFiles(commandOptions);

            sw.Stop();

            var paths = new List<string> { Path.GetDirectoryName(result.ContextFilePath) };

            paths = paths.Concat(result.ContextConfigurationFilePaths.Select(p => Path.GetDirectoryName(p)).Distinct()).ToList();

            paths = paths.Concat(result.EntityTypeFilePaths.Select(p => Path.GetDirectoryName(p)).Distinct()).ToList();

            Console.WriteLine($"{result.EntityTypeFilePaths.Count + 1} files generated in {(int)sw.Elapsed.TotalSeconds} seconds");

            Console.WriteLine();

            foreach (var path in paths.Distinct())
            {
                Console.WriteLine($"output: {path}");
            }

            Console.WriteLine();

            foreach (var error in result.EntityErrors)
            {
                Console.WriteLine($"error: {error}");
            }

            foreach (var warning in result.EntityWarnings)
            {
                Console.WriteLine($"warning: {warning}");
            }

            var readmePath = Providers.CreateReadme(options.Provider, commandOptions, EfCoreVersion);

            var fileUri = new Uri(new Uri("file://"), readmePath);

            AnsiConsole.MarkupLine("[cyan]Thank you for using EF Core Power Tools, please open the readme file for next steps:[/]");

            AnsiConsole.MarkupLine($"[blue][link]{fileUri}[/][/]");

            Console.WriteLine();

            return 0;
        }

        return 1;
    }

    private sealed class ScaffoldOptions
    {
        [Value(0, MetaName = "connection", HelpText = "ADO.NET connection string for the source database (or .dacpac path)", Required = true)]
        public string ConnectionString { get; set; }

        [Value(1, MetaName = "provider", HelpText = "Name of EF Core provider, or use an abbreviation (mssql, postgres, sqlite, oracle, mysql)", Required = true)]
        public string Provider { get; set; }

        [Option('o', "output", HelpText = "Root output folder, defaults to current directory")]
        public string Output { get; set; }

        [Option('i', "input", HelpText = $"Full pathname to the {Program.ConfigName} file, default is '{Program.ConfigName}' in currrent directory")]
        public FileInfo ConfigFile { get; set; }

        public bool IsDacpac => ConnectionString?.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
