using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
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
                try
                {
                    return GetFirstResultSet(connection, module);
                }
                catch (SqlException ex) when (ShouldTryDefinitionFallback(ex))
                {
                    return GetResultSetsWithMetadataOrDefinitionFallback(
                        () => GetAllResultSets(connection, module, !multipleResults),
                        () => GetResultSetsFromDefinition(connection, module, !multipleResults),
                        fallbackException => fallbackException is SqlException sqlException && ShouldTryDefinitionFallback(sqlException),
                        ex);
                }
            }

            return GetResultSetsWithMetadataOrDefinitionFallback(
                () => GetAllResultSets(connection, module, !multipleResults),
                () => GetResultSetsFromDefinition(connection, module, !multipleResults),
                ex => ex is SqlException sqlException && ShouldTryDefinitionFallback(sqlException));
        }

        private static List<List<ModuleResultElement>> GetAllResultSets(SqlConnection connection, Routine module, bool singleResult)
        {
            var result = new List<List<ModuleResultElement>>();
            using var sqlCommand = CreateStoredProcedureCommand(connection, module);

            using var schemaReader = sqlCommand.ExecuteReader(CommandBehavior.SchemaOnly);
            ReadResultSets(module, singleResult, result, schemaReader);

            return result;
        }

        /// <summary>
        /// Tries SQL Server metadata discovery first and falls back to parsing the stored procedure definition.
        /// </summary>
        private static List<List<ModuleResultElement>> GetResultSetsWithMetadataOrDefinitionFallback(
            Func<List<List<ModuleResultElement>>> getMetadataResultSets,
            Func<List<List<ModuleResultElement>>> getDefinitionResultSets,
            Func<Exception, bool> shouldTryDefinitionFallback,
            Exception originalException = null)
        {
            try
            {
                return getMetadataResultSets();
            }
            catch (Exception ex) when (shouldTryDefinitionFallback(ex))
            {
                var resultFromDefinition = getDefinitionResultSets();
                if (resultFromDefinition.Count > 0)
                {
                    return resultFromDefinition;
                }

                throw originalException ?? ex;
            }
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

        /// <summary>
        /// Creates a schema-only stored procedure command with placeholder parameters for metadata discovery.
        /// </summary>
        private static SqlCommand CreateStoredProcedureCommand(SqlConnection connection, Routine module)
        {
            var sqlCommand = connection.CreateCommand();

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            sqlCommand.CommandText = $"[{module.Schema}].[{module.Name}]";
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            sqlCommand.CommandType = CommandType.StoredProcedure;

            AddStoredProcedureParameters(sqlCommand, module, connection);

            return sqlCommand;
        }

        /// <summary>
        /// Adds placeholder parameters needed to compile the stored procedure and infer its result shape.
        /// </summary>
        private static void AddStoredProcedureParameters(SqlCommand sqlCommand, Routine module, SqlConnection connection)
        {
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
        }

        /// <summary>
        /// Reads one or more result-set schemas from a data reader into <see cref="ModuleResultElement"/> collections.
        /// </summary>
        private static void ReadResultSets(Routine module, bool singleResult, List<List<ModuleResultElement>> result, SqlDataReader schemaReader)
        {
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
                    throw new InvalidOperationException("Only un-named result columns in procedure");
                }

                if (unnamedColumnCount > 0)
                {
                    module.UnnamedColumnCount += unnamedColumnCount;
                }

                result.Add(list);
            }
            while (schemaReader.NextResult() && !singleResult);
        }

        /// <summary>
        /// Identifies SQL Server errors that should fall back to parsing the stored procedure definition.
        /// Known observed live-db discovery failures from the DockerPlayground repro include:
        /// "Invalid object name '#OrderTable'."
        /// "Invalid object name '#OrderLegacyTable'."
        /// "Invalid object name '#OrderSummaryTable'."
        /// "Invalid object name '#OrderSearchTable'."
        /// The broader text matches remain as defensive fallbacks for temp-table wording and describe-result-set errors.
        /// </summary>
        private static bool ShouldTryDefinitionFallback(SqlException ex)
        {
            return ShouldTryDefinitionFallback(ex.Number, ex.Message);
        }

        internal static bool ShouldTryDefinitionFallback(int errorNumber, string message)
        {
            ArgumentNullException.ThrowIfNull(message);

            // Observed live-db failures from DockerPlayground are SqlException 208
            // with temp-table object names such as '#OrderTable'.
            if (errorNumber == 208 && message.Contains('#', StringComparison.Ordinal))
            {
                return true;
            }

            return message.Contains("temp table", StringComparison.OrdinalIgnoreCase)
                || message.Contains("sp_describe_first_result_set", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Reads the stored procedure body from SQL Server and parses it to infer result sets when live discovery fails.
        /// </summary>
        private static List<List<ModuleResultElement>> GetResultSetsFromDefinition(SqlConnection connection, Routine module, bool singleResult)
        {
            var definition = GetRoutineDefinition(connection, module);
            if (string.IsNullOrWhiteSpace(definition))
            {
                return new List<List<ModuleResultElement>>();
            }

            return SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult);
        }

        /// <summary>
        /// Retrieves the T-SQL module definition for the target stored procedure from system catalog views.
        /// </summary>
        private static string GetRoutineDefinition(SqlConnection connection, Routine module)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
SELECT sm.definition
FROM sys.sql_modules sm
INNER JOIN sys.objects o ON sm.object_id = o.object_id
INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
WHERE s.name = @schemaName
AND o.name = @routineName;";

            command.Parameters.AddWithValue("@schemaName", module.Schema);
            command.Parameters.AddWithValue("@routineName", module.Name);

            return command.ExecuteScalar() as string;
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
