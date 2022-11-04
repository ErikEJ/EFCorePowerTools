using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace RoslynRenamer
{
    internal static class RoslynExtensions
    {
        private static VisualStudioInstance vsInstance;

        public static async Task<Document> RenamePropertyAsync(
            this IEnumerable<Document> documents,
            string className,
            string oldPropertyName,
            string newPropertyName,
            bool renameOverloads = false,
            bool renameInStrings = false,
            bool renameInComments = false,
            bool renameFile = false)
        {
            if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(oldPropertyName) ||
                string.IsNullOrEmpty(newPropertyName))
            {
                return null;
            }

            foreach (var document in documents)
            {
                var root = await document.GetSyntaxRootAsync();
                var classSyntax = root?.DescendantNodesAndSelf().OfType<TypeDeclarationSyntax>()
                    .FirstOrDefault(o => o.Identifier.ToString() == className);
                if (classSyntax == null)
                {
                    continue; // not found in this doc
                }

                // now find property
                var propSyntax = classSyntax.DescendantNodesAndSelf().OfType<PropertyDeclarationSyntax>()
                    .FirstOrDefault(o => o.Identifier.Text == oldPropertyName);
                if (propSyntax == null)
                {
                    continue; // not found in this class
                }

                // found it
                var model = await document.GetSemanticModelAsync();
                var propSymbol = model.GetDeclaredSymbol(propSyntax) ??
                                 throw new Exception("Property symbol not found");

                // rename all references to the property
                var newSolution = await Renamer.RenameSymbolAsync(
                    document.Project.Solution,
                    propSymbol,
                    new SymbolRenameOptions(renameOverloads, renameInStrings, renameInComments, renameFile),
                    newPropertyName);

                // sln has been revised. return new doc
                var currentDocument = newSolution.GetDocument(document.Id);
                return currentDocument;
            }

            return null;
        }

        public static async Task<int> SaveDocumentsAsync(this IEnumerable<Document> documents)
        {
            var saveCount = 0;
            foreach (var document in documents)
            {
                var path = document.FilePath ?? throw new Exception("Path unknown for document " + document.Name);
                var text = string.Join(
                    Environment.NewLine,
                    (await document.GetTextAsync()).Lines.Select(o => o.ToString())).Trim();
                var orig = File.ReadAllText(path, Encoding.UTF8)?.Trim();
                if (text == orig)
                {
                    continue;
                }

                File.WriteAllText(path, text, Encoding.UTF8);
                saveCount++;
            }

            return saveCount;
        }

        public static async Task<Project> LoadExistingProjectAsync(string csProjPath)
        {
            try
            {
                vsInstance ??= MSBuildLocatorRegisterDefaults();
                Debug.WriteLine($"Using msbuild: {vsInstance.MSBuildPath}");
                using var workspace = MSBuildWorkspace.Create();
                workspace.WorkspaceFailed += (_, failure) => Debug.WriteLine(failure.Diagnostic);
                var project = await workspace.OpenProjectAsync(csProjPath);
                var docs = project.Documents.ToArray();
                var diagnostics = workspace.Diagnostics;
                foreach (var diagnostic in diagnostics)
                {
                    if (diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
                    {
                        throw new Exception(diagnostic.Message);
                    }
                }

                Debug.Assert(docs.Length > 0);
                return project;
            }
            catch
            {
                return null;
            }
        }

        private static VisualStudioInstance MSBuildLocatorRegisterDefaults()
        {
            // override default behavior using using reflection to get the VS instances list and register the LATEST version of VS  
            try
            {
                var instances = MSBuildLocator.QueryVisualStudioInstances(VisualStudioInstanceQueryOptions.Default)
                    .OrderByDescending(o => o.Version).ToArray();
                if (instances.Length > 0)
                {
                    var latest = instances.FirstOrDefault();

                    latest = (VisualStudioInstance)Activator.CreateInstance(
                        typeof(VisualStudioInstance),
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        new object[] { latest.Name, latest.MSBuildPath + "\\", latest.Version, latest.DiscoveryType },
                        null,
                        null)!;

                    MSBuildLocator.RegisterInstance(latest);
                    return latest;
                }

                return MSBuildLocator.RegisterDefaults();
            }
            catch
            {
                // ignored
                return null;
            }
        }

        public static AdhocWorkspace GetWorkspaceForFilePaths(
            this IEnumerable<string> filePaths,
            IEnumerable<MetadataReference> projReferences = null)
        {
            var ws = new AdhocWorkspace();
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var references = new List<MetadataReference>() { mscorlib };
            if (projReferences != null)
            {
                references.AddRange(projReferences);
            }

            var projInfo = ProjectInfo.Create(
                ProjectId.CreateNewId(),
                VersionStamp.Create(),
                "MyProject",
                "MyAssembly",
                "C#",
                metadataReferences: references);

            var projectId = ws.AddProject(projInfo).Id;

            foreach (var filePath in filePaths)
            {
                var info = new FileInfo(filePath);
                var content = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(content))
                {
                    continue;
                }

                var text = SourceText.From(content);
                var documentInfo = DocumentInfo.Create(
                        DocumentId.CreateNewId(projectId),
                        info.Name,
                        null,
                        SourceCodeKind.Regular,
                        TextLoader.From(TextAndVersion.Create(text, VersionStamp.Default, info.FullName)))
                    .WithFilePath(info.FullName);
                ws.AddDocument(documentInfo);
            }

            return ws;
        }
    }
}
