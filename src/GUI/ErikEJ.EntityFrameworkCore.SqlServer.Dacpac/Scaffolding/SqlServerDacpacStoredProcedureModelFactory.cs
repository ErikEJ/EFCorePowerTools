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
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public SqlServerDacpacStoredProcedureModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
        {
            _logger = logger;
        }

        public RoutineModel Create(string dacpacPath, ModuleModelFactoryOptions options)
        {
            if (string.IsNullOrEmpty(dacpacPath))
            {
                throw new ArgumentException(@"invalid path", nameof(dacpacPath));
            }
            if (!File.Exists(dacpacPath))
            {
                throw new ArgumentException($"Dacpac file not found: {dacpacPath}");
            }

            return GetStoredProcedures(dacpacPath, options);
        }

        private RoutineModel GetStoredProcedures(string dacpacPath, ModuleModelFactoryOptions options)
        {
            var result = new List<RevEng.Core.Abstractions.Metadata.Procedure>();
            var errors = new List<string>();

            var consolidator = new DacpacConsolidator();
            dacpacPath = consolidator.Consolidate(dacpacPath);

            var model = new TSqlTypedModel(dacpacPath);

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

                        try
                        {
                            procedure.Results.Add(GetStoredProcedureResultElements(proc));
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Unable to get result set shape for {procedure.Schema}.{procedure.Name}" + Environment.NewLine + ex.Message);
                            _logger?.Logger.LogWarning(ex, $"Unable to get result set shape for {procedure.Schema}.{procedure.Name}" + Environment.NewLine + ex.Message);
                        }
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

        private List<ModuleParameter> GetStoredProcedureParameters(TSqlProcedure proc)
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

        private List<ModuleResultElement> GetStoredProcedureResultElements(TSqlProcedure proc)
        {
            var result = new List<ModuleResultElement>();
            var metaProc = new SqlSharpener.Model.Procedure(proc.Element);

            if (metaProc.Selects == null || metaProc.Selects.Count() == 0)
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
