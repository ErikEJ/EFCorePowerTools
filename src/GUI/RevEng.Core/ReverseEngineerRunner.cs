using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace RevEng.Core
{
    public static class ReverseEngineerRunner
    {
        public static ReverseEngineerResult GenerateFiles(ReverseEngineerCommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var errors = new List<string>();
            var warnings = new List<string>();
            var info = new List<string>();
            var serviceProvider = ServiceProviderBuilder.Build(options, errors, warnings, info);
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
                table.Name = table.Name.Replace("'", "''", StringComparison.OrdinalIgnoreCase);
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

            var scaffolder = serviceProvider.GetService<IReverseEngineerScaffolder>();

            SavedModelFiles filePaths = scaffolder!.GenerateDbContext(options, schemas, outputContextDir, modelNamespace, contextNamespace, options.ProjectPath, options.OutputPath);

#if CORE70
            if (options.UseT4)
            {
                foreach (var paths in GetAlternateCodeTemplatePaths(options.ProjectPath))
                {
                    scaffolder!.GenerateDbContext(options, schemas, outputContextDir, modelNamespace, contextNamespace, paths.Path, paths.OutputPath);
                }
            }
#endif
            if (options.SelectedToBeGenerated != 2)
            {
                bool supportsRoutines = options.DatabaseType == DatabaseType.SQLServerDacpac || options.DatabaseType == DatabaseType.SQLServer;
                procedurePaths = scaffolder.GenerateStoredProcedures(options, ref warnings, outputContextDir, modelNamespace, contextNamespace, supportsRoutines);

#if CORE60
                bool supportsFunctions = options.DatabaseType == DatabaseType.SQLServer;
                functionPaths = scaffolder.GenerateFunctions(options, ref warnings, outputContextDir, modelNamespace, contextNamespace, supportsFunctions);
#endif
                if (functionPaths != null || procedurePaths != null)
                {
                    var dbContextLines = File.ReadAllLines(filePaths.ContextFile).ToList();
                    if (procedurePaths != null)
                    {
                        var index = dbContextLines.FindIndex(l => l.Contains("        OnModelCreatingPartial(modelBuilder);", StringComparison.Ordinal));
                        if (index != -1)
                        {
#if CORE70
                            dbContextLines.Insert(index, "        OnModelCreatingGeneratedProcedures(modelBuilder);");
#else
                            dbContextLines.Insert(index, "            OnModelCreatingGeneratedProcedures(modelBuilder);");
#endif
                        }
                    }

                    if (functionPaths != null)
                    {
                        var index = dbContextLines.FindIndex(l => l.Contains("        OnModelCreatingPartial(modelBuilder);", StringComparison.Ordinal));
                        if (index != -1)
                        {
#if CORE70
                            dbContextLines.Insert(index, "        OnModelCreatingGeneratedFunctions(modelBuilder);");
#else
                            dbContextLines.Insert(index, "            OnModelCreatingGeneratedFunctions(modelBuilder);");
#endif
                        }
                    }

                    RetryFileWrite(filePaths.ContextFile, dbContextLines);
                }

                RemoveFragments(filePaths.ContextFile, options.ContextClassName, options.IncludeConnectionString, options.UseNoDefaultConstructor);
                if (!options.UseHandleBars && !options.UseT4)
                {
                    PostProcess(filePaths.ContextFile, options.UseNullableReferences, !options.LegacyLangVersion);
                }

                entityTypeConfigurationPaths = SplitDbContext(filePaths.ContextFile, options.UseDbContextSplitting, contextNamespace, options.UseNullableReferences);
            }
            else if (options.Tables.Any(t => t.ObjectType == ObjectType.Procedure)
                || options.Tables.Any(t => t.ObjectType == ObjectType.ScalarFunction))
            {
                warnings.Add("Selected stored procedures/scalar functions will not be generated, as 'Entity Types only' was selected");
            }

            if (!options.UseHandleBars && !options.UseT4)
            {
                foreach (var file in filePaths.AdditionalFiles)
                {
                    PostProcess(file, options.UseNullableReferences, !options.LegacyLangVersion);
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

        public static void RetryFileWrite(string path, List<string> finalLines)
        {
            for (int i = 1; i <= 3; ++i)
            {
                try
                {
                    File.WriteAllLines(path, finalLines, Encoding.UTF8);
                    break;
                }
                catch (IOException) when (i <= 3)
                {
                    Thread.Sleep(500);
                }
            }
        }

        public static void RetryFileWrite(string path, string finalText)
        {
            for (int i = 1; i <= 3; ++i)
            {
                try
                {
                    File.WriteAllText(path, finalText, Encoding.UTF8);
                    break;
                }
                catch (IOException) when (i <= 3)
                {
                    Thread.Sleep(500);
                }
            }
        }

#if CORE70
        private static List<(string Path, string OutputPath)> GetAlternateCodeTemplatePaths(string projectPath)
        {
            var result = new List<(string Path, string OutputPath)>();

            if (Directory.Exists(projectPath))
            {
                foreach (var path in Directory.EnumerateDirectories(projectPath, "*", new EnumerationOptions { RecurseSubdirectories = true }))
                {
                    if (path == Path.Join(projectPath, "CodeTemplates", "EFCore"))
                    {
                        continue;
                    }

                    if (path.EndsWith(Path.Join("CodeTemplates", "EFCore"), StringComparison.InvariantCultureIgnoreCase)
                        && (File.Exists(Path.Join(path, "EntityType.T4"))
                        || File.Exists(Path.Join(path, "DbContext.T4"))))
                    {
                        var outputRoot = path.Replace(Path.Join(Path.DirectorySeparatorChar.ToString(), "CodeTemplates", "EFCore"), string.Empty, StringComparison.InvariantCultureIgnoreCase);
                        var output = outputRoot.Replace(Path.Join(projectPath, Path.DirectorySeparatorChar.ToString()), string.Empty, StringComparison.InvariantCultureIgnoreCase);
                        result.Add((outputRoot, output));
                    }
                }
            }

            return result;
        }
#endif

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
                var index = lines.IndexOf($"    public {contextName}()");

                if (index == -1)
                {
                    index = lines.IndexOf($"        public {contextName}()");
                }

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
                    if (line.Trim().Contains("protected override void OnConfiguring", StringComparison.OrdinalIgnoreCase))
                    {
                        inConfiguring = true;
                        continue;
                    }

                    if (inConfiguring && !string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    if (inConfiguring)
                    {
                        inConfiguring = false;
                        continue;
                    }
                }

                finalLines.Add(line);
                i++;
            }

            RetryFileWrite(contextFile, finalLines);
        }

        private static void PostProcess(string file, bool useNullable, bool supportsNullable)
        {
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            var header = PathHelper.Header;

            if (supportsNullable)
            {
                if (useNullable)
                {
                    header = $"{header}{Environment.NewLine}#nullable enable";
                }
                else
                {
                    header = $"{header}{Environment.NewLine}#nullable disable";
                }
            }

            var text = File.ReadAllText(file, Encoding.UTF8);

            RetryFileWrite(
                file,
                header + Environment.NewLine + text.Replace(";Command Timeout=300", string.Empty, StringComparison.OrdinalIgnoreCase).Replace(";Trust Server Certificate=True", string.Empty, StringComparison.OrdinalIgnoreCase).TrimEnd());
        }

        private static void CleanUp(SavedModelFiles filePaths, List<string> entityTypeConfigurationPaths, string outputDir)
        {
            var allGeneratedFiles = filePaths.AdditionalFiles.Select(s => Path.GetFullPath(s)).Concat(entityTypeConfigurationPaths).ToHashSet();

            if (!string.IsNullOrEmpty(filePaths.ContextFile))
            {
                var contextFolderFiles = Directory.GetFiles(Path.GetDirectoryName(filePaths.ContextFile) ?? string.Empty, "*.cs");

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
            {
                return;
            }

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
                firstLine = reader.ReadLine() ?? string.Empty;
            }

            if (firstLine == PathHelper.Header)
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    if (File.Exists(codeFile))
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                            codeFile,
                            Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                }
                catch
                {
                    // Ignore
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }
    }
}
