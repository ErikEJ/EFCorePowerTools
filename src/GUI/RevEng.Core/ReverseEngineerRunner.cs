using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
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
    public class ReverseEngineerRunner
    {
        public static ReverseEngineerResult GenerateFiles(ReverseEngineerCommandOptions options)
        {
            var errors = new List<string>();
            var warnings = new List<string>();
            var reporter = new OperationReporter(
                new OperationReportHandler(
                    m => errors.Add(m),
                    m => warnings.Add(m)));
            var serviceProvider = ServiceProviderBuilder.Build(options);
            var scaffolder = serviceProvider.GetService<IReverseEngineerScaffolder>();
            var schemas = new List<string>();

            options.ConnectionString = SqlServerHelper.SetConnectionString(options.DatabaseType, options.ConnectionString);

            if (options.DefaultDacpacSchema != null)
            {
                schemas.Add(options.DefaultDacpacSchema);
            }

            if (options.FilterSchemas)
            {
                schemas.AddRange(options.Schemas.Select(s => s.Name));
            }

            if (options.UseNoObjectFilter)
            {
                options.Tables = new List<SerializationTableModel>();
            }

            var outputDir = !string.IsNullOrEmpty(options.OutputPath)
               ? Path.IsPathFullyQualified(options.OutputPath)
                    ? options.OutputPath
                    : Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputPath))
                : options.ProjectPath;

            var outputContextDir = !string.IsNullOrEmpty(options.OutputContextPath)
               ? Path.IsPathFullyQualified(options.OutputContextPath)
                    ? options.OutputContextPath
                    : Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputContextPath))
                : outputDir;

            var modelNamespace = string.Empty;
            var contextNamespace = string.Empty;

            if (string.IsNullOrEmpty(options.ProjectRootNamespace))
            {
                modelNamespace = !string.IsNullOrEmpty(options.ModelNamespace)
                    ? options.ModelNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputDir, options.ProjectPath, options.ProjectRootNamespace);

                contextNamespace = !string.IsNullOrEmpty(options.ContextNamespace)
                    ? options.ContextNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputContextDir, options.ProjectPath, options.ProjectRootNamespace);
            }
            else
            {
                modelNamespace = !string.IsNullOrEmpty(options.ModelNamespace)
                    ? options.ProjectRootNamespace + "." + options.ModelNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputDir, options.ProjectPath, options.ProjectRootNamespace);

                contextNamespace = !string.IsNullOrEmpty(options.ContextNamespace)
                    ? options.ProjectRootNamespace + "." + options.ContextNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputContextDir, options.ProjectPath, options.ProjectRootNamespace);
            }

            SavedModelFiles procedurePaths = GenerateStoredProcedures(options, ref errors, serviceProvider, outputContextDir, modelNamespace, contextNamespace);

            SavedModelFiles functionPaths = GenerateFunctions(options, ref errors, serviceProvider, outputContextDir, modelNamespace, contextNamespace);

            SavedModelFiles filePaths = GenerateDbContext(options, serviceProvider, scaffolder, schemas, outputContextDir, modelNamespace, contextNamespace);

#if CORE50
#else
            RemoveOnConfiguring(filePaths.ContextFile, options.IncludeConnectionString);
