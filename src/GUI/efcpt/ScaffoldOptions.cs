using System;
using System.IO;
using CommandLine;

namespace ErikEJ.EfCorePowerTools;

public static partial class Program
{
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
