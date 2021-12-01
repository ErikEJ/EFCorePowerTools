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
    public class SqlServerStoredProcedureModelFactory : SqlServerRoutineModelFactory, IProcedureModelFactory
    {
        public SqlServerStoredProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
            : base(logger)
        {
        }

        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            return GetRoutines(connectionString, options);
        }

        protected override List<ModuleParameter> GetParameters(SqlConnection connection, string schema, string name)
        {
            var moduleParameters = base.GetParameters(connection, schema, name);

            // Add parameter to hold the standard return value
            moduleParameters.Add(new ModuleParameter()
            {
                Name = "returnValue",
                StoreType = "int",
                Output = true,
                Nullable = false,
                IsReturnValue = true,
            });

            return moduleParameters;
        }

        protected override List<List<ModuleResultElement>> GetResultElementLists(SqlConnection connection, Routine module, bool multipleResultSets)
        {
            if (multipleResultSets)
            {
                GetAllResultSets(connection, module);        
            }

            return GetFirstResultSet(connection, module.Schema, module.Name);
        }


        private static List<List<ModuleResultElement>> GetAllResultSets(SqlConnection connection, Routine model)
        {
            var list = new List<ModuleResultElement>();

            var sqlCommand = connection.CreateCommand();
            //Add parameters!

            using var schemaReader = sqlCommand.ExecuteReader(CommandBehavior.SchemaOnly);

            do
            {
                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldatareader.getschematable.aspx
                var schemaTable = schemaReader.GetSchemaTable();

                //For each field in the table...
                foreach (DataRow row in schemaTable.Rows)
                {
                    
                }
            } while (schemaReader.NextResult());


            var result = new List<List<ModuleResultElement>>
            {
                list
            };

            return result;
        }

        private static List<List<ModuleResultElement>> GetFirstResultSet(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var list = new List<ModuleResultElement>();

            var sql = $"exec dbo.sp_describe_first_result_set N'[{schema}].[{name}]';";

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
                    StoreType = string.IsNullOrEmpty(res["system_type_name"].ToString()) ? res["user_type_name"].ToString() : res["system_type_name"].ToString(),
                    Ordinal = int.Parse(res["column_ordinal"].ToString()),
                    Nullable = (bool)res["is_nullable"],
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
