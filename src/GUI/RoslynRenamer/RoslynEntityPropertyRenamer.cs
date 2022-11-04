using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RevEng.Common;
using RevEng.Common.EnumMapping;
using RevEng.Common.Extensions;
using RevEng.Common.PropertyRenaming;
using RevEng.Common.TableColumnRenaming;

[assembly: CLSCompliant(false)]

namespace RoslynRenamer
{
    public static class RoslynEntityPropertyRenamer
    {
        /// <summary> Load all rule files from the project base path and apply to the enclosed project. </summary>
        /// <param name="projectBasePath"> project folder. </param>
        /// <returns> list of errors. </returns>
        public static async Task<List<string>> ApplyRenamingRulesAsync(string projectBasePath)
        {
            if (projectBasePath == null || !Directory.Exists(projectBasePath))
            {
                throw new ArgumentException(nameof(projectBasePath));
            }

            var fullProjectPath = Directory.GetFiles(projectBasePath, "*.csproj").FirstOrDefault();
            if (fullProjectPath == null)
            {
                throw new ArgumentException(nameof(projectBasePath));
            }

            var jsonFiles = Directory.GetFiles(
                    projectBasePath,
                    RuleFiles.GeneralEfPtJsonFileMask,
                    SearchOption.TopDirectoryOnly)
                .Select(o => new FileInfo(o))
                .ToList();

            var status = new List<string>();
            if (jsonFiles.Count == 0)
            {
                return status; // nothing to do
            }

            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    if (!SupportedRuleFiles.Contains(jsonFile.Name))
                    {
                        continue;
                    }

                    switch (jsonFile.Name.ToLower(CultureInfo.InvariantCulture))
                    {
                        case RuleFiles.PropertyFilename:
                            if (TryReadRules<PropertyRenamingRoot>(jsonFile, status, out var propertyRenamingRoot))
                            {
                                status.AddRange(
                                    await ApplyRenamingRulesInternalAsync(propertyRenamingRoot, fullProjectPath));
                            }

                            break;
                        case RuleFiles.EnumMappingFilename:
                            if (TryReadRules<EnumMappingRoot>(jsonFile, status, out var enumMappingRoot))
                            {
                                status.AddRange(
                                    await ApplyEnumMappingRulesInternalAsync(enumMappingRoot, fullProjectPath));
                            }

                            break;
                        case RuleFiles.RenamingFilename:
                            if (TryReadRules<List<Schema>>(jsonFile, status, out var schemas))
                            {
                                status.AddRange(
                                    await ApplyTableAndColumnRenamingRulesInternalAsync(schemas, fullProjectPath));
                            }

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return status;
        }

        private static readonly HashSet<string> SupportedRuleFiles =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                RuleFiles.PropertyFilename, RuleFiles.EnumMappingFilename
            };


        private static bool TryReadRules<T>(FileInfo jsonFile, List<string> status, out T rules) where T : class, new()
        {
            if (!jsonFile.FullName.TryReadJsonFile<T>(out rules))
            {
                status.Add($"Unable to open {jsonFile.Name}");
                return false;
            }

            return true;
        }


        public static Task<List<string>> ApplyRenamingRulesAsync(
            PropertyRenamingRoot propertyRenamingRoot,
            string fullProjectPath,
            string contextFolder = "",
            string modelsFolder = "")
        {
            if (propertyRenamingRoot == null)
            {
                throw new ArgumentNullException(nameof(propertyRenamingRoot));
            }

            return ApplyRenamingRulesInternalAsync(propertyRenamingRoot, fullProjectPath, contextFolder, modelsFolder);
        }

