using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace EFCorePowerTools.Helpers
{
    internal static class RoslynEntityPropertyRenamer
    {
        /// <summary> apply renaming using Roslyn, return rename count.</summary>
        /// <param name="optionsPath">path to renaming rules file. can process primitive or navigation rules.</param>
        /// <param name="projectPath">full path to current .csproj.</param>
        /// <param name="contextFolder">optional subfolder for context location.</param>
        /// <param name="modelsFolder">optional subfolder for model location.</param>
        /// <returns>list of process messages.</returns>
        public static async Task<List<string>> ApplyRenamingRulesAsync(string optionsPath, string projectPath, string contextFolder, string modelsFolder)
        {
            var status = new List<string>();

            var project = await RoslynExtensions.LoadExistingProjectAsync(projectPath);

            var dir = Path.GetDirectoryName(optionsPath);

            if (project == null)
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
                if (contextFolder?.Length > 0 && Directory.Exists(Path.Combine(dir, contextFolder)))
                {
                    cSharpFolders.Add(Path.Combine(dir, contextFolder));
                }

                if (modelsFolder?.Length > 0 && Directory.Exists(Path.Combine(dir, modelsFolder)))
                {
                    cSharpFolders.Add(Path.Combine(dir, modelsFolder));
                }

                if (cSharpFolders.Count == 0)
                {
                    status.Add("Property renamer => No folders found");
                    return status;
                }

                var cSharpFiles = cSharpFolders
                    .SelectMany(o => Directory.GetFiles(o, "*.cs", SearchOption.AllDirectories))
                    .Distinct().ToList();

                if (cSharpFiles.Count == 0)
                {
                    status.Add("Property renamer => No .cs files found");
                    return status;
                }

                var refAssemblies = new[]
                {
                    // typeof(IEntityTypeConfiguration<>).Assembly, typeof(SqlServerValueGenerationStrategy).Assembly,
                    // typeof(Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SearchConditionConvertingExpressionVisitor).Assembly,
                    // typeof(Microsoft.EntityFrameworkCore.RelationalDbFunctionsExtensions).Assembly,
                    typeof(NotMappedAttribute).Assembly,
                }.Distinct().ToList();
                var refs = refAssemblies.Select(o => MetadataReference.CreateFromFile(o.Location)).ToList();

                try
                {
                    var workspace = cSharpFiles.GetWorkspaceForFilePaths(refs);
                    project = workspace.CurrentSolution.Projects.First();
                }
                catch (Exception ex)
                {
                    status.Add($"Property renamer => Unable to get inmemory project from workspace: {ex.Message}");
                    return status;
                }
            }

            var rulesList = RenamingRulesSerializer.TryRead(optionsPath);
            var renameCount = 0;
            foreach (var rule in rulesList)
            {
                foreach (var classRename in rule.Classes)
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
                            status.Add($"Property renamer => Renamed class {classRename.Name} property {fromNames[0]} -> {refRename.NewName}");
                        }
                        else
                        {
                            status.Add($"Property renamer => Could not find table {classRename.Name} property {string.Join(", ", fromNames)}");
                        }
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
            status.Add($"Property renamer => {renameCount} files renamed");
            return status;
        }
    }
}
