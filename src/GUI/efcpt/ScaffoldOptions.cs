using System;
using System.IO;
using CommandLine;

namespace Efcpt
{
    public class ScaffoldOptions
    {
        [Value(0, MetaName = "connection", HelpText = "ADO.NET connection string for the source database (or .dacpac path)", Required = true)]
        public string ConnectionString { get; set; }

        [Value(1, MetaName = "provider", HelpText = "Name of EF Core provider, or use an abbreviation (mssql, postgres, sqlite, oracle, mysql)", Required = true)]
        public string Provider { get; set; }

        [Option('o', "output", HelpText = "Root output folder, defaults to current directory")]
        public string Output { get; set; }

        [Option('i', "input", HelpText = "Full pathname to the efcpt-config.json file, default is 'efcpt-config.json' in currrent directory")]
        public FileInfo ConfigFile { get; set; }

        public bool IsDacpac => ConnectionString?.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase) ?? false;

        internal void Dump()
        {
            Console.WriteLine($"ConnectionString: {ConnectionString}");
            Console.WriteLine($"Provider: {Provider}");
        }
    }
}
