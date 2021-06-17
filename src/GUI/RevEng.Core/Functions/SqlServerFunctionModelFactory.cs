using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System.Collections.Generic;
using System.Data;

namespace RevEng.Core.Procedures
{
    public class SqlServerFunctionModelFactory : SqlServerModuleModelFactory, IFunctionModelFactory
    {
        public SqlServerFunctionModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
            : base(logger)
        {
        }

        public ModuleModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetRoutines(connectionString, options, "function");
        }

        protected override List<ModuleResultElement> GetResultElements(SqlConnection connection, string schema, string name)// int objectId)
        {
            var dtResult = new DataTable();
            var result = new List<ModuleResultElement>();

            var sql = $@"
SELECT 
    c.name,
    COALESCE(type_name(c.system_type_id), type_name(c.user_type_id)) AS type_name,
    c.column_id AS column_ordinal,
    c.is_nullable
FROM sys.columns c
WHERE object_id = OBJECT_ID('{schema}.{name}');";

            var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };

            adapter.Fill(dtResult);

            int rCounter = 0;

            foreach (DataRow res in dtResult.Rows)
            {
                var parameter = new ModuleResultElement()
                {
                    Name = string.IsNullOrEmpty(res["name"].ToString()) ? $"Col{rCounter}" : res["name"].ToString(),
                    StoreType = res["type_name"].ToString(),
                    Ordinal = int.Parse(res["column_ordinal"].ToString()),
                    Nullable = (bool)res["is_nullable"],
                };

                result.Add(parameter);

                rCounter++;
            }

            return result;
        }
    }
}
