using Microsoft.SqlServer.Dac.Extensions.Prototype;
using Microsoft.SqlServer.Dac.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReverseEngineer20
{
    public class DacpacTableListBuilder
    {
        private readonly string _dacpacPath;

        public DacpacTableListBuilder(string dacpacPath)
        {
            if (string.IsNullOrEmpty(dacpacPath))
            {
                throw new ArgumentException(@"invalid path", nameof(dacpacPath));
            }
            if (!File.Exists(dacpacPath))
            {
                throw new ArgumentException("Dacpac file not found");
            }
            _dacpacPath = dacpacPath;
        }

        public List<string> GetTableNames()
        {
            var names = new List<string>();
            var model = new TSqlTypedModel(_dacpacPath);

            var tables = model.GetObjects<TSqlTable>(DacQueryScopes.UserDefined)
                .Where(t => t.PrimaryKeyConstraints.Count() > 0)
                .ToList();

            foreach (var table in tables)
            {
                names.Add($"{table.Name.Parts[0]}.{table.Name.Parts[1]}");
            }
            return names;
        }
    }
}
