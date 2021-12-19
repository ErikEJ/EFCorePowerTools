using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GOEddie.Dacpac.References
{
    public class DacpacConsolidator
    {
        public string Consolidate(string dacpacPath)
        {
            var fileNames = GetAllReferences(dacpacPath, true);

            if (fileNames.Count == 0)
            {
                return dacpacPath;
            }

            fileNames.Insert(0, dacpacPath);

            var target = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dacpacPath), $"efpt-{Guid.NewGuid()}.dacpac"));
            var merger = new DacpacMerger(target, fileNames.ToArray());
            merger.Merge();

            return target;
        }

        private List<string> GetAllReferences(string dacpacPath, bool isRootDacpac)
        {
            if (!isRootDacpac && !File.Exists(dacpacPath))
            {
                // When the root DACPAC has a reference with SuppressMissingDependenciesErrors = true,
                // second-level references of that first-level reference don't have to be referenced from the root project.
                // For that case, DACPACs not referenced directly by the root DACPAC are optional.
                return Enumerable.Empty<string>().ToList();
            }

            var parser = new HeaderParser(dacpacPath);

            var references = parser.GetCustomData()
                .Where(p => p.Category == "Reference" && p.Type == "SqlSchema")
                .ToList();

            if (references.Count == 0)
            {
                return Enumerable.Empty<string>().ToList();
            }

            var fileNames = references
                .SelectMany(r => r.Items)
                .Where(i => i.Name == "LogicalName")
                .Where(i => !i.Value.Equals("master.dacpac", StringComparison.OrdinalIgnoreCase))
                .Select(i => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dacpacPath), i.Value)))
                .ToList();

            if (!isRootDacpac)
            {
                // If we're looking for references of a non-root DACPAC,
                // any reference is optional (see SuppressMissingDependenciesErrors = true).
                // Therefore this DACPACs won't be included, if they don't exist in the build output directory.
                fileNames = fileNames.Where(File.Exists).ToList();
            }

            if (fileNames.Count == 0)
            {
                return Enumerable.Empty<string>().ToList();
            }

            var additionalFiles = new List<string>();
            foreach (var fileName in fileNames)
            {
                additionalFiles.AddRange(GetAllReferences(fileName, false));
            }

            fileNames.AddRange(additionalFiles);

            return fileNames.Distinct().ToList();
        }
    }
}
