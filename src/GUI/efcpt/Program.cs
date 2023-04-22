using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CommandLine;
using Efcpt;
using RevEng.Common;
using RevEng.Common.Efcpt;
using RevEng.Core;

namespace ErikEJ.EfCorePowerTools;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            Console.OutputEncoding = Encoding.UTF8;

            return Parser.Default.ParseArguments<ScaffoldOptions>(args)
              .MapResult(
                options => RunAndReturnExitCode(options),
                _ => 1);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Demystify().ToString());
            return 1;
        }
    }

    private static int RunAndReturnExitCode(ScaffoldOptions options)
    {
        options.Dump();

        // TODO test .dacpac!
        // TODO Validate options
        // TODO add progress messages - Spectre.Console ?

        var dbType = Providers.GetDatabaseTypeFromProvider(options.Provider, options.IsDacpac);

        if (dbType == DatabaseType.Undefined)
        {
            // TODO Show error
            return 1;
        }

        var builder = new TableListBuilder((int)dbType, options.ConnectionString, Array.Empty<SchemaInfo>(), false);

        var buildResult = builder.GetTableModels();

        buildResult.AddRange(builder.GetProcedures());

        buildResult.AddRange(builder.GetFunctions());

        var configPath = options.ConfigFile?.FullName ?? "efcpt-config.json";

        if (EfcptConfigMapper.TryGetEfcptConfig(configPath, buildResult, out EfcptConfig config))
        {
            var fullPath = Path.GetFullPath(configPath);

            var commandOptions = EfcptConfigMapper.ToOptions(config, options.ConnectionString, options.Provider, Path.GetDirectoryName(fullPath));

            var result = ReverseEngineerRunner.GenerateFiles(commandOptions);

            // TODO report result
            return 0;
        }

        return 1;
    }
}
