using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CommandLine;
using CommandLine.Text;
using Efcpt;
using RevEng.Common;
using RevEng.Common.Efcpt;
using RevEng.Core;
using Spectre.Console;

namespace ErikEJ.EfCorePowerTools;

public static class Program
{
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
            Console.Error.WriteLine(ex.Demystify().ToString());
            return 1;
        }
    }

    private static int DisplayHelp(ParserResult<ScaffoldOptions> parserResult, IEnumerable<Error> errs)
    {
        Console.WriteLine(HelpText.AutoBuild(parserResult, h =>
        {
            h.AddPostOptionsLine("SAMPLES:");
            h.AddPostOptionsLine(@"efcpt ""Server=(local);Database=Northwind;User=sa;Pwd=123;Encrypt=false"" mssql");
            h.AddPostOptionsLine(@"efcpt ""/temp/mydb.dacpac"" mssql");
            return h;
        }));
        return 1;
    }

    private static int RunAndReturnExitCode(ScaffoldOptions options)
    {
        Console.WriteLine();
        AnsiConsole.MarkupLine("[bold yellow]EF Core Power Tools CLI[/]");
        AnsiConsole.MarkupLine("[link]https://github.com/ErikEJ/EFCorePowerTools[/]");
        Console.WriteLine();

        // TODO test .dacpac! test root namespace
        var dbType = Providers.GetDatabaseTypeFromProvider(options.Provider, options.IsDacpac);

        if (dbType == DatabaseType.Undefined)
        {
            Console.Error.WriteLine($"Unknown provider '{options.Provider}'");
            return 1;
        }

        Console.WriteLine("Getting database objects...");

        var builder = new TableListBuilder((int)dbType, options.ConnectionString, Array.Empty<SchemaInfo>(), false);

        var buildResult = builder.GetTableModels();

        Console.WriteLine($"Found {buildResult.Count} tables and views");

        var procedures = builder.GetProcedures();
        buildResult.AddRange(procedures);
        if (procedures.Count > 0)
        {
            Console.WriteLine($"Found {procedures.Count} stored procedures");
        }

        var functions = builder.GetFunctions();
        buildResult.AddRange(functions);
        if (functions.Count > 0)
        {
            Console.WriteLine($"Found {functions.Count} functions");
        }

        var configPath = options.ConfigFile?.FullName ?? Path.GetFullPath("efcpt-config.json");

        if (EfcptConfigMapper.TryGetEfcptConfig(configPath, options.ConnectionString, dbType, buildResult, out EfcptConfig config))
        {
            Console.WriteLine($"Using config file: '{configPath}'");

            var commandOptions = EfcptConfigMapper.ToOptions(config, options.ConnectionString, options.Provider, Path.GetDirectoryName(configPath), options.IsDacpac);

            Console.WriteLine("Generating code...");

            Stopwatch sw = Stopwatch.StartNew();

            var result = ReverseEngineerRunner.GenerateFiles(commandOptions);

            Console.WriteLine($"Generated {result.EntityTypeFilePaths.Count + 1} files in {(int)sw.Elapsed.TotalSeconds} seconds");

            Console.WriteLine();

            foreach (var error in result.EntityErrors)
            {
                Console.WriteLine($"error: {error}");
            }

            foreach (var warning in result.EntityWarnings)
            {
                Console.WriteLine($"warning: {warning}");
            }

            return 0;
        }

        return 1;
    }
}
