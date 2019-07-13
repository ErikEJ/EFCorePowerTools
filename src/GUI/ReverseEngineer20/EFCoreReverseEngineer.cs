using EFCorePowerTools.Shared.Models;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReverseEngineer20
{
    public class EfCoreReverseEngineer
    {
        public EfCoreReverseEngineerResult GenerateFiles(ReverseEngineerOptions reverseEngineerOptions)
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

            var @namespace = reverseEngineerOptions.ProjectRootNamespace;

            if (!string.IsNullOrEmpty(reverseEngineerOptions.OutputPath) && !reverseEngineerOptions.DoNotCombineNamespace)
            {
                @namespace += "." + reverseEngineerOptions.OutputPath.Replace(Path.DirectorySeparatorChar, '.').Replace(Path.AltDirectorySeparatorChar, '.');
            }
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
                    @namespace,
                    "C#",
                    null,
                    reverseEngineerOptions.ContextClassName,
                    modelOptions,
                    codeOptions);

            var filePaths = scaffolder.Save(
                scaffoldedModel,
                Path.Combine(reverseEngineerOptions.ProjectPath, reverseEngineerOptions.OutputPath ?? string.Empty),
                overwriteFiles: true);

            PostProcessContext(filePaths.ContextFile, reverseEngineerOptions);

            foreach (var file in filePaths.AdditionalFiles)
            {
                PostProcess(file, reverseEngineerOptions.IdReplace);
            }
            PostProcess(filePaths.ContextFile, reverseEngineerOptions.IdReplace);

            var result = new EfCoreReverseEngineerResult
            {
                EntityErrors = errors,
                EntityWarnings = warnings,
                EntityTypeFilePaths = filePaths.AdditionalFiles,
                ContextFilePath = filePaths.ContextFile,
            };

            return result;
        }


        private void PostProcessContext(string contextFile, ReverseEngineerOptions options)
        {
            var finalLines = new List<string>();
            var lines = File.ReadAllLines(contextFile);

            int i = 1;
            var inModelCreating = false;

            foreach (var line in lines)
            {
                if (!options.IncludeConnectionString)
                {
                    if (line.Trim().StartsWith("#warning To protect"))
                        continue;

                    if (line.Trim().StartsWith("optionsBuilder.Use"))
                        continue;
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
            File.WriteAllText(file, "// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>" + Environment.NewLine + text.TrimEnd(), Encoding.UTF8);
        }

        public string GenerateClassName(string value)
        {
            var className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            var isValid = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#").IsValidIdentifier(className);

            if (!isValid)
            {
                // File name contains invalid chars, remove them
                var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                className = regex.Replace(className, "");

                // Class name doesn't begin with a letter, insert an underscore
                if (!char.IsLetter(className, 0))
                {
                    className = className.Insert(0, "_");
                }
            }

            return className.Replace(" ", string.Empty);
        }

        public List<TableInformationModel> GetDacpacTables(string dacpacPath)
        {
            var builder = new DacpacTableListBuilder(dacpacPath);
            return builder.GetTableDefinitions();
        }
    }
}
