using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReverseEngineer20
{
    public class EfCoreReverseEngineer
    {
        public ReverseEngineerResult GenerateFiles(ReverseEngineerOptions reverseEngineerOptions, bool includeViews)
        {
            if (includeViews)
            {
                return LaunchExternalRunner(reverseEngineerOptions);
            }

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
               ? Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath))
               : reverseEngineerOptions.ProjectPath;

            var outputContextDir = !string.IsNullOrEmpty(reverseEngineerOptions.OutputContextPath)
                ? Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputContextPath))
                : outputDir;

            var modelNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ModelNamespace)
                ? reverseEngineerOptions.ProjectRootNamespace + "." + reverseEngineerOptions.ModelNamespace
                : PathHelper.GetNamespaceFromOutputPath(outputDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);

            var contextNamespace = !string.IsNullOrEmpty(reverseEngineerOptions.ContextNamespace)
                ? reverseEngineerOptions.ProjectRootNamespace + "." + reverseEngineerOptions.ContextNamespace
                : PathHelper.GetNamespaceFromOutputPath(outputContextDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);

            var modelOptions = new ModelReverseEngineerOptions
            {
                UseDatabaseNames = reverseEngineerOptions.UseDatabaseNames
            };

            var codeOptions = new ModelCodeGenerationOptions
            {
                UseDataAnnotations = !reverseEngineerOptions.UseFluentApiOnly
            };

            var scaffoldedModel = scaffolder.ScaffoldModel(
                    reverseEngineerOptions.Dacpac != null
                        ? reverseEngineerOptions.Dacpac
                        : reverseEngineerOptions.ConnectionString,
                    reverseEngineerOptions.Tables.Select(m => m.Name).ToArray(),
                    schemas,
                    modelNamespace,
                    "C#",
                    outputContextDir,
                    reverseEngineerOptions.ContextClassName,
                    modelOptions,
                    codeOptions);

            var filePaths = scaffolder.Save(
                scaffoldedModel,
                outputDir,
                overwriteFiles: true);

            PostProcessContext(filePaths.ContextFile, reverseEngineerOptions, modelNamespace, contextNamespace);

            foreach (var file in filePaths.AdditionalFiles)
            {
                PostProcess(file, reverseEngineerOptions.IdReplace);
            }
            
            PostProcess(filePaths.ContextFile, reverseEngineerOptions.IdReplace);

            var entityTypeConfigurationPaths = SplitDbContext(filePaths.ContextFile, reverseEngineerOptions.UseDbContextSplitting, contextNamespace);

            CleanUp(filePaths, entityTypeConfigurationPaths);

            var result = new ReverseEngineerResult
            {
                EntityErrors = errors,
                EntityWarnings = warnings,
                EntityTypeFilePaths = filePaths.AdditionalFiles,
                ContextFilePath = filePaths.ContextFile,
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

        private ReverseEngineerResult LaunchExternalRunner(ReverseEngineerOptions options)
        {
            var commandOptions = new ReverseEngineerCommandOptions
            {
                ConnectionString = options.ConnectionString,
                ContextClassName = options.ContextClassName,
                CustomReplacers = options.CustomReplacers,
                Dacpac = options.Dacpac,
                DatabaseType = options.DatabaseType,
                DefaultDacpacSchema = options.DefaultDacpacSchema,
                DoNotCombineNamespace = options.DoNotCombineNamespace,
                IdReplace = options.IdReplace,
                IncludeConnectionString = options.IncludeConnectionString,
                OutputPath = options.OutputPath,
                ContextNamespace = options.ContextNamespace,
                ModelNamespace = options.ModelNamespace,
                OutputContextPath = options.OutputContextPath,
                ProjectPath = options.ProjectPath,
                ProjectRootNamespace = options.ProjectRootNamespace,
                SelectedHandlebarsLanguage = options.SelectedHandlebarsLanguage,
                SelectedToBeGenerated = options.SelectedToBeGenerated,
                Tables = options.Tables,
                UseDatabaseNames = options.UseDatabaseNames,
                UseFluentApiOnly = options.UseFluentApiOnly,
                UseHandleBars = options.UseHandleBars,
                UseInflector = options.UseInflector,
                UseLegacyPluralizer = options.UseLegacyPluralizer,
                UseSpatial = options.UseSpatial,
                UseDbContextSplitting = options.UseDbContextSplitting,
                UseNodaTime = options.UseNodaTime,
            };

            var launcher = new EfRevEngLauncher(commandOptions);
            return launcher.GetOutput();
        }

        private void PostProcessContext(string contextFile, ReverseEngineerOptions options, string modelNamespace, string contextNamespace)
        {
            var finalLines = new List<string>();
            var lines = File.ReadAllLines(contextFile);

            int i = 1;
            var inModelCreating = false;
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

                if (modelNamespace != contextNamespace)
                {
                    if (line.Contains("using System;"))
                    {
                        finalLines.Add("using " + modelNamespace + ";");
                    }
                    if (line.Contains("namespace"))
                    {
                        finalLines.Add("namespace " + contextNamespace);
                        continue;
                    }
                }

                if (line.Contains("OnModelCreating")) inModelCreating = true;

                if (!options.UseHandleBars && inModelCreating && line.StartsWith("        }"))
                {
                    finalLines.Add(string.Empty);
                    finalLines.Add("            OnModelCreatingPartial(modelBuilder);");
                }

                if (!options.UseHandleBars && inModelCreating && line.StartsWith("    }"))
                {
                    finalLines.Add(string.Empty);
                    finalLines.Add("        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);");
                }

                finalLines.Add(line);
                i++;
            }
            File.WriteAllLines(contextFile, finalLines, Encoding.UTF8);
        }

        private void PostProcess(string file, bool idReplace)
        {
            var text = File.ReadAllText(file, Encoding.UTF8);
            if (idReplace)
            {
                text = text.Replace("Id, ", "ID, ");
                text = text.Replace("Id }", "ID }");
                text = text.Replace("Id }", "ID }");
                text = text.Replace("Id)", "ID)");
                text = text.Replace("Id { get; set; }", "ID { get; set; }");
            }
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
