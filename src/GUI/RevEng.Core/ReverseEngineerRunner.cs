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
#if CORE60
#else

            foreach (var table in options.Tables)
            {
                table.Name = table.Name.Replace("'", "''");
            }
#endif
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
#if CORE50 || CORE60
                if (functionPaths != null)
                {
                    var dbContextLines = File.ReadAllLines(filePaths.ContextFile).ToList();
                    var index = dbContextLines.IndexOf("            OnModelCreatingPartial(modelBuilder);");
                    if (index != -1)
                    {
                        dbContextLines.Insert(index, "            OnModelCreatingGeneratedFunctions(modelBuilder);");
                        File.WriteAllLines(filePaths.ContextFile, dbContextLines);
                    }
                }
#endif
                RemoveFragments(filePaths.ContextFile, options.ContextClassName, options.IncludeConnectionString, options.UseNoDefaultConstructor);
                if (!options.UseHandleBars)
                {
                    PostProcess(filePaths.ContextFile, options.UseNullableReferences);
                }

                entityTypeConfigurationPaths = SplitDbContext(filePaths.ContextFile, options.UseDbContextSplitting, contextNamespace, options.UseNullableReferences);
            }
            else if (options.SelectedToBeGenerated == 2
                && (options.Tables.Count(t => t.ObjectType == ObjectType.Procedure) > 0
                || options.Tables.Count(t => t.ObjectType == ObjectType.ScalarFunction) > 0))
            {
                warnings.Add("Selected stored procedures/scalar functions will not be generated, as 'Entity Types only' was selected");
            }

            if (!options.UseHandleBars)
            {
                foreach (var file in filePaths.AdditionalFiles)
                {
                    PostProcess(file, options.UseNullableReferences);
                }
            }

            if (options.RunCleanup)
            {
                var cleanUpPaths = CreateCleanupPaths(procedurePaths, functionPaths, filePaths);

                CleanUp(cleanUpPaths, entityTypeConfigurationPaths, outputDir);
            }

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
                foreach (var additionalFile in functionPaths.AdditionalFiles)
                {
                    cleanUpPaths.AdditionalFiles.Add(additionalFile);
                }
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

        private static void RemoveFragments(string contextFile, string contextName, bool includeConnectionString, bool removeDefaultConstructor)
        {
            if (string.IsNullOrEmpty(contextFile))
            {
                return;
            }

            var finalLines = new List<string>();
            var lines = File.ReadAllLines(contextFile).ToList();

            if (removeDefaultConstructor)
            {
                var index = lines.IndexOf($"        public {contextName}()");
                if (index != -1)
                {
                    lines.RemoveAt(index + 3);
                    lines.RemoveAt(index + 2);
                    lines.RemoveAt(index + 1);
                    lines.RemoveAt(index);
                }
            }

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

        private static void PostProcess(string file, bool useNullable)
        {
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            var header = PathHelper.Header;

            if (useNullable)
            {
                header = $"{header}{Environment.NewLine}#nullable enable";
            }
            else
            {
                header = $"{header}{Environment.NewLine}#nullable disable";
            }

            var text = File.ReadAllText(file, Encoding.UTF8);
            File.WriteAllText(file, header
                + Environment.NewLine
                + text.Replace(";Command Timeout=300", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(";Trust Server Certificate=True", string.Empty, StringComparison.OrdinalIgnoreCase)
                .TrimEnd(), Encoding.UTF8)
                ;
        }

        private static void CleanUp(SavedModelFiles filePaths, List<string> entityTypeConfigurationPaths, string outputDir)
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

            var allowedFolders = allGeneratedFiles.Select(s => Path.GetDirectoryName(s)).Distinct().ToHashSet();

            foreach (var modelFile in Directory.GetFiles(outputDir, "*.cs", SearchOption.AllDirectories))
            {
                if (allGeneratedFiles.Contains(modelFile, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (allowedFolders.Contains(Path.GetDirectoryName(modelFile)))
                {
                    TryRemoveFile(modelFile);
                }
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
