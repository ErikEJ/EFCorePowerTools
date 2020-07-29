﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Core.Procedures.Scaffolding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReverseEngineer20.ReverseEngineer
{
    public class ReverseEngineerRunner
    {
        public ReverseEngineerResult GenerateFiles(ReverseEngineerCommandOptions reverseEngineerOptions)
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
            if (reverseEngineerOptions.DefaultDacpacSchema != null)
            {
                schemas.Add(reverseEngineerOptions.DefaultDacpacSchema);
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

            var modelNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ModelNamespace)
                ? reverseEngineerOptions.ProjectRootNamespace + "." + reverseEngineerOptions.ModelNamespace
                : PathHelper.GetNamespaceFromOutputPath(outputDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);

            var contextNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ContextNamespace)
                ? reverseEngineerOptions.ProjectRootNamespace + "." + reverseEngineerOptions.ContextNamespace
                : PathHelper.GetNamespaceFromOutputPath(outputContextDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);

            var modelOptions = new ModelReverseEngineerOptions
            {
                UseDatabaseNames = reverseEngineerOptions.UseDatabaseNames,
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
            };

            SavedModelFiles procedurePaths = null;
            var procedureModelScaffolder = serviceProvider.GetService<IProcedureScaffolder>();
            if (procedureModelScaffolder != null)
            {
                var procedureOptions = new ProcedureScaffolderOptions
                {
                    ContextDir = outputContextDir,
                    ContextName = reverseEngineerOptions.ContextClassName,
                    ContextNamespace = contextNamespace,
                    ModelNamespace = modelNamespace,
                };

                var procedureModel = procedureModelScaffolder.ScaffoldModel(reverseEngineerOptions.ConnectionString, procedureOptions);
                procedurePaths = procedureModelScaffolder.Save(
                    procedureModel,
                    Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath ?? string.Empty)));
            }
            var dbOptions = new DatabaseModelFactoryOptions(reverseEngineerOptions.Tables.Select(m => m.Name), schemas);

            var scaffoldedModel = scaffolder.ScaffoldModel(
                    reverseEngineerOptions.Dacpac ?? reverseEngineerOptions.ConnectionString,
                    dbOptions,
                    modelOptions,
                    codeOptions);

            var filePaths = scaffolder.Save(
                scaffoldedModel,
                Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath ?? string.Empty)),
                overwriteFiles: true);

            PostProcessContext(filePaths.ContextFile, reverseEngineerOptions);

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

        private List<string> SplitDbContext(string contextFile, bool useDbContextSplitting, string contextNamespace)
        {
            if (!useDbContextSplitting)
            {
                return new List<string>();
            }

            return DbContextSplitter.Split(contextFile, contextNamespace);
        }

        private void PostProcessContext(string contextFile, ReverseEngineerCommandOptions options)
        {
            var finalLines = new List<string>();
            var lines = File.ReadAllLines(contextFile);

            int i = 1;
            var inConfiguring = false;

            foreach (var line in lines)
            {
                if (!options.IncludeConnectionString)
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

        private void PostProcess(string file)
        {
            var text = File.ReadAllText(file, Encoding.UTF8);
            File.WriteAllText(file, PathHelper.Header + Environment.NewLine + text.TrimEnd(), Encoding.UTF8);
        }

        private void CleanUp(SavedModelFiles filePaths, List<string> entityTypeConfigurationPaths)
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