#endif
            foreach (var file in filePaths.AdditionalFiles)
            {
                PostProcess(file);
            }

            PostProcess(filePaths.ContextFile);

            var entityTypeConfigurationPaths = SplitDbContext(filePaths.ContextFile, options.UseDbContextSplitting, contextNamespace);

            var cleanUpPaths = CreateCleanupPaths(procedurePaths, functionPaths, filePaths);

            CleanUp(cleanUpPaths, entityTypeConfigurationPaths);

            var result = new ReverseEngineerResult
            {
                EntityErrors = errors,
                EntityWarnings = warnings,
                EntityTypeFilePaths = filePaths.AdditionalFiles,
                ContextFilePath = filePaths.ContextFile,
                ContextConfigurationFilePaths = entityTypeConfigurationPaths,
            };

            return result;
        }

        private static SavedModelFiles GenerateDbContext(
            ReverseEngineerCommandOptions options, 
            ServiceProvider serviceProvider, 
            IReverseEngineerScaffolder scaffolder, 
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
                    serviceProvider);

            filePaths = scaffolder.Save(
                scaffoldedModel,
                Path.GetFullPath(Path.Combine(options.ProjectPath, options.OutputPath ?? string.Empty)),
                overwriteFiles: true);
            return filePaths;
        }

        private static SavedModelFiles GenerateFunctions(
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

        private static SavedModelFiles GenerateStoredProcedures(
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
            ServiceProvider serviceProvider)
        {
            var _databaseModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();
            var _factory = serviceProvider.GetService<IScaffoldingModelFactory>();
            var _selector = serviceProvider.GetService<IModelCodeGeneratorSelector>();
            var cache = new DbModelCacheProvider();

            var databaseModel = cache.GetModelFromFileCache(_databaseModelFactory, connectionString, databaseOptions); 

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

            return codeGenerator.GenerateModel(model, codeOptions);
        }

        private static SavedModelFiles CreateCleanupPaths(SavedModelFiles procedurePaths, SavedModelFiles functionPaths, SavedModelFiles filePaths)
        {
            var cleanUpPaths = new SavedModelFiles(filePaths.ContextFile, filePaths.AdditionalFiles);

            if (procedurePaths != null)
            {
                cleanUpPaths.AdditionalFiles.Add(procedurePaths.ContextFile);
                foreach (var additionalFile in procedurePaths.AdditionalFiles)
                {
                    cleanUpPaths.AdditionalFiles.Add(additionalFile);
                }
            }

            if (functionPaths != null)
            {
                cleanUpPaths.AdditionalFiles.Add(functionPaths.ContextFile);
            }

            return cleanUpPaths;
        }

        private static List<string> SplitDbContext(string contextFile, bool useDbContextSplitting, string contextNamespace)
        {
            if (!useDbContextSplitting)
            {
                return new List<string>();
            }

            return DbContextSplitter.Split(contextFile, contextNamespace);
        }

        private static void RemoveOnConfiguring(string contextFile, bool includeConnectionString)
        {
            var finalLines = new List<string>();
            var lines = File.ReadAllLines(contextFile);

            int i = 1;
            var inConfiguring = false;

            foreach (var line in lines)
            {
                if (!includeConnectionString)
                {
                    if (line.Trim().Contains("protected override void OnConfiguring"))
                    {
                        inConfiguring = true;
                        continue;
                    }

                    if (inConfiguring && line.Trim() != string.Empty)
                    {
                        continue;
                    }

                    if (inConfiguring && line.Trim() == string.Empty)
                    {
                        inConfiguring = false;
                        continue;
                    }
                }

                finalLines.Add(line);
                i++;
            }
            File.WriteAllLines(contextFile, finalLines, Encoding.UTF8);
        }

        private static void PostProcess(string file)
        {
            var text = File.ReadAllText(file, Encoding.UTF8);
            File.WriteAllText(file, PathHelper.Header 
                + Environment.NewLine 
                + text.Replace(";Command Timeout=300", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(";Trust Server Certificate=True", string.Empty, StringComparison.OrdinalIgnoreCase)
                .TrimEnd(), Encoding.UTF8)
                ;
        }

        private static void CleanUp(SavedModelFiles filePaths, List<string> entityTypeConfigurationPaths)
        {
            var contextFolderFiles = Directory.GetFiles(Path.GetDirectoryName(filePaths.ContextFile), "*.cs");

            var allGeneratedFiles = filePaths.AdditionalFiles.Select(s => Path.GetFullPath(s)).Concat(new List<string> { Path.GetFullPath(filePaths.ContextFile) }).Concat(entityTypeConfigurationPaths).ToList();

            foreach (var contextFolderFile in contextFolderFiles)
            {
                if (allGeneratedFiles.Contains(contextFolderFile, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                TryRemoveFile(contextFolderFile);
            }

            if (filePaths.AdditionalFiles.Count == 0)
                return;

            foreach (var modelFile in Directory.GetFiles(Path.GetDirectoryName(filePaths.AdditionalFiles.First()), "*.cs"))
            {
                if (allGeneratedFiles.Contains(modelFile, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                TryRemoveFile(modelFile);
            }
        }

        private static void TryRemoveFile(string codeFile)
        {
            string firstLine;
            using (var reader = new StreamReader(codeFile, Encoding.UTF8))
            {
                firstLine = reader.ReadLine() ?? "";
            }

            if (firstLine == PathHelper.Header)
            {
                try
                {
                    File.Delete(codeFile);
                }
                catch { }
            }
        }
    }
}