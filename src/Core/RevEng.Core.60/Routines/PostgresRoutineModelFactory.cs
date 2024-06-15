using Npgsql;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace RevEng.Core.Routines
{
    public abstract class PostgresRoutineModelFactory
    {
        public string RoutineSql { get; set; }

        public string RoutineType { get; set; }

#pragma warning disable CA1716 // Identifiers should not match keywords
        protected abstract List<List<ModuleResultElement>> GetResultElementLists(NpgsqlConnection connection, Routine module);
#pragma warning restore CA1716 // Identifiers should not match keywords

        protected RoutineModel GetRoutines(string connectionString, ModuleModelFactoryOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            var result = new List<Routine>();
            var found = new List<Tuple<string, string, bool, char>>();
            var errors = new List<string>();

            var filter = options.Modules.ToHashSet();

            using (var connection = new NpgsqlConnection(connectionString))
            {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                using (var command = new NpgsqlCommand(RoutineSql, connection))
                {
                    connection.Open();

                    if (connection.PostgreSqlVersion.Major < 11)
                    {
                        return new RoutineModel
                        {
                            Routines = result,
                            Errors = errors,
                        };
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Schema, Name, IsScalar, Type (f or p)
                            found.Add(new Tuple<string, string, bool, char>(reader.GetString(0), reader.GetString(1), reader.GetFieldValue<bool>(2), reader.GetChar(3)));
                        }
                    }
                }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                var allParameters = options.FullModel && found.Count != 0 ? GetParameters(connection) : new Dictionary<string, List<ModuleParameter>>();

                foreach (var foundModule in found)
                {
                    var key = $"{foundModule.Item1}.{foundModule.Item2}";

                    if (filter.Count == 0 || filter.Contains(key))
                    {
                        var isScalar = false; //// !foundModule.Item3;

                        var module = RoutineType == "PROCEDURE"
                            ? (Routine)new Procedure()
                            : new Function { IsScalar = isScalar };

                        module.Schema = foundModule.Item1;
                        module.Name = foundModule.Item2;
                        module.IsScalar = foundModule.Item4 == 'f';

                        if (options.FullModel)
                        {
                            module.HasValidResultSet = true;

                            if (options.MappedModules?.ContainsKey(key) ?? false)
                            {
                                module.MappedType = options.MappedModules[key];
                            }

                            if (allParameters.TryGetValue($"{module.Schema}.{module.Name}", out var moduleParameters))
                            {
                                module.Parameters = moduleParameters;
                            }

#pragma warning disable CA1031 // Do not catch general exception types
                            try
                            {
                                module.Results.AddRange(GetResultElementLists(connection, module));
                            }
                            catch (Exception ex)
                            {
                                module.HasValidResultSet = false;
                                module.Results = new List<List<ModuleResultElement>>
                                {
                                    new List<ModuleResultElement>(),
                                };
#pragma warning disable CA1308 // Normalize strings to uppercase
                                errors.Add($"Unable to get result set shape for {RoutineType.ToLower(CultureInfo.InvariantCulture)} '{module.Schema}.{module.Name}'. {ex.Message}.");
#pragma warning restore CA1308 // Normalize strings to uppercase
                            }
#pragma warning restore CA1031 // Do not catch general exception types
                        }

                        if (!isScalar)
                        {
                            result.Add(module);
                        }
                    }
                }
            }

            return new RoutineModel
            {
                Routines = result,
                Errors = errors,
            };
        }

        private static Dictionary<string, List<ModuleParameter>> GetParameters(NpgsqlConnection connection)
        {
            using var dtResult = new DataTable();
            var result = new List<ModuleParameter>();

            var sql = $@"
select proc.specific_schema as procedure_schema,
       proc.routine_name as procedure_name,
       args.parameter_name,
       args.parameter_mode,
       args.data_type,
	   args.character_maximum_length,
	   args.numeric_precision,
	   args.numeric_scale
from information_schema.routines proc
left join information_schema.parameters args
          on proc.specific_schema = args.specific_schema
          and proc.specific_name = args.specific_name
where proc.routine_schema not in ('pg_catalog', 'information_schema')
     and (proc.routine_type = 'PROCEDURE' OR proc.routine_type = 'FUNCTION')
order by procedure_schema,
         proc.specific_name,
         procedure_name,
         args.ordinal_position;";

            using var adapter = new NpgsqlDataAdapter
            {
                SelectCommand = new NpgsqlCommand(sql, connection),
            };

            adapter.Fill(dtResult);

            foreach (DataRow par in dtResult.Rows)
            {
                if (par != null)
                {
                    var parameter = new ModuleParameter()
                    {
                        Name = par["parameter_name"].ToString(),
                        RoutineName = par["procedure_name"].ToString(),
                        RoutineSchema = par["procedure_schema"].ToString(),
                        StoreType = par["data_type"].ToString(),
                        Length = (par["character_maximum_length"] is DBNull) ? (int?)null : int.Parse(par["character_maximum_length"].ToString()!, CultureInfo.InvariantCulture),
                        Precision = (par["numeric_precision"] is DBNull) ? (int?)null : int.Parse(par["Precision"].ToString()!, CultureInfo.InvariantCulture),
                        Scale = (par["numeric_scale"] is DBNull) ? (int?)null : int.Parse(par["Scale"].ToString()!, CultureInfo.InvariantCulture),
                        Output = par["parameter_mode"].ToString() == "INOUT",
                        Nullable = true,
                    };

                    if (par["parameter_mode"].ToString() != "OUT")
                    {
                        result.Add(parameter);
                    }
                }
            }

            return result.GroupBy(x => $"{x.RoutineSchema}.{x.RoutineName}").ToDictionary(g => g.Key, g => g.ToList(), StringComparer.InvariantCulture);
        }
    }
}
