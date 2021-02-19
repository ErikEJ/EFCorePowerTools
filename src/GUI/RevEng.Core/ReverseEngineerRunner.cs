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
        public static ReverseEngineerResult GenerateFiles(ReverseEngineerCommandOptions reverseEngineerOptions)
        {
            var errors = new List<string>();
            var warnings = new List<string>();
            var reporter = new OperationReporter(
                new OperationReportHandler(
                    m => errors.Add(m),
                    m => warnings.Add(m)));
            var serviceProvider = ServiceProviderBuilder.Build(reverseEngineerOptions);
            var scaffolder = serviceProvider.GetService<IReverseEngineerScaffolder>();
            var schemas = new List<string>();

            reverseEngineerOptions.ConnectionString = SqlServerHelper.SetConnectionString(reverseEngineerOptions.DatabaseType, reverseEngineerOptions.ConnectionString); 

            if (reverseEngineerOptions.DefaultDacpacSchema != null)
            {
                schemas.Add(reverseEngineerOptions.DefaultDacpacSchema);
            }

            if (reverseEngineerOptions.FilterSchemas)
            {
                schemas.AddRange(reverseEngineerOptions.Schemas.Select(s => s.Name));
            }

            var outputDir = !string.IsNullOrEmpty(reverseEngineerOptions.OutputPath)
               ? Path.IsPathFullyQualified(reverseEngineerOptions.OutputPath)
                    ? reverseEngineerOptions.OutputPath
                    : Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath))
                : reverseEngineerOptions.ProjectPath;

            var outputContextDir = !string.IsNullOrEmpty(reverseEngineerOptions.OutputContextPath)
               ? Path.IsPathFullyQualified(reverseEngineerOptions.OutputContextPath)
                    ? reverseEngineerOptions.OutputContextPath
                    : Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputContextPath))
                : outputDir;

            var modelNamespace = string.Empty;
            var contextNamespace = string.Empty;

            if (string.IsNullOrEmpty(reverseEngineerOptions.ProjectRootNamespace))
            {
                modelNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ModelNamespace)
                    ? reverseEngineerOptions.ModelNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);

                contextNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ContextNamespace)
                    ? reverseEngineerOptions.ContextNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputContextDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);
            }
            else
            {
                modelNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ModelNamespace)
                    ? reverseEngineerOptions.ProjectRootNamespace + "." + reverseEngineerOptions.ModelNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);

                contextNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ContextNamespace)
                    ? reverseEngineerOptions.ProjectRootNamespace + "." + reverseEngineerOptions.ContextNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputContextDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);
            }

            SavedModelFiles procedurePaths = null;
            var procedureModelScaffolder = serviceProvider.GetService<IProcedureScaffolder>();
            if (procedureModelScaffolder != null
                && reverseEngineerOptions.Tables.Any(t => t.ObjectType == ObjectType.Procedure))
            {
                var procedureModelFactory = serviceProvider.GetService<IProcedureModelFactory>();

                var procedureModelFactoryOptions = new ModuleModelFactoryOptions
                {
                    FullModel = true,
                    Modules = reverseEngineerOptions.Tables.Where(t => t.ObjectType == ObjectType.Procedure).Select(m => m.Name),
                };

                var procedureModel = procedureModelFactory.Create(reverseEngineerOptions.Dacpac ?? reverseEngineerOptions.ConnectionString, procedureModelFactoryOptions);

                var procedureOptions = new ModuleScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = reverseEngineerOptions.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                    NullableReferences = reverseEngineerOptions.UseNullableReferences,
                };

                var procedureScaffoldedModel = procedureModelScaffolder.ScaffoldModel(procedureModel, procedureOptions, ref errors);

                if (procedureScaffoldedModel != null)
                {
                    procedurePaths = procedureModelScaffolder.Save(
                        procedureScaffoldedModel,
                        Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath ?? string.Empty)),
                        contextNamespace);
                }
            }

            SavedModelFiles functionPaths = null;
            var functionModelScaffolder = serviceProvider.GetService<IFunctionScaffolder>();
            if (functionModelScaffolder != null
                && reverseEngineerOptions.Tables.Any(t => t.ObjectType == ObjectType.ScalarFunction))
            {
                var functionModelFactory = serviceProvider.GetService<IFunctionModelFactory>();

                var modelFactoryOptions = new ModuleModelFactoryOptions
                {
                    FullModel = true,
                    Modules = reverseEngineerOptions.Tables.Where(t => t.ObjectType == ObjectType.ScalarFunction).Select(m => m.Name),
                };

                var functionModel = functionModelFactory.Create(reverseEngineerOptions.Dacpac ?? reverseEngineerOptions.ConnectionString, modelFactoryOptions);

                var functionOptions = new ModuleScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = reverseEngineerOptions.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                    NullableReferences = reverseEngineerOptions.UseNullableReferences,
                };

                var functionScaffoldedModel = functionModelScaffolder.ScaffoldModel(functionModel, functionOptions, ref errors);

                if (functionScaffoldedModel != null)
                {
                    functionPaths = functionModelScaffolder.Save(
                        functionScaffoldedModel,
                        Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath ?? string.Empty)),
                        contextNamespace);
                }
            }

            var modelOptions = new ModelReverseEngineerOptions
            {
                UseDatabaseNames = reverseEngineerOptions.UseDatabaseNames,
#if CORE50
                NoPluralize = !reverseEngineerOptions.UseInflector,
#endif
            };

            var codeOptions = new ModelCodeGenerationOptions
            {
                UseDataAnnotations = !reverseEngineerOptions.UseFluentApiOnly,
                Language = "C#",
                ContextName = reverseEngineerOptions.ContextClassName,
                ContextDir = outputContextDir,
                RootNamespace = null,
                ContextNamespace = contextNamespace,
                ModelNamespace = modelNamespace,
                SuppressConnectionStringWarning = false,
                ConnectionString = reverseEngineerOptions.ConnectionString,
#if CORE50
                SuppressOnConfiguring = !reverseEngineerOptions.IncludeConnectionString,
#endif
            };

            var dbOptions = new DatabaseModelFactoryOptions(reverseEngineerOptions.Tables.Where(t => t.ObjectType.HasColumns()).Select(m => m.Name), schemas);

            var scaffoldedModel = ScaffoldModel(
                    reverseEngineerOptions.Dacpac ?? reverseEngineerOptions.ConnectionString,
                    dbOptions,
                    modelOptions,
                    codeOptions,
                    reverseEngineerOptions.UseBoolPropertiesWithoutDefaultSql,
                    reverseEngineerOptions.UseNoNavigations,
                    serviceProvider);

            var filePaths = scaffolder.Save(
                scaffoldedModel,
                Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath ?? string.Empty)),
                overwriteFiles: true);

#if CORE50
#else
            RemoveOnConfiguring(filePaths.ContextFile, reverseEngineerOptions.IncludeConnectionString);
#endif
            foreach (var file in filePaths.AdditionalFiles)
            {
                PostProcess(file);
            }

            PostProcess(filePaths.ContextFile);

            var entityTypeConfigurationPaths = SplitDbContext(filePaths.ContextFile, reverseEngineerOptions.UseDbContextSplitting, contextNamespace);

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

            return codeGenerator.GenerateModel(model, codeOptions);
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