        private static async Task<List<string>> ApplyRenamingRulesInternalAsync(
            PropertyRenamingRoot propertyRenamingRoot,
            string fullProjectPath,
            string contextFolder = "",
            string modelsFolder = "")
        {
            var status = new List<string>();
            if (propertyRenamingRoot.Classes == null || propertyRenamingRoot.Classes.Count == 0)
            {
                return status; // nothing to do
            }

            var project = await RoslynExtensions.LoadExistingProjectAsync(fullProjectPath);

            var dir = Path.GetDirectoryName(fullProjectPath);

            if ((project == null || !project.Documents.Any()) &&
                !TryLoadFallbackAdhocProject(dir, contextFolder, modelsFolder, status, ref project))
            {
                return status;
            }

            var renameCount = 0;
            foreach (var classRename in propertyRenamingRoot.Classes)
            {
                foreach (var refRename in classRename.Properties)
                {
                    var fromNames = new[] { refRename.Name, refRename.AlternateName }
                        .Where(o => !string.IsNullOrEmpty(o)).Distinct().ToArray();
                    if (fromNames.Length == 0)
                    {
                        continue;
                    }

                    Document docWithRename = null;
                    foreach (var fromName in fromNames)
                    {
                        docWithRename = await project.Documents.RenamePropertyAsync(
                            classRename.Name,
                            fromName,
                            refRename.NewName);
                        if (docWithRename != null)
                        {
                            break;
                        }
                    }

                    if (docWithRename != null)
                    {
                        // documents have been mutated. update reference to workspace:
                        project = docWithRename.Project;
                        renameCount++;
                        status.Add(
                            $"Property renamer => Renamed class {classRename.Name} property {fromNames[0]} -> {refRename.NewName}");
                    }
                    else
                    {
                        status.Add(
                            $"Property renamer => Could not find table {classRename.Name} property {string.Join(", ", fromNames)}");
                    }
                }
            }

            if (renameCount == 0)
            {
                status.Add("Property renamer => No properties renamed");
                return status;
            }

            var saved = await project.Documents.SaveDocumentsAsync();
            Debug.Assert(saved > 0, "No documents saved");
            status.Add($"Property renamer => {renameCount} properties renamed across {saved} files");
            return status;
        }

        private static async Task<List<string>> ApplyEnumMappingRulesInternalAsync(
            EnumMappingRoot enumMappingRoot,
            string fullProjectPath,
            string contextFolder = "",
            string modelsFolder = "")
        {
            var status = new List<string>();
            if (enumMappingRoot.Classes == null || enumMappingRoot.Classes.Count == 0)
            {
                return status; // nothing to do
            }

            var project = await RoslynExtensions.LoadExistingProjectAsync(fullProjectPath);

            var dir = Path.GetDirectoryName(fullProjectPath);

            if ((project == null || !project.Documents.Any()) &&
                !TryLoadFallbackAdhocProject(dir, contextFolder, modelsFolder, status, ref project))
            {
                return status;
            }

            var renameCount = 0;
            foreach (var classRename in enumMappingRoot.Classes)
            {
                foreach (var refRename in classRename.Properties)
                {
                    var fromName = refRename.Name?.Trim() ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(fromName))
                    {
                        continue;
                    }

                    // todo 
                    Document docWithRename = null;

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (docWithRename != null)
                    {
                        // documents have been mutated. update reference to workspace:
                        project = docWithRename.Project;
                        renameCount++;
                        status.Add(
                            $"Enum mapper => Renamed class {classRename.Name} property {fromName} -> {refRename.EnumType}");
                    }
                    else
                    {
                        status.Add($"Enum mapper => Could not find table {classRename.Name} property {fromName}");
                    }
                }
            }

            if (renameCount == 0)
            {
                status.Add("Enum mapper => No properties renamed");
                return status;
            }

            var saved = await project.Documents.SaveDocumentsAsync();
            Debug.Assert(saved > 0, "No documents saved");
            status.Add($"Enum mapper => {renameCount} properties mapped to enums across {saved} files");
            return status;
        }

        private static async Task<List<string>> ApplyTableAndColumnRenamingRulesInternalAsync(
            List<Schema> schemas,
            string fullProjectPath,
            string contextFolder = "",
            string modelsFolder = "")
        {
            var status = new List<string>();


            return status; // nothing to do 
        }

