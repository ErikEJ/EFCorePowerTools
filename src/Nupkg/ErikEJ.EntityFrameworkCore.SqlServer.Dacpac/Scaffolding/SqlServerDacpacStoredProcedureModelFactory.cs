using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GOEddie.Dacpac.References;
using Microsoft.SqlServer.Dac.Extensions.Prototype;
using Microsoft.SqlServer.Dac.Model;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;

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

            ArgumentNullException.ThrowIfNull(options);

            return GetStoredProcedures(connectionString, options, dacpacOptions.MergeDacpacs);
        }

        private static RoutineModel GetStoredProcedures(string dacpacPath, ModuleModelFactoryOptions options, bool mergeDacpacs)
        {
            var result = new List<RevEng.Core.Abstractions.Metadata.Procedure>();
            var errors = new List<string>();

            if (mergeDacpacs && dacpacPath != null)
            {
                dacpacPath = DacpacConsolidator.Consolidate(dacpacPath);
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

                var key = $"[{procedure.Schema}].[{procedure.Name}]";

                if (filter.Count == 0 || filter.Contains(key))
                {
                    if (options.FullModel)
                    {
                        procedure.Parameters = GetStoredProcedureParameters(proc);

                        if (options.MappedModules?.ContainsKey(key) ?? false)
                        {
                            procedure.MappedType = options.MappedModules[key];
                        }

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

            string typeName = null;

            foreach (var parameter in proc.Parameters)
            {
                var storeType = parameter.DataType.First().Name.Parts[0];

                var dtReference = parameter.DataType.First() as TSqlDataTypeReference;

#pragma warning disable S2219 // Runtime type checking should be simplified
                if (dtReference != null
                    && dtReference.Type != null
                    && dtReference.Type.Any()
                    && dtReference.Type.First() is TSqlDataTypeReference)
                {
                    storeType = dtReference.Type.First().Name.Parts[0];
                }

                if (parameter.DataType.First() is TSqlUserDefinedTypeReference udtReference)
                {
                    storeType = udtReference.Name.Parts[1];
                }

                if (parameter.DataType.First() is TSqlTableTypeReference tableReference)
                {
                    // parameter is a table type (TVP)
                    storeType = "structured";
                    typeName = tableReference.Name.ToString();
                }

#pragma warning restore S2219 // Runtime type checking should be simplified

                var newParameter = new ModuleParameter()
                {
                    Length = parameter.Length,
                    Name = parameter.Name.Parts[2].Trim('@'),
                    Output = parameter.IsOutput,
                    Precision = parameter.Precision,
                    Scale = parameter.Scale,
                    StoreType = storeType,
                    Nullable = true,
                    TypeName = typeName,
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
                IsReturnValue = true,
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
            foreach (var column in metaProc.Selects.First().Columns)
            {
                if (column.DataTypes != null)
                {
                    result.Add(new ModuleResultElement
                    {
                        Name = column.Name,
                        Nullable = column.IsNullable,
                        StoreType = column.DataTypes[SqlSharpener.TypeFormat.SqlServerDbType],
                        Ordinal = ordinal++,
                    });
                }
            }

            return result;
        }
    }
}
