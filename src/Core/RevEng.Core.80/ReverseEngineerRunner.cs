using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;

namespace RevEng.Core
{
    public static class ReverseEngineerRunner
    {
        public static ReverseEngineerResult GenerateFiles(ReverseEngineerCommandOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            var errors = new List<string>();
            var warnings = new List<string>();
            var info = new List<string>();
            var serviceProvider = new ServiceCollection().AddEfpt(options, errors, warnings, info).BuildServiceProvider();
            var schemas = new List<string>();

            options.ConnectionString = options.ConnectionString.ApplyDatabaseType(options.DatabaseType);

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
                var code = serviceProvider.GetRequiredService<ICSharpHelper>();
                options.ProjectRootNamespace = GenerateNameSpace(options.ProjectRootNamespace, code);

                modelNamespace = !string.IsNullOrEmpty(options.ModelNamespace)
                    ? options.ProjectRootNamespace + "." + options.ModelNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputDir, options.ProjectPath, options.ProjectRootNamespace);

                contextNamespace = !string.IsNullOrEmpty(options.ContextNamespace)
                    ? options.ProjectRootNamespace + "." + options.ContextNamespace
                    : PathHelper.GetNamespaceFromOutputPath(outputContextDir, options.ProjectPath, options.ProjectRootNamespace);
            }

            ValidateOptions(options, warnings);

            var entityTypeConfigurationPaths = new List<string>();
            SavedModelFiles procedurePaths = null;
            SavedModelFiles functionPaths = null;

            var scaffolder = serviceProvider.GetService<IReverseEngineerScaffolder>();

