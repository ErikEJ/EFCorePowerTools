﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.Data.SqlClient;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines.Procedures
{
    public class SqlServerStoredProcedureModelFactory : SqlServerRoutineModelFactory, IProcedureModelFactory
    {
        public SqlServerStoredProcedureModelFactory()
        {
            RoutineType = "PROCEDURE";

            RoutineSql = $@"
SELECT
    ROUTINE_SCHEMA,
    ROUTINE_NAME,
    CAST(0 AS bit) AS IS_SCALAR
FROM INFORMATION_SCHEMA.ROUTINES
WHERE NULLIF(ROUTINE_NAME, '') IS NOT NULL
AND OBJECTPROPERTY(OBJECT_ID(QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME)), 'IsMSShipped') = 0
AND object_id(QUOTENAME(ROUTINE_SCHEMA) + '.' + QUOTENAME(ROUTINE_NAME)) NOT IN (SELECT [ep].[major_id]
        FROM [sys].[extended_properties] AS [ep]
        WHERE [ep].[minor_id] = 0
            AND [ep].[class] = 1
            AND [ep].[name] = N'microsoft_database_tools_support'
    )
AND ROUTINE_TYPE = N'PROCEDURE' 
ORDER BY ROUTINE_NAME;";
        }

        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetRoutines(connectionString, options);
        }

        protected override List<List<ModuleResultElement>> GetResultElementLists(SqlConnection connection, Routine module, bool multipleResults, bool useLegacyResultSetDiscovery)
        {
            ArgumentNullException.ThrowIfNull(connection);

            ArgumentNullException.ThrowIfNull(module);

            if (useLegacyResultSetDiscovery)
            {
                return GetFirstResultSet(connection, module);
            }

            return GetAllResultSets(connection, module, !multipleResults);
        }

        private static List<List<ModuleResultElement>> GetAllResultSets(SqlConnection connection, Routine module, bool singleResult)
        {
            var result = new List<List<ModuleResultElement>>();
            using var sqlCommand = connection.CreateCommand();

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            sqlCommand.CommandText = $"[{module.Schema}].[{module.Name}]";
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var parameters = module.Parameters.Take(module.Parameters.Count - 1);

            foreach (var parameter in parameters)
            {
                var param = new SqlParameter("@" + parameter.Name, DBNull.Value);

                if (parameter.ClrTypeFromSqlParameter() == typeof(DataTable))
                {
                    param.Value = GetDataTableFromSchema(parameter, connection);
                    param.SqlDbType = SqlDbType.Structured;
                }

                if (parameter.ClrTypeFromSqlParameter() == typeof(byte[]))
                {
                    param.SqlDbType = SqlDbType.VarBinary;
                }

                sqlCommand.Parameters.Add(param);
            }

            using var schemaReader = sqlCommand.ExecuteReader(CommandBehavior.SchemaOnly);
            do
            {
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.datatablereader.getschematable
                var schemaTable = schemaReader.GetSchemaTable();
                var list = new List<ModuleResultElement>();

                if (schemaTable == null)
                {
                    break;
                }

                var unnamedColumnCount = 0;

                foreach (DataRow row in schemaTable.Rows)
                {
                    if (row != null)
                    {
                        var name = row["ColumnName"].ToString();
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            unnamedColumnCount++;
                            continue;
                        }

                        var storeType = row["DataTypeName"].ToString();

                        if (row["ProviderSpecificDataType"]?.ToString()?.StartsWith("Microsoft.SqlServer.Types.Sql", StringComparison.OrdinalIgnoreCase) ?? false)
                        {
#pragma warning disable CA1308 // Normalize strings to uppercase
                            storeType = row["ProviderSpecificDataType"].ToString()?.Replace("Microsoft.SqlServer.Types.Sql", string.Empty, StringComparison.OrdinalIgnoreCase).ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
                        }

                        list.Add(new ModuleResultElement
                        {
                            Name = name,
                            Nullable = (bool?)row["AllowDBNull"] ?? true,
                            Ordinal = (int)row["ColumnOrdinal"],
                            StoreType = storeType,
                            Precision = (short?)row["NumericPrecision"],
                            Scale = (short?)row["NumericScale"],
                            MaxLength = (int)row["ColumnSize"],
                        });
                    }
                }

                // If the result set only contains un-named columns
                if (schemaTable.Rows.Count > 0 && schemaTable.Rows.Count == unnamedColumnCount)
                {
                    throw new InvalidOperationException($"Only un-named result columns in procedure");
                }

                if (unnamedColumnCount > 0)
                {
                    module.UnnamedColumnCount += unnamedColumnCount;
                }

                result.Add(list);
            }
            while (schemaReader.NextResult() && !singleResult);

            return result;
        }

        private static List<List<ModuleResultElement>> GetFirstResultSet(SqlConnection connection, Routine module)
        {
            using var dtResult = new DataTable();
            var list = new List<ModuleResultElement>();

            var sql = $"exec dbo.sp_describe_first_result_set N'[{module.Schema}].[{module.Name}]';";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            using var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection),
            };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

            adapter.Fill(dtResult);

            var unnamedColumnCount = 0;

            foreach (DataRow row in dtResult.Rows)
            {
                if (row != null)
                {
                    var name = row["name"].ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        unnamedColumnCount++;
                        continue;
                    }

                    var storeType = string.IsNullOrEmpty(row["system_type_name"].ToString()) ? row["user_type_name"].ToString() : row["system_type_name"].ToString();
                    var maxLength = row["max_length"] == DBNull.Value ? 0 : int.Parse(row["max_length"].ToString()!, CultureInfo.InvariantCulture);

                    if (storeType != null
                        && storeType.StartsWith("nvarchar", StringComparison.OrdinalIgnoreCase)
                        && maxLength > 0)
                    {
                        maxLength = maxLength / 2;
                    }

                    var parameter = new ModuleResultElement()
                    {
                        Name = name,
                        StoreType = storeType,
                        Ordinal = int.Parse(row["column_ordinal"].ToString()!, CultureInfo.InvariantCulture),
                        Nullable = (bool)row["is_nullable"],
                        MaxLength = maxLength,
                        Precision = (byte)row["precision"],
                        Scale = (byte)row["scale"],
                    };

                    list.Add(parameter);
                }
            }

            // If the result set only contains un-named columns
            if (dtResult.Rows.Count == unnamedColumnCount)
            {
                throw new InvalidOperationException($"Only un-named result columns in procedure");
            }

            if (unnamedColumnCount > 0)
            {
                module.UnnamedColumnCount += unnamedColumnCount;
            }

            var result = new List<List<ModuleResultElement>>
            {
                list,
            };

            return result;
        }

        private static DataTable GetDataTableFromSchema(ModuleParameter parameter, SqlConnection connection)
        {
            var userType = new SqlParameter
            {
                Value = parameter.TypeId,
                ParameterName = "@userTypeId",
            };

            var userSchema = new SqlParameter
            {
                Value = parameter.TypeSchema,
                ParameterName = "@schemaId",
            };

            var query = "SELECT SC.name, ST.name AS datatype FROM sys.columns SC " +
                        "INNER JOIN sys.types ST ON ST.system_type_id = SC.system_type_id AND ST.is_user_defined = 0 " +
                        "WHERE ST.name <> 'sysname' AND SC.object_id = " +
                        "(SELECT type_table_object_id FROM sys.table_types WHERE schema_id = @schemaId AND user_type_id =  @userTypeId);";

            var dataTable = new DataTable();

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(userType);
                command.Parameters.Add(userSchema);
                using (var sqlDataReader = command.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        var columnName = sqlDataReader["name"].ToString();
                        var clrType = SqlServerSqlTypeExtensions.GetClrType(sqlDataReader["datatype"].ToString(), false);
                        dataTable.Columns.Add(columnName, clrType);
                    }
                }
            }

            return dataTable;
        }
    }
}