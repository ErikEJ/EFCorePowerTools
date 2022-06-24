using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace RevEng.Core.Procedures
{
    public class SqlServerStoredProcedureModelFactory : SqlServerRoutineModelFactory, IProcedureModelFactory
    {
        public SqlServerStoredProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
            : base(logger)
        {
            RoutineType = "PROCEDURE";
        }

        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetRoutines(connectionString, options);
        }


        protected override List<List<ModuleResultElement>> GetResultElementLists(SqlConnection connection, Routine module, bool multipleResults, bool useLegacyResultSetDiscovery)
        {
            if (connection is null)
            { 
                throw new ArgumentNullException(nameof(connection));
            }

            if (module is null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            if (useLegacyResultSetDiscovery)
            {
                return GetFirstResultSet(connection, module.Schema, module.Name);
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

                if (parameter.ClrType() == typeof(DataTable))
                {
                    param.Value = GetDataTableFromSchema(parameter, connection);
                    param.SqlDbType = SqlDbType.Structured;
                }

                if (parameter.ClrType() == typeof(byte[]))
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
                
                foreach (DataRow row in schemaTable.Rows)
                {
                    var name = row["ColumnName"].ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    var storeType = row["DataTypeName"].ToString();

                    if (row["ProviderSpecificDataType"]?.ToString().StartsWith("Microsoft.SqlServer.Types.Sql", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
#pragma warning disable CA1308 // Normalize strings to uppercase
                        storeType = row["ProviderSpecificDataType"].ToString().Replace("Microsoft.SqlServer.Types.Sql", string.Empty, StringComparison.OrdinalIgnoreCase).ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
                    }

                    list.Add(new ModuleResultElement
                    {
                        Name = name,
                        Nullable = (bool?)row["AllowDBNull"] ?? true,
                        Ordinal = (int)row["ColumnOrdinal"],
                        StoreType = storeType,
                    });
                }

                result.Add(list);
            } while (schemaReader.NextResult() && !singleResult);

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

        private static List<List<ModuleResultElement>> GetFirstResultSet(SqlConnection connection, string schema, string moduleName)
        {
            using var dtResult = new DataTable();
            var list = new List<ModuleResultElement>();

            var sql = $"exec dbo.sp_describe_first_result_set N'[{schema}].[{moduleName}]';";

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            using var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

            adapter.Fill(dtResult);

            int rCounter = 0;

            foreach (DataRow row in dtResult.Rows)
            {
                var name = row["name"].ToString();
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                var parameter = new ModuleResultElement()
                {
                    Name = name,
                    StoreType = string.IsNullOrEmpty(row["system_type_name"].ToString()) ? row["user_type_name"].ToString() : row["system_type_name"].ToString(),
                    Ordinal = int.Parse(row["column_ordinal"].ToString(), CultureInfo.InvariantCulture),
                    Nullable = (bool)row["is_nullable"],
                };

                list.Add(parameter);

                rCounter++;
            }

            var result = new List<List<ModuleResultElement>>
            {
                list
            };

            return result;
        }
    }
}
