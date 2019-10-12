using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
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

            var outputDir = reverseEngineerOptions.OutputPath != null
               ? Path.IsPathFullyQualified(reverseEngineerOptions.OutputPath)
                    ? reverseEngineerOptions.OutputPath
                    : Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath))
                : reverseEngineerOptions.ProjectPath;

            var outputContextDir = reverseEngineerOptions.OutputContextPath != null
               ? Path.IsPathFullyQualified(reverseEngineerOptions.OutputContextPath)
                    ? reverseEngineerOptions.OutputContextPath
                    : Path.GetFullPath(Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputContextPath))
                : outputDir;

            var modelNamespace = reverseEngineerOptions.ModelNamespace != null
                ? reverseEngineerOptions.ProjectRootNamespace + "." + reverseEngineerOptions.ModelNamespace
                : PathHelper.GetNamespaceFromOutputPath(outputDir, reverseEngineerOptions.ProjectPath, reverseEngineerOptions.ProjectRootNamespace);

            var contextNamespace = reverseEngineerOptions.ContextNamespace != null
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

            var dbOptions = new DatabaseModelFactoryOptions(reverseEngineerOptions.Tables.Select(m => m.Name), schemas);

            var scaffoldedModel = scaffolder.ScaffoldModel(
                    reverseEngineerOptions.Dacpac ?? reverseEngineerOptions.ConnectionString,
                    dbOptions,
                    modelOptions,
                    codeOptions);

            var filePaths = scaffolder.Save(
                scaffoldedModel,
                Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath ?? string.Empty),
                overwriteFiles: true);

            string fixedNamespace = modelNamespace != contextNamespace
                ? modelNamespace
                : null;

            PostProcessContext(filePaths.ContextFile, reverseEngineerOptions, fixedNamespace);

            foreach (var file in filePaths.AdditionalFiles)
            {
                PostProcess(file);
            }

            PostProcess(filePaths.ContextFile);

            var result = new ReverseEngineerResult
            {
                EntityErrors = errors,
                EntityWarnings = warnings,
                EntityTypeFilePaths = filePaths.AdditionalFiles,
                ContextFilePath = filePaths.ContextFile,
            };

            return result;
        }

        private void PostProcessContext(string contextFile, ReverseEngineerCommandOptions options, string fixedNamespace)
        {
            var finalLines = new List<string>();
            var lines = File.ReadAllLines(contextFile);

            int i = 1;

            foreach (var line in lines)
            {
                if (fixedNamespace != null)
                {
                    if (line.Contains("using System;"))
                    {
                        finalLines.Add("using " + fixedNamespace + ";");
                    }
                }

                if (!options.IncludeConnectionString)
                {
                    if (line.Trim().StartsWith("#warning To protect"))
                        continue;

                    if (line.Trim().StartsWith("optionsBuilder.Use"))
                        continue;
                }

                finalLines.Add(line);
                i++;
            }
            File.WriteAllLines(contextFile, finalLines, Encoding.UTF8);
        }

        private void PostProcess(string file)
        {
            var text = File.ReadAllText(file, Encoding.UTF8);
            File.WriteAllText(file, "// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>" + Environment.NewLine + text.TrimEnd(), Encoding.UTF8);
        }
    }
}