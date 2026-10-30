using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GOEddie.Dacpac.References;

public static class DacpacConsolidator
{
    public static string Consolidate(string dacpacPath)
        => Consolidate(dacpacPath, null);

    public static string Consolidate(string dacpacPath, Action<string> reportWarning)
    {
        var fileNames = new List<string>();
        var visited = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        GetAllReferences(Path.GetFullPath(dacpacPath), fileNames, visited, reportWarning);

        if (fileNames.Count == 1)
        {
            return dacpacPath;
        }

        var target = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dacpacPath), $"efpt-{Guid.NewGuid()}.dacpac"));
        try
        {
            using var merger = new DacpacMerger(target, fileNames.ToArray());
            merger.Merge();
            return target;
        }
        catch
        {
            File.Delete(target);
            throw;
        }
    }

    internal static ConsolidatedDacpac Open(string dacpacPath, bool mergeDacpacs, Action<string> reportWarning)
        => new ConsolidatedDacpac(dacpacPath, mergeDacpacs ? Consolidate(dacpacPath, reportWarning) : dacpacPath);

    private static void GetAllReferences(string dacpacPath, List<string> fileNames, HashSet<string> visited, Action<string> reportWarning)
    {
        if (!visited.Add(dacpacPath))
        {
            return;
        }

        fileNames.Add(dacpacPath);
        var parser = new HeaderParser(dacpacPath);
        var references = parser.GetCustomData()
            .Where(p => p.Category == "Reference" && p.Type == "SqlSchema")
            .SelectMany(r => r.Items)
            .Where(i => i.Name == "LogicalName")
            .Where(i => !i.Value.Equals("master.dacpac", StringComparison.OrdinalIgnoreCase));

        foreach (var reference in references)
        {
            var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dacpacPath), reference.Value));
            if (!File.Exists(path))
            {
                if (visited.Add(path))
                {
                    reportWarning?.Invoke($"Referenced DACPAC '{path}' (referenced by '{dacpacPath}') was not found. Its objects will not be included in reverse engineering.");
                }

                continue;
            }

            GetAllReferences(path, fileNames, visited, reportWarning);
        }
    }

    internal sealed class ConsolidatedDacpac : IDisposable
    {
        private readonly bool ownsFile;

        internal ConsolidatedDacpac(string originalPath, string path)
        {
            Path = path;
            ownsFile = originalPath != path;
        }

        internal string Path { get; }

        public void Dispose()
        {
            if (ownsFile)
            {
                File.Delete(Path);
            }
        }
    }
}