using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Functions;
using RevEng.Core.Procedures;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
#if CORE50 || CORE60
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
#if CORE50 ||CORE60
                SuppressOnConfiguring = !options.IncludeConnectionString,
#endif
#if CORE60
                UseNullableReferenceTypes = options.UseNullableReferences,
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
                    options.UseSchemaFolders,
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

                ApplyRenamers(functionModel.Routines, options.CustomReplacers);

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
                    DiscoverMultipleResultSets = options.UseMultipleSprocResultSets,
                    FullModel = true,
                    Modules = options.Tables.Where(t => t.ObjectType == ObjectType.Procedure).Select(m => m.Name),
                };

                var procedureModel = procedureModelFactory.Create(options.Dacpac ?? options.ConnectionString, procedureModelFactoryOptions);

                ApplyRenamers(procedureModel.Routines, options.CustomReplacers);

                var procedureOptions = new ModuleScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = options.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                    NullableReferences = options.UseNullableReferences,
                    UseSchemaFolders = options.UseSchemaFolders,
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
            bool useSchemaFolders,
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
                        .Where(column => (column.StoreType == "bit" || column.StoreType == "boolean")
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

#if CORE50 || CORE60
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
            AppendSchemaFolders(model, codeModel, useSchemaFolders);

            return codeModel;
        }

        private static void AppendSchemaFolders(IModel databaseModel, ScaffoldedModel scaffoldedModel, bool useSchemaFolders)
        {
            // Tables and views only
            if (!useSchemaFolders)
                return;

            foreach (var entityType in scaffoldedModel.AdditionalFiles)
            {
                var entityTypeName = Path.GetFileNameWithoutExtension(entityType.Path);
                var entityTypeExtension = Path.GetExtension(entityType.Path);
                var entityMatch = databaseModel.GetEntityTypes().Where(x => x.Name == entityTypeName).FirstOrDefault();
                var entityTypeSchema = entityMatch?.GetSchema();
#if CORE50 || CORE60
                if (entityMatch?.GetViewName() != null)
                {
                    entityTypeSchema = entityMatch?.GetViewSchema();
                }
#endif
                if (!string.IsNullOrEmpty(entityTypeSchema))
                {
                    entityType.Path = Path.Combine(entityTypeSchema, entityTypeName + entityTypeExtension);
                }
            }
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
                Directory.CreateDirectory(Path.GetDirectoryName(additionalFilePath));
                File.WriteAllText(additionalFilePath, entityTypeFile.Code, Encoding.UTF8);
                additionalFiles.Add(additionalFilePath);
            }

            return new SavedModelFiles(contextPath, additionalFiles);
        }

        private static void ApplyRenamers(IEnumerable<SqlObjectBase> sqlObjects, List<Schema> renamers)
        {
            if (renamers == null || !renamers.Any())
            {
                return;
            }

            foreach (var sqlObject in sqlObjects)
            {
                var schema = renamers
                    .FirstOrDefault(x => x.SchemaName == sqlObject.Schema);

                if (schema != null)
                {
                    if (schema.Tables != null && schema.Tables.Any(t => t.Name == sqlObject.Name))
                    {
                        sqlObject.NewName = schema.Tables.SingleOrDefault(t => t.Name == sqlObject.Name)?.NewName;
                    }
                    else if (!string.IsNullOrEmpty(schema.TableRegexPattern) && schema.TablePatternReplaceWith != null)
                    {
                        sqlObject.NewName = RegexNameReplace(schema.TableRegexPattern, sqlObject.Name, schema.TablePatternReplaceWith);
                    }
                }
            }
        }

        private static string RegexNameReplace(string pattern, string originalName, string replacement, int timeout = 100)
        {
            string newName = string.Empty;

            try
            {
                newName = Regex.Replace(originalName, pattern, replacement, RegexOptions.None, TimeSpan.FromMilliseconds(timeout));
            }
            catch (RegexMatchTimeoutException)
            {
                Console.WriteLine($"Regex pattern {pattern} time out when trying to match {originalName}, name won't be replaced");
            }

            return newName;
        }
    }
}
