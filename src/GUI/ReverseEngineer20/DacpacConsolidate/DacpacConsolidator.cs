using GOEddie.Dacpac.References;
using System;
using System.IO;
using System.Linq;

namespace ReverseEngineer20.DacpacConsolidate
{
    public class DacpacConsolidator
    {
        public string Consolidate(string dacpacPath)
        {
            var parser = new HeaderParser(dacpacPath);

            var references = parser.GetCustomData()
                .Where(p => p.Category == "Reference" && p.Type == "SqlSchema")
                .ToList();

            if (references.Count == 0)
            {
                return dacpacPath;
            }

            //TODO Resolve nested references!


            var fileNames = references
                .SelectMany(r => r.Items)
                .Where(i => i.Name == "FileName")
                .Select(i => i.Value)
                .ToList();

            if (fileNames.Count() == 0)
            {
                return dacpacPath;
            }
            fileNames.Insert(0, dacpacPath);

            var target = Path.Combine(Path.GetDirectoryName(dacpacPath), $"efpt-{Guid.NewGuid()}.dacpac");
            var merger = new DacpacMerger(target, fileNames.ToArray());
            merger.Merge();

            return target;
        }
    }
}
