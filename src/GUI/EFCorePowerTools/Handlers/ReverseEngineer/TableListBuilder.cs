using RevEng.Shared;
using System;
using System.Collections.Generic;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class TableListBuilder
    {
        private readonly string _connectionString;
        private readonly DatabaseType _databaseType;
        private readonly SchemaInfo[] _schemas;

        public TableListBuilder(string connectionString, DatabaseType databaseType, SchemaInfo[] schemas)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
            _databaseType = databaseType;
            _schemas = schemas;
        }

        public List<TableModel> GetTableDefinitions(CodeGenerationMode codeGenerationMode)
        {
            var launcher = new EfRevEngLauncher(null, codeGenerationMode);

            return launcher.GetTables(_connectionString, _databaseType, _schemas);
        }
    }
}
