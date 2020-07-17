using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RevEng.Core.Procedures.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RevEng.Core.Procedures
{
    class SqlServerProcedureModelFactory : IProcedureModelFactory
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        public ProcedureModel Create(string connectionString, ProcedureModelFactoryOptions options)
        {
            //TODO Filter based on options later

            var model = new ProcedureModel
            {
                Procedures = GetStoredProcedures(connectionString)
            };

            return model;
        }


        private List<StoredProcedure> GetStoredProcedures(string connectionString)
        {
            var dtResult = new DataTable();
            var result = new List<StoredProcedure>();

            using (var connection = new SqlConnection(connectionString))
            {
                string sql = $@"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME";

                connection.Open();

                var adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(sql, connection)
                };

                adapter.Fill(dtResult);

                foreach (DataRow row in dtResult.Rows)
                {
                    try
                    {
                        var procedure = new StoredProcedure
                        {
                            Schema = row["ROUTINE_SCHEMA"].ToString(),
                            Name = row["ROUTINE_NAME"].ToString()
                        };

                        procedure.Parameters = GetStoredProcedureParameters(connection, procedure.Schema, procedure.Name);

                        procedure.ResultElements = GetStoredProcedureResultElements(connection, procedure.Schema, procedure.Name);

                        result.Add(procedure);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                        System.Diagnostics.Trace.WriteLine($"Unable to scaffold {row["ROUTINE_NAME"]}");
                        _logger?.Logger.LogWarning(ex, $"Unable to scaffold {row["ROUTINE_NAME"]}");
                    }
                }
            }

            return result;
        }

        private List<StoredProcedureParameter> GetStoredProcedureParameters(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var result = new List<StoredProcedureParameter>();

            // Validate this - based on https://stackoverflow.com/questions/20115881/how-to-get-stored-procedure-parameters-details/41330791

            var sql = $@"
SELECT  
    'Parameter' = name,  
    'Type'   = type_name(system_type_id),  
    'Length'   = CAST(max_length AS INT),  
    'Precision'   = CAST(case when type_name(system_type_id) = 'uniqueidentifier' 
                then precision  
                else OdbcPrec(system_type_id, max_length, precision) end AS INT),  
    'Scale'   = CAST(OdbcScale(system_type_id, scale) AS INT),  
    'Order'  = CAST(parameter_id AS INT),  
    'Collation'   = convert(sysname, 
                    case when system_type_id in (35, 99, 167, 175, 231, 239)  
                    then ServerProperty('collation') end),
    is_output AS output,
	is_nullable AS nullable
    from sys.parameters where object_id = object_id('{schema}.{name}')
    ORDER BY parameter_id;";

            var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };

            adapter.Fill(dtResult);

            foreach (DataRow par in dtResult.Rows)
            {
                var parameter = new StoredProcedureParameter()
                {
                    Name = par["Parameter"].ToString().Replace("@", ""),
                    Length = par["Length"].GetType() == typeof(DBNull) ? (int?)null : int.Parse(par["Length"].ToString()),
                    Precision = par["Precision"].GetType() == typeof(DBNull) ? (byte?)null : byte.Parse(par["Precision"].ToString()),
                    Scale = par["Scale"].GetType() == typeof(DBNull) ? (byte?)null : byte.Parse(par["Scale"].ToString()),
                    Order = int.Parse(par["Order"].ToString()),
                    Output = (bool)par["output"],
                    Nullable = (bool)par["nullable"],
                    SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), par["Type"].ToString(), true),
                };

                parameter.ClrType = GetClrType(parameter.SqlDbType, parameter.Nullable);

                result.Add(parameter);
            }

            return result;
        }

        private List<StoredProcedureResultElement> GetStoredProcedureResultElements(SqlConnection connection, string schema, string name)
        {
            var dtResult = new DataTable();
            var result = new List<StoredProcedureResultElement>();

            var sql = $@"exec dbo.sp_describe_first_result_set N'[{schema}].[{name}]';";

            var adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(sql, connection)
            };

            adapter.Fill(dtResult);

            int rCounter = 0;

            foreach (DataRow res in dtResult.Rows)
            {
                var cleanedTypeName = RemoveMatchingBraces(res["system_type_name"].ToString());

                var parameter = new StoredProcedureResultElement()
                {
                    Name = (string.IsNullOrEmpty(res["name"].ToString()) ? $"Col{rCounter}" : res["name"].ToString()),
                    Order = int.Parse(res["column_ordinal"].ToString()),
                    Nullable = (bool)res["is_nullable"],
                    SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), cleanedTypeName, true),
                };

                parameter.ClrType = GetClrType(parameter.SqlDbType, parameter.Nullable);

                result.Add(parameter);

                rCounter++;
            }

            return result;
        }

        private static string RemoveMatchingBraces(string s)
        {
            var stack = new Stack<char>();
            int count = 0;
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '(':
                        count += 1;
                        stack.Push(ch);
                        break;
                    case ')':
                        if (count == 0)
                            stack.Push(ch);
                        else
                        {
                            char popped;
                            do
                            {
                                popped = stack.Pop();
                            } while (popped != '(');
                            count -= 1;
                        }
                        break;
                    default:
                        stack.Push(ch);
                        break;
                }
            }
            return string.Join("", stack.Reverse());
        }

        private static Type GetClrType(SqlDbType sqlType, bool isNullable)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return isNullable ? typeof(long?) : typeof(long);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return isNullable ? typeof(bool?) : typeof(bool);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return isNullable ? typeof(DateTime?) : typeof(DateTime);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return isNullable ? typeof(decimal?) : typeof(decimal);

                case SqlDbType.Float:
                    return isNullable ? typeof(double?) : typeof(double);

                case SqlDbType.Int:
                    return isNullable ? typeof(int?) : typeof(int);

                case SqlDbType.Real:
                    return isNullable ? typeof(float?) : typeof(float);

                case SqlDbType.UniqueIdentifier:
                    return isNullable ? typeof(Guid?) : typeof(Guid);

                case SqlDbType.SmallInt:
                    return isNullable ? typeof(short?) : typeof(short);

                case SqlDbType.TinyInt:
                    return isNullable ? typeof(byte?) : typeof(byte);

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }
    }
}