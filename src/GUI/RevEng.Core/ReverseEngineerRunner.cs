using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
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

            var entityTypeConfigurationPaths = new List<string>();
            SavedModelFiles procedurePaths = null;
            SavedModelFiles functionPaths = null;
            
            SavedModelFiles filePaths = ReverseEngineerScaffolder.GenerateDbContext(options, serviceProvider, schemas, outputContextDir, modelNamespace, contextNamespace);

            if (options.SelectedToBeGenerated != 2)
            {
                procedurePaths = ReverseEngineerScaffolder.GenerateStoredProcedures(options, ref errors, serviceProvider, outputContextDir, modelNamespace, contextNamespace);

                functionPaths = ReverseEngineerScaffolder.GenerateFunctions(options, ref errors, serviceProvider, outputContextDir, modelNamespace, contextNamespace);
#if CORE50
#else
                RemoveOnConfiguring(filePaths.ContextFile, options.IncludeConnectionString);
#endif
                PostProcess(filePaths.ContextFile);

                entityTypeConfigurationPaths = SplitDbContext(filePaths.ContextFile, options.UseDbContextSplitting, contextNamespace, options.UseNullableReferences);
            }

            foreach (var file in filePaths.AdditionalFiles)
            {
                PostProcess(file);
            }

            var cleanUpPaths = CreateCleanupPaths(procedurePaths, functionPaths, filePaths);

            CleanUp(cleanUpPaths, entityTypeConfigurationPaths);

            var allfiles = filePaths.AdditionalFiles.ToList();
            if (procedurePaths != null)
            {
                allfiles.AddRange(procedurePaths.AdditionalFiles);
                allfiles.Add(procedurePaths.ContextFile);
            }
            if (functionPaths != null)
            {
                allfiles.AddRange(functionPaths.AdditionalFiles);
                allfiles.Add(functionPaths.ContextFile);
            }

            var result = new ReverseEngineerResult
            {
                EntityErrors = errors,
                EntityWarnings = warnings,
                EntityTypeFilePaths = allfiles,
                ContextFilePath = filePaths.ContextFile,
                ContextConfigurationFilePaths = entityTypeConfigurationPaths,
            };

            return result;
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

        private static List<string> SplitDbContext(string contextFile, bool useDbContextSplitting, string contextNamespace, bool supportNullable)
        {
            if (!useDbContextSplitting)
            {
                return new List<string>();
            }

            return DbContextSplitter.Split(contextFile, contextNamespace, supportNullable);
        }

        private static void RemoveOnConfiguring(string contextFile, bool includeConnectionString)
        {
            if (string.IsNullOrEmpty(contextFile))
            {
                return;            
            }

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
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

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
            var allGeneratedFiles = filePaths.AdditionalFiles.Select(s => Path.GetFullPath(s)).Concat(entityTypeConfigurationPaths).ToHashSet();

            if (!string.IsNullOrEmpty(filePaths.ContextFile))
            {
                var contextFolderFiles = Directory.GetFiles(Path.GetDirectoryName(filePaths.ContextFile), "*.cs");

                allGeneratedFiles = filePaths.AdditionalFiles.Select(s => Path.GetFullPath(s)).Concat(new List<string> { Path.GetFullPath(filePaths.ContextFile) }).Concat(entityTypeConfigurationPaths).ToHashSet();

                foreach (var contextFolderFile in contextFolderFiles)
                {
                    if (allGeneratedFiles.Contains(contextFolderFile, StringComparer.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    TryRemoveFile(contextFolderFile);
                }
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