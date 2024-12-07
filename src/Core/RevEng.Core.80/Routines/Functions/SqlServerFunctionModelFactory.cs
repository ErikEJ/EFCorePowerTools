using Microsoft.Data.SqlClient;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace RevEng.Core.Routines.Functions
{
    public class SqlServerFunctionModelFactory : SqlServerRoutineModelFactory, IFunctionModelFactory
    {
        public SqlServerFunctionModelFactory()
        {
            RoutineType = "FUNCTION";

            RoutineSql = $@"
SELECT
    ROUTINE_SCHEMA,
    ROUTINE_NAME,
    CAST(CASE WHEN (DATA_TYPE != 'TABLE') THEN 1 ELSE 0 END AS bit) AS IS_SCALAR
FROM INFORMATION_SCHEMA.ROUTINES
WHERE NULLIF(ROUTINE_NAME, '') IS NOT NULL
AND OBJECTPROPERTY(OBJECT_ID(QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME)), 'IsMSShipped') = 0
AND object_id(QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME)) NOT IN (SELECT [ep].[major_id]
        FROM [sys].[extended_properties] AS [ep]
        WHERE [ep].[minor_id] = 0
            AND [ep].[class] = 1
            AND [ep].[name] = N'microsoft_database_tools_support'
    )
AND ROUTINE_TYPE = N'FUNCTION' 
ORDER BY ROUTINE_NAME;";
        }

        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetRoutines(connectionString, options);
        }

        protected override List<List<ModuleResultElement>> GetResultElementLists(SqlConnection connection, Routine module, bool multipleResults, bool useLegacyResultSetDiscovery)
        {
            ArgumentNullException.ThrowIfNull(module);

            using var dtResult = new DataTable();
            var list = new List<ModuleResultElement>();

            var sql = $@"
SELECT 
    c.name,
    COALESCE(ts.name, tu.name) AS type_name,
    c.column_id AS column_ordinal,
    c.is_nullable,
    c.max_length,
    c.precision,
    c.scale
FROM sys.columns c
inner join sys.types tu ON c.user_type_id = tu.user_type_id 
inner join sys.objects AS o on o.object_id = c.object_id
inner JOIN sys.schemas AS s ON o.schema_id = s.schema_id
LEFT JOIN sys.types ts ON tu.system_type_id = ts.user_type_id
where o.name = '{module.Name}' and s.name = '{module.Schema}';";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            using var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection),
            };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

            adapter.Fill(dtResult);

            var rCounter = 0;

            foreach (DataRow res in dtResult.Rows)
            {
                if (res != null)
                {
                    var storeType = res["type_name"].ToString();
                    var maxLength = res["max_length"] == DBNull.Value ? 0 : int.Parse(res["max_length"].ToString()!, CultureInfo.InvariantCulture);

                    if (storeType != null
                        && storeType.StartsWith("nvarchar", StringComparison.OrdinalIgnoreCase)
                        && maxLength > 0)
                    {
                        maxLength = maxLength / 2;
                    }

                    var parameter = new ModuleResultElement()
                    {
                        Name = string.IsNullOrEmpty(res["name"].ToString()) ? $"Col{rCounter}" : res["name"].ToString(),
                        StoreType = storeType,
                        Ordinal = int.Parse(res["column_ordinal"].ToString()!, CultureInfo.InvariantCulture),
                        Nullable = (bool)res["is_nullable"],
                        MaxLength = maxLength,
                        Precision = (byte)res["precision"],
                        Scale = (byte)res["scale"],
                    };

                    list.Add(parameter);
                }

                rCounter++;
            }

            var result = new List<List<ModuleResultElement>>
            {
                list,
            };

            return result;
        }
    }
}