        private static bool TryLoadFallbackAdhocProject(string projectBasePath, string contextFolder,
            string modelsFolder, List<string> status, ref Project project)
        {
            /* If existing project file fails to load, we can fallback to creating an adhoc workspace with an in memory project
                      * that contains only the cs files we are concerned with.
                      *
                      * Currently, this will successfully rename the model properties but it does NOT process the references from the Configuration
                      * classes.  This is likely because Roslyn is not resolving information about EntityTypeBuilder<> such that it
                      * can identify the model property references.
                      *
                      * This issue *should* be resolved simply by adding EFC references to the project (as is illustrated below) but
                      * it's still not working.
                      *
                      * To do: resolve paths to EFC assemblies and use those in the refAssemblies list below.
                      */
            var cSharpFolders = new HashSet<string>();
            if (contextFolder?.Length > 0 && Directory.Exists(Path.Combine(projectBasePath, contextFolder)))
            {
                cSharpFolders.Add(Path.Combine(projectBasePath, contextFolder));
            }

            if (modelsFolder?.Length > 0 && Directory.Exists(Path.Combine(projectBasePath, modelsFolder)))
            {
                cSharpFolders.Add(Path.Combine(projectBasePath, modelsFolder));
            }

            if (cSharpFolders.Count == 0)
            {
                // use project base path
                cSharpFolders.Add(projectBasePath);
            }

            var cSharpFiles = cSharpFolders
                .SelectMany(o => Directory.GetFiles(o, "*.cs", SearchOption.AllDirectories))
                .Distinct().ToList();

            if (cSharpFiles.Count == 0)
            {
                status.Add("Property renamer => No .cs files found");
                {
                    return false;
                }
            }

            var refAssemblies = new[]
            {
                // typeof(IEntityTypeConfiguration<>).Assembly, typeof(SqlServerValueGenerationStrategy).Assembly,
                // typeof(Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SearchConditionConvertingExpressionVisitor).Assembly,
                // typeof(Microsoft.EntityFrameworkCore.RelationalDbFunctionsExtensions).Assembly,
                typeof(NotMappedAttribute).Assembly
            }.Distinct().ToList();
            var refs = refAssemblies.Select(o => MetadataReference.CreateFromFile(o.Location)).ToList();

            try
            {
                using var workspace = cSharpFiles.GetWorkspaceForFilePaths(refs);
                project = workspace.CurrentSolution.Projects.First();
            }
            catch (Exception ex)
            {
                status.Add($"Property renamer => Unable to get in-memory project from workspace: {ex.Message}");
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool TryParseArgs(string[] args, out string projectBasePath)
        {
            projectBasePath = null;
            if (args == null || args.Length == 0 || (args.Length == 1 && args[0] == "."))
            {
                // auto inspect current folder for both csproj and edmx
                projectBasePath = Directory.GetCurrentDirectory();
                var projectFiles = Directory.GetFiles(projectBasePath, "*.csproj", SearchOption.TopDirectoryOnly);
                if (projectFiles.Length != 1)
                {
                    projectBasePath = null;
                    return false;
                }

                return true;
            }

            var csProjFile =
                args.FirstOrDefault(o => o?.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) == true);

            if (csProjFile != null && File.Exists(csProjFile))
            {
                projectBasePath = new FileInfo(csProjFile).Directory?.FullName;
                return projectBasePath != null;
            }

            projectBasePath = args.FirstOrDefault(o =>
                o?.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) == false
                && Directory.Exists(o));
            if (projectBasePath == null)
            {
                return false;
            }

            if (!Directory.Exists(projectBasePath))
            {
                return false;
            }

            var projectFiles2 = Directory.GetFiles(projectBasePath, "*.csproj", SearchOption.TopDirectoryOnly);
            if (projectFiles2.Length == 0)
            {
                projectBasePath = null;
                return false;
            }

            return true;
        }
    }
}
