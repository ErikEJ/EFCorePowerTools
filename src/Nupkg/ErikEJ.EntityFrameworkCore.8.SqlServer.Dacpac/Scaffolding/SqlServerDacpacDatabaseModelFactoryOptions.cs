using System.Collections.Generic;

namespace ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding
{
    public class SqlServerDacpacDatabaseModelFactoryOptions
    {
        public bool MergeDacpacs { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public Dictionary<string, IEnumerable<string>> ExcludedIndexes { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}