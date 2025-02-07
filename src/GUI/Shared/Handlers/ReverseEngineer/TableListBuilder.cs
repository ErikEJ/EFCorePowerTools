using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevEng.Common;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class TableListBuilder
    {
        private readonly string connectionString;
        private readonly DatabaseType databaseType;
        private readonly SchemaInfo[] schemas;

        public TableListBuilder(string connectionString, DatabaseType databaseType, SchemaInfo[] schemas)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.connectionString = connectionString;
            this.databaseType = databaseType;
            this.schemas = schemas;
        }

        public async Task<List<TableModel>> GetTableDefinitionsAsync(CodeGenerationMode codeGenerationMode)
        {
            var launcher = new EfRevEngLauncher(null, codeGenerationMode);

            return await launcher.GetTablesAsync(connectionString, databaseType, schemas, AdvancedOptions.Instance.MergeDacpacs);
        }
    }
}