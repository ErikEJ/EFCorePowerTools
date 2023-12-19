using System;
using System.IO;
using CommandLine;

namespace ErikEJ.EFCorePowerTools;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
internal sealed class ScaffoldOptions
{
    [Value(
        0,
        MetaName = "connection",
        HelpText = "ADO.NET connection string for the source database (or .dacpac path)",
        Required = true)]
    public string ConnectionString { get; set; }

    [Value(
        1,
        MetaName = "provider",
        HelpText = "Name of EF Core provider, or use an abbreviation (mssql, postgres, sqlite, oracle, mysql, firebird). May not be required, as an attempt is made to resolve the provider from the connection string.")]
    public string Provider { get; set; }

    [Option('o', "output", HelpText = "Root output folder, defaults to current directory")]
    public string Output { get; set; }

    [Option(
        'i',
        "input",
        HelpText = $"Full pathname to the {Constants.ConfigFileName} file, default is '{Constants.ConfigFileName}' in currrent directory")]
    public FileInfo ConfigFile { get; set; }

    public bool IsDacpac => ConnectionString?.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase) ?? false;
}
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
