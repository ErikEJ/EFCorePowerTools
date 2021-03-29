using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Functions;
using RevEng.Core.Procedures;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RevEng.Core
{
    class ReverseEngineerScaffolder
    {
        public static SavedModelFiles GenerateDbContext(
            ReverseEngineerCommandOptions options,
            ServiceProvider serviceProvider,
            List<string> schemas,
            string outputContextDir,
            string modelNamespace,
            string contextNamespace)
        {
            SavedModelFiles filePaths;
            var modelOptions = new ModelReverseEngineerOptions
            {
                UseDatabaseNames = options.UseDatabaseNames,
#if CORE50
                NoPluralize = !options.UseInflector,
#endif
            };

            var codeOptions = new ModelCodeGenerationOptions
            {
                UseDataAnnotations = !options.UseFluentApiOnly,
                Language = "C#",
                ContextName = options.ContextClassName,
                ContextDir = outputContextDir,
                RootNamespace = null,
                ContextNamespace = contextNamespace,
                ModelNamespace = modelNamespace,
                SuppressConnectionStringWarning = false,
                ConnectionString = options.ConnectionString,
#if CORE50
                SuppressOnConfiguring = !options.IncludeConnectionString,
#endif
            };

            var dbOptions = new DatabaseModelFactoryOptions(options.Tables.Where(t => t.ObjectType.HasColumns()).Select(m => m.Name), schemas);

            var scaffoldedModel = ScaffoldModel(
                    options.Dacpac ?? options.ConnectionString,
                    dbOptions,
                    modelOptions,
                    codeOptions,
                    options.UseBoolPropertiesWithoutDefaultSql,
                    options.UseNoNavigations,
                    options.SelectedToBeGenerated == 1, //DbContext only
                    options.SelectedToBeGenerated == 2, //Entities only
                    serviceProvider);

            filePaths = Save(
                scaffoldedModel,
                Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputPath ?? string.Empty)));
            return filePaths;
        }

        public static SavedModelFiles GenerateFunctions(
            ReverseEngineerCommandOptions options,
            ref List<string> errors,
            ServiceProvider serviceProvider,
            string outputContextDir,
            string modelNamespace,
            string contextNamespace)
        {
            var functionModelScaffolder = serviceProvider.GetService<IFunctionScaffolder>();
            if (functionModelScaffolder != null
                && (options.Tables.Any(t => t.ObjectType == ObjectType.ScalarFunction)
                    || !options.Tables.Any()))
            {
                var functionModelFactory = serviceProvider.GetService<IFunctionModelFactory>();

                var modelFactoryOptions = new ModuleModelFactoryOptions
                {
                    FullModel = true,
                    Modules = options.Tables.Where(t => t.ObjectType == ObjectType.ScalarFunction).Select(m => m.Name),
                };

                var functionModel = functionModelFactory.Create(options.Dacpac ?? options.ConnectionString, modelFactoryOptions);

                var functionOptions = new ModuleScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = options.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                    NullableReferences = options.UseNullableReferences,
                };

                var functionScaffoldedModel = functionModelScaffolder.ScaffoldModel(functionModel, functionOptions, ref errors);

                if (functionScaffoldedModel != null)
                {
                    return functionModelScaffolder.Save(
                        functionScaffoldedModel,
                        Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputPath ?? string.Empty)),
                        contextNamespace);
                }
            }

            return null;
        }

        public static SavedModelFiles GenerateStoredProcedures(
            ReverseEngineerCommandOptions options,
            ref List<string> errors,
            ServiceProvider serviceProvider,
            string outputContextDir,
            string modelNamespace,
            string contextNamespace)
        {
            var procedureModelScaffolder = serviceProvider.GetService<IProcedureScaffolder>();
            if (procedureModelScaffolder != null
                && (options.Tables.Any(t => t.ObjectType == ObjectType.Procedure)
                    || !options.Tables.Any()))
            {
                var procedureModelFactory = serviceProvider.GetService<IProcedureModelFactory>();

                var procedureModelFactoryOptions = new ModuleModelFactoryOptions
                {
                    FullModel = true,
                    Modules = options.Tables.Where(t => t.ObjectType == ObjectType.Procedure).Select(m => m.Name),
                };

                var procedureModel = procedureModelFactory.Create(options.Dacpac ?? options.ConnectionString, procedureModelFactoryOptions);

                var procedureOptions = new ModuleScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = options.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                    NullableReferences = options.UseNullableReferences,
                    ProceduresReturnList = options.ProceduresReturnList,
                };

                var procedureScaffoldedModel = procedureModelScaffolder.ScaffoldModel(procedureModel, procedureOptions, ref errors);

                if (procedureScaffoldedModel != null)
                {
                    return procedureModelScaffolder.Save(
                        procedureScaffoldedModel,
                        Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputPath ?? string.Empty)),
                        contextNamespace);
                }
            }

            return null;
        }

        private static ScaffoldedModel ScaffoldModel(
            string connectionString,
            DatabaseModelFactoryOptions databaseOptions,
            ModelReverseEngineerOptions modelOptions,
            ModelCodeGenerationOptions codeOptions,
            bool removeNullableBoolDefaults,
            bool excludeNavigations,
            bool dbContextOnly,
            bool entitiesOnly,
            ServiceProvider serviceProvider)
        {
            var _databaseModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();
            var _factory = serviceProvider.GetService<IScaffoldingModelFactory>();
            var _selector = serviceProvider.GetService<IModelCodeGeneratorSelector>();

            var databaseModel = _databaseModelFactory.Create(connectionString, databaseOptions);

            if (removeNullableBoolDefaults)
            {
                foreach (var column in databaseModel.Tables
                    .SelectMany(table => table.Columns
                        .Where(column => column.StoreType == "bit"
                            && !column.IsNullable
                            && !string.IsNullOrEmpty(column.DefaultValueSql))))
                {
                    column.DefaultValueSql = null;
                }
            }

            if (excludeNavigations)
            {
                foreach (var table in databaseModel.Tables)
                {
                    table.ForeignKeys.Clear();
                }
            }

#if CORE50
            var model = _factory.Create(databaseModel, modelOptions);
#else
            var model = _factory.Create(databaseModel, modelOptions.UseDatabaseNames);
#endif
            if (model == null)
            {
                throw new InvalidOperationException($"No model from provider {_factory.GetType().ShortDisplayName()}");
            }

            var codeGenerator = _selector.Select(codeOptions.Language);

            var codeModel = codeGenerator.GenerateModel(model, codeOptions);
            if (entitiesOnly)
            {
                codeModel.ContextFile = null;
            }
            if (dbContextOnly)
            {
                codeModel.AdditionalFiles.Clear();
            }

            return codeModel;
        }

        private static SavedModelFiles Save(
           ScaffoldedModel scaffoldedModel,
           string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            var contextPath = string.Empty;

            if (scaffoldedModel.ContextFile != null)
            {
                contextPath = Path.GetFullPath(Path.Combine(outputDir, scaffoldedModel.ContextFile!.Path));
                Directory.CreateDirectory(Path.GetDirectoryName(contextPath)!);
                File.WriteAllText(contextPath, scaffoldedModel.ContextFile.Code, Encoding.UTF8);
            }
            var additionalFiles = new List<string>();
            foreach (var entityTypeFile in scaffoldedModel.AdditionalFiles)
            {
                var additionalFilePath = Path.Combine(outputDir, entityTypeFile.Path);
                File.WriteAllText(additionalFilePath, entityTypeFile.Code, Encoding.UTF8);
                additionalFiles.Add(additionalFilePath);
            }

            return new SavedModelFiles(contextPath, additionalFiles);
        }
    }
}
