using Microsoft.Data.SqlClient;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace RevEng.Core.Procedures
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
AND (
            select 
                major_id 
            from 
                sys.extended_properties 
            where 
                major_id = object_id(QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME)) and 
                minor_id = 0 and 
                class = 1 and 
                name = N'microsoft_database_tools_support'
        ) IS NULL 
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
    COALESCE(type_name(c.system_type_id), type_name(c.user_type_id)) AS type_name,
    c.column_id AS column_ordinal,
    c.is_nullable
FROM sys.columns c
WHERE object_id = OBJECT_ID('{module.Schema}.{module.Name}');";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            using var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection),
            };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

            adapter.Fill(dtResult);

            int rCounter = 0;

            foreach (DataRow res in dtResult.Rows)
            {
                if (res != null)
                {
                    var parameter = new ModuleResultElement()
                    {
                        Name = string.IsNullOrEmpty(res["name"].ToString()) ? $"Col{rCounter}" : res["name"].ToString(),
                        StoreType = res["type_name"].ToString(),
                        Ordinal = int.Parse(res["column_ordinal"].ToString()!, CultureInfo.InvariantCulture),
                        Nullable = (bool)res["is_nullable"],
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
