using GOEddie.Dacpac.References;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Dac.Extensions.Prototype;
using Microsoft.SqlServer.Dac.Model;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding
{
    public class SqlServerDacpacStoredProcedureModelFactory : IProcedureModelFactory
    {
        private readonly SqlServerDacpacDatabaseModelFactoryOptions dacpacOptions;

        public SqlServerDacpacStoredProcedureModelFactory(SqlServerDacpacDatabaseModelFactoryOptions options)
        {
            dacpacOptions = options;
        }

        public RoutineModel Create(string connectionString, ModuleModelFactoryOptions options)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(@"invalid path", nameof(connectionString));
            }
            if (!File.Exists(connectionString))
            {
                throw new ArgumentException($"Dacpac file not found: {connectionString}");
            }
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return GetStoredProcedures(connectionString, options, dacpacOptions.MergeDacpacs);
        }

        private static RoutineModel GetStoredProcedures(string dacpacPath, ModuleModelFactoryOptions options, bool mergeDacpacs)
        {
            var result = new List<RevEng.Core.Abstractions.Metadata.Procedure>();
            var errors = new List<string>();

            if (mergeDacpacs && dacpacPath != null)
            {
                var consolidator = new DacpacConsolidator();
                dacpacPath = consolidator.Consolidate(dacpacPath);
            }

            using var model = new TSqlTypedModel(dacpacPath);

            var procedures = model.GetObjects<TSqlProcedure>(DacQueryScopes.UserDefined)
               .ToList();

            var filter = new HashSet<string>(options.Modules);

            foreach (var proc in procedures)
            {
                var procedure = new RevEng.Core.Abstractions.Metadata.Procedure
                {
                    Schema = proc.Name.Parts[0],
                    Name = proc.Name.Parts[1],
                };

                if (filter.Count == 0 || filter.Contains($"[{procedure.Schema}].[{procedure.Name}]"))
                {
                    if (options.FullModel)
                    {
                        procedure.Parameters = GetStoredProcedureParameters(proc);

#pragma warning disable CA1031 // Do not catch general exception types
                        try
                        {
                            procedure.Results.Add(GetStoredProcedureResultElements(proc));
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Unable to get result set shape for {procedure.Schema}.{procedure.Name}" + Environment.NewLine + ex.ToString());
                        }
#pragma warning restore CA1031 // Do not catch general exception types
                    }

                    result.Add(procedure);
                }
            }

            return new RoutineModel
            {
                Routines = result.Cast<Routine>().ToList(),
                Errors = errors,
            };
        }

        private static List<ModuleParameter> GetStoredProcedureParameters(TSqlProcedure proc)
        {
            var result = new List<ModuleParameter>();

            foreach (var parameter in proc.Parameters)
            {
                var newParameter = new ModuleParameter()
                {
                     Length = parameter.Length,
                     Name = parameter.Name.Parts[2].Trim('@'),
                     Output = parameter.IsOutput,
                     Precision = parameter.Precision,
                     Scale = parameter.Scale,
                     StoreType = parameter.DataType.First().Name.Parts[0],
                     Nullable = true,
                     //TypeName = parameter.ObjectType.Name,
                };

                result.Add(newParameter);
            }

            // Add parameter to hold the standard return value
            result.Add(new ModuleParameter()
            {
                Name = "returnValue",
                StoreType = "int",
                Output = true,
                Nullable = false,
            });

            return result;
        }

        private static List<ModuleResultElement> GetStoredProcedureResultElements(TSqlProcedure proc)
        {
            var result = new List<ModuleResultElement>();
            var metaProc = new SqlSharpener.Model.Procedure(proc.Element);

            if (metaProc.Selects == null || !metaProc.Selects.Any())
            {
                return result;
            }

            int ordinal = 0;
            foreach (var column in metaProc.Selects.FirstOrDefault()?.Columns)
            {
                result.Add(new ModuleResultElement
                { 
                    Name = column.Name,
                    Nullable = column.IsNullable,
                    StoreType = column.DataTypes[SqlSharpener.TypeFormat.SqlServerDbType],
                    Ordinal = ordinal++,
                });
            }

            return result;
        }
    }
}