            try
            {
                SavedModelFiles filePaths = scaffolder!.GenerateDbContext(options, schemas, outputContextDir, modelNamespace, contextNamespace, options.ProjectPath, options.OutputPath, options.ProjectRootNamespace);

                if (options.UseT4 || options.UseT4Split)
                {
                    foreach (var paths in GetAlternateCodeTemplatePaths(options.ProjectPath))
                    {
                        scaffolder!.GenerateDbContext(options, schemas, paths.Path, modelNamespace, contextNamespace, paths.Path, paths.OutputPath, options.ProjectRootNamespace);
                    }
                }

                if (options.SelectedToBeGenerated != 2)
                {
                    bool supportsProcedures = options.DatabaseType == DatabaseType.SQLServerDacpac
                        || options.DatabaseType == DatabaseType.SQLServer
                        || options.DatabaseType == DatabaseType.Npgsql;
                    procedurePaths = scaffolder.GenerateStoredProcedures(options, schemas, ref warnings, outputContextDir, modelNamespace, contextNamespace, supportsProcedures);

                    bool supportsFunctions = options.DatabaseType == DatabaseType.SQLServer
                        || options.DatabaseType == DatabaseType.SQLServerDacpac;
                    functionPaths = scaffolder.GenerateFunctions(options, schemas, ref warnings, outputContextDir, modelNamespace, contextNamespace, supportsFunctions);

                    if (functionPaths != null || procedurePaths != null)
                    {
                        var dbContextLines = File.ReadAllLines(filePaths.ContextFile).ToList();

                        if (functionPaths != null)
                        {
                            var index = dbContextLines.FindIndex(l => l.Contains("        OnModelCreatingPartial(modelBuilder);", StringComparison.Ordinal));
                            if (index != -1)
                            {
                                dbContextLines.Insert(index, "        OnModelCreatingGeneratedFunctions(modelBuilder);");
                            }
                        }

                        RetryFileWrite(filePaths.ContextFile, dbContextLines);
                    }

                    RemoveFragments(filePaths.ContextFile, options.ContextClassName, options.IncludeConnectionString, options.UseNoDefaultConstructor);
                    if (!options.UseHandleBars && !options.UseT4 && !options.UseT4Split)
                    {
                        PostProcess(filePaths.ContextFile, options.UseNullableReferences);
                    }

                    entityTypeConfigurationPaths = SplitDbContext(filePaths.ContextFile, options.UseDbContextSplitting, contextNamespace, options.UseNullableReferences, options.ContextClassName);

                    if (options.UseT4Split)
                    {
                        entityTypeConfigurationPaths.AddRange(MoveConfigurationFiles(filePaths.AdditionalFiles));
                    }
                }
                else if (options.Tables.Exists(t => t.ObjectType == ObjectType.Procedure)
                    || options.Tables.Exists(t => t.ObjectType == ObjectType.ScalarFunction))
                {
                    warnings.Add("Selected stored procedures/scalar functions will not be generated, as 'Entity Types only' was selected");
                }

                if (!options.UseHandleBars && !options.UseT4 && !options.UseT4Split)
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

                var sku = SkuHelper.GetSkuInfo(options.ConnectionString, options.DatabaseType);

                if ((options.DatabaseType == DatabaseType.SQLServer)
                    && sku.Version > 12 && sku.Level > 0 && sku.Level < 130)
                {
                    warnings.Add($"Your database compatibility level is only '{sku.Level}', consider updating to 130 or higher to take full advantage of new database engine features.");
                }

                var result = new ReverseEngineerResult
                {
                    EntityErrors = errors,
                    EntityWarnings = warnings,
                    EntityTypeFilePaths = allfiles,
                    ContextFilePath = filePaths.ContextFile,
                    ContextConfigurationFilePaths = entityTypeConfigurationPaths,
                    DatabaseEdition = sku.Edition,
                    DatabaseVersion = sku.Version,
                    DatabaseLevel = sku.Level,
                    DatabaseEditionId = sku.EditionId,
                };

                return result;
            }
#pragma warning disable CA1031
            catch (Exception ex)
            {
                errors.Add(ex.ToString());

                var result = new ReverseEngineerResult
                {
                    EntityErrors = errors,
                    EntityWarnings = warnings,
                    EntityTypeFilePaths = Array.Empty<string>(),
                    ContextFilePath = string.Empty,
                    ContextConfigurationFilePaths = Array.Empty<string>(),
                };

                return result;
            }
#pragma warning restore CA1031
        }

        private static string GenerateNameSpace(string projectNamespace, ICSharpHelper code)
        {
            var parts = projectNamespace.Split('.');

            foreach (var part in parts)
            {
                if (char.IsUpper(part[0]))
                {
                    parts[Array.IndexOf(parts, part)] = code.Identifier(part, capitalize: true);
                }
                else
                {
                    parts[Array.IndexOf(parts, part)] = code.Identifier(part, capitalize: false);
                }
            }

            return string.Join(".", parts);
        }

        public static void RetryFileWrite(string path, List<string> finalLines)
        {
            for (int i = 1; i <= 4; ++i)
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
            for (int i = 1; i <= 4; ++i)
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

        private static void ValidateOptions(ReverseEngineerCommandOptions options, List<string> warnings)
        {
            if (options.UseDatabaseNames && options.CustomReplacers?.Count > 0)
            {
                warnings.Add($"'use-database-names' / 'UseDatabaseNames' has been set to true, but a '{Constants.RenamingFileName}' file was also found. This prevents '{Constants.RenamingFileName}' from functioning.");
            }

            if (options.UseT4 && options.UseT4Split)
            {
                warnings.Add("Both UseT4 and UseT4Split are set to true.  Only one of these should be used, UseT4Split will be used.");
                options.UseT4 = false;
            }

            if (options.UseDbContextSplitting)
            {
                warnings.Add("UseDbContextSplitting (preview) is obsolete, please switch to the T4 split DbContext template.");
            }

            if (options.UseT4Split && options.UseDbContextSplitting)
            {
                warnings.Add("Both UseDbContextSplitting (preview) and UseT4Split are set to true. Only one of these should be used, UseT4Split will be used.");
                options.UseDbContextSplitting = false;
            }

            if (options.UseInternalAccessModifiersForSprocsAndFunctions && !(options.UseT4 || options.UseT4Split))
            {
                warnings.Add("UseInternalAccessModifiersForSprocsAndFunctions is set to true, but UseT4 or UseT4Split are not true.  This will result in conflicting access modifiers for the partial DBContext class.  This option is intended for when the T4 templates have been used and are setting the DBContext class to internal rather than public.  UseInternalAccessModifiersForSprocsAndFunctions will be reset to false to ensure a valid DBContext is generated.");
                options.UseInternalAccessModifiersForSprocsAndFunctions = false;
            }
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

        private static List<string> SplitDbContext(string contextFile, bool useDbContextSplitting, string contextNamespace, bool supportNullable, string dbContextName)
        {
            if (!useDbContextSplitting)
            {
                return new List<string>();
            }

            return DbContextSplitter.Split(contextFile, contextNamespace, supportNullable, dbContextName);
        }

        // If we didn't split, we might have used EntityTypeConfiguration.t4.  In that case, <ModelName>Configuration.cs files were generated.
        private static List<string> MoveConfigurationFiles(IList<string> files)
        {
            var configurationFiles = files.Where(x => x.EndsWith("Configuration.cs", StringComparison.InvariantCulture)).ToList();

            var movedFiles = new List<string>();
            foreach (var configurationFile in configurationFiles)
            {
                // This handles the scenario where an Entity ends with the word "Configuration" such as "SoftwareConfiguration".  In this scenario
                // there would be 2 files "SoftwareConfiguration.cs" and "SoftwareConfigurationConfiguration.cs".  The former is the entity model itself
                // which should not be moved.  We're also checking to make sure the line isn't part of a comment block. (to handle a scenario where someone
                // has customized the EntityType.t4 to include a comment block that points to the EntityTypeConfiguration file)
                bool isConfigFile = false;
                foreach (var line in File.ReadLines(configurationFile))
                {
                    if (line.Contains("EntityTypeConfiguration<", StringComparison.Ordinal)
                        && !(line.Trim().StartsWith('/') || line.Trim().StartsWith('*')))
                    {
                        isConfigFile = true;
                        break;
                    }
                }

                if (!isConfigFile)
                {
                    continue;
                }

                var newDirectoryName = Path.Combine(Path.GetDirectoryName(configurationFile) ?? string.Empty, "Configurations");
                if (newDirectoryName is null)
                {
                    throw new InvalidOperationException("Could not determine directory name for configuration file.");
                }

                if (!Directory.Exists(newDirectoryName))
                {
                    Directory.CreateDirectory(newDirectoryName);
                }

                var newFileName = Path.Combine(newDirectoryName, Path.GetFileName(configurationFile));

                File.Move(configurationFile, newFileName, overwrite: true);
                movedFiles.Add(newFileName);
            }

            return movedFiles;
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