using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace EFCorePowerTools.Helpers
{
    internal static class RoslynEntityPropertyRenamer
    {
        /// <summary> apply renaming using Roslyn, return rename count </summary>
        /// <param name="path">path to renaming rules file. can process primitive or navigation rules.</param>
        /// <param name="contextFolder">optional subfolder for context location. </param>
        /// <param name="modelsFolder">optional subfolder for model location. </param>
        /// <returns>number of properties renamed. </returns>
        public static async Task<int> ApplyRenamingRulesAsync(string path, string contextFolder = "Data", string modelsFolder = "Models")
        {
            var fileInfo = new FileInfo(path);
            var dir = fileInfo.Directory?.FullName;
            var csProjPath = Directory.GetFiles(dir, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();

            Project project = null;
            if (csProjPath != null)
            {
                project = await RoslynExtensions.LoadExistingProjectAsync(csProjPath);
            }

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
                    cSharpFolders.Add(Path.Combine(dir, contextFolder));
                if (modelsFolder?.Length > 0 && Directory.Exists(Path.Combine(dir, modelsFolder)))
                    cSharpFolders.Add(Path.Combine(dir, modelsFolder));
                if (cSharpFolders.Count == 0) return 0;
                var cSharpFiles = cSharpFolders
                    .SelectMany(o => Directory.GetFiles(o, "*.cs", SearchOption.AllDirectories))
                    .Distinct().ToList();
                if (cSharpFiles.Count == 0) return 0;

                var refAssemblies = new[]
                {
                    //typeof(IEntityTypeConfiguration<>).Assembly, typeof(SqlServerValueGenerationStrategy).Assembly,
                    //typeof(Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SearchConditionConvertingExpressionVisitor).Assembly,
                    //typeof(Microsoft.EntityFrameworkCore.RelationalDbFunctionsExtensions).Assembly,
                    typeof(NotMappedAttribute).Assembly,
                }.Distinct().ToList();
                var refs = refAssemblies.Select(o => MetadataReference.CreateFromFile(o.Location)).ToList();

                try
                {
                    var workspace = cSharpFiles.GetWorkspaceForFilePaths(refs);
                    project = workspace.CurrentSolution.Projects.First();
                }
                catch
                {
                    return 0; // log this?
                }
            }

            var rulesText = File.ReadAllText(path);
            var rulesList = RenamingRulesSerializer.FromJson(rulesText);
            var renameCount = 0;
            foreach (var rule in rulesList)
            {
                foreach (var tableRename in rule.Tables)
                {
                    var table = tableRename.NewName ?? tableRename.Name;
                    foreach (var columnRename in tableRename.Columns)
                    {
                        var fromNames = new[] { columnRename.Name, columnRename.AlternateName }
                            .Where(o => !string.IsNullOrEmpty(o)).Distinct().ToArray();
                        if (fromNames.Length == 0) { continue; }

                        Document docWithRename = null;
                        foreach (var fromName in fromNames)
                        {
                            docWithRename = await project.Documents.RenameProperty(
                                table,
                                fromName,
                                columnRename.NewName);
                            if (docWithRename != null) { break; }
                        }

                        if (docWithRename != null)
                        {
                            // documents have been mutated. update reference to workspace:
                            project = docWithRename.Project;
                            renameCount++;
                            Console.WriteLine(
                                $"Renamed table {table} nav prop {fromNames[0]} -> {columnRename.NewName}");
                        }
                        else
                        {
                            Console.WriteLine($"Could not find table {table} nav prop {string.Join(", ", fromNames)}");
                        }
                    }
                }
            }

            if (renameCount == 0) return 0;
            var saved = await project.Documents.SaveDocuments();
            Debug.Assert(saved > 0);
            return renameCount;
        }
    }
}
