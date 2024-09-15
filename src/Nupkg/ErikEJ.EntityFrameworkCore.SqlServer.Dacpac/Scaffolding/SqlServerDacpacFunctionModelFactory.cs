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
    public class SqlServerDacpacFunctionModelFactory : IFunctionModelFactory
    {
        private readonly SqlServerDacpacDatabaseModelFactoryOptions dacpacOptions;

        public SqlServerDacpacFunctionModelFactory(SqlServerDacpacDatabaseModelFactoryOptions options)
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

            var scalars = GetScalarFunctions(connectionString, options, dacpacOptions.MergeDacpacs);

            var tableValued = GetTableValuedFunctions(connectionString, options, dacpacOptions.MergeDacpacs);

            return new RoutineModel
            {
                Routines = scalars.Routines.Concat(tableValued.Routines).ToList(),
                Errors = scalars.Errors.Concat(tableValued.Errors).ToList(),
            };
        }

        private static RoutineModel GetScalarFunctions(string dacpacPath, ModuleModelFactoryOptions options, bool mergeDacpacs)
        {
            var result = new List<Function>();
            var errors = new List<string>();

            if (mergeDacpacs && dacpacPath != null)
            {
                dacpacPath = DacpacConsolidator.Consolidate(dacpacPath);
            }

            using var model = new TSqlTypedModel(dacpacPath);

            var functions = model.GetObjects<TSqlScalarFunction>(DacQueryScopes.UserDefined)
               .ToList();

            var filter = new HashSet<string>(options.Modules);

            foreach (var func in functions)
            {
                var function = new Function
                {
                    Schema = func.Name.Parts[0],
                    Name = func.Name.Parts[1],
                    IsScalar = true,
                };

                var key = $"[{function.Schema}].[{function.Name}]";

                if (filter.Count == 0 || filter.Contains(key))
                {
                    if (options.FullModel)
                    {
                        function.Parameters = GetFunctionParameters(func.Parameters);
                        function.Parameters.Insert(0, new ModuleParameter
                        {
                            StoreType = func.ReturnType.First().Name.Parts[0],
                            Output = false,
                            Nullable = true,
                        });
                    }

                    result.Add(function);
                }
            }

            return new RoutineModel
            {
                Routines = result.Cast<Routine>().ToList(),
                Errors = errors,
            };
        }

        private static RoutineModel GetTableValuedFunctions(string dacpacPath, ModuleModelFactoryOptions options, bool mergeDacpacs)
        {
            var result = new List<Function>();
            var errors = new List<string>();

            if (mergeDacpacs && dacpacPath != null)
            {
                dacpacPath = DacpacConsolidator.Consolidate(dacpacPath);
            }

            using var model = new TSqlTypedModel(dacpacPath);

            var functions = model.GetObjects<TSqlTableValuedFunction>(DacQueryScopes.UserDefined)
               .ToList();

            var filter = new HashSet<string>(options.Modules);

            foreach (var func in functions)
            {
                var function = new Function
                {
                    Schema = func.Name.Parts[0],
                    Name = func.Name.Parts[1],
                    IsScalar = false,
                };

                var key = $"[{function.Schema}].[{function.Name}]";

                if (filter.Count == 0 || filter.Contains(key))
                {
                    if (options.FullModel)
                    {
                        var columnList = new List<ModuleResultElement>();
                        var i = 0;
                        foreach (var column in func.Columns)
                        {
                            columnList.Add(new ModuleResultElement
                            {
                                StoreType = column.DataType.First().Name.Parts[0],
                                Nullable = column.Nullable,
                                Name = column.Name.Parts[2],
                                Ordinal = i++,
                            });
                        }

                        function.Results.Add(columnList);

                        function.Parameters = GetFunctionParameters(func.Parameters);
                    }

                    result.Add(function);
                }
            }

            return new RoutineModel
            {
                Routines = result.Cast<Routine>().ToList(),
                Errors = errors,
            };
        }

        private static List<ModuleParameter> GetFunctionParameters(IEnumerable<TSqlParameter> parameters)
        {
            var result = new List<ModuleParameter>();

            string typeName = null;

            foreach (var parameter in parameters)
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

            return result;
        }
    }
}
