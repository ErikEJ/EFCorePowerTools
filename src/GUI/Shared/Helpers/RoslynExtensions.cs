using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace EFCorePowerTools.Helpers
{
    public static class RoslynExtensions
    {
        /// <summary> traverse the parents of this node searching for a node of the given type T </summary>
        public static T GetParent<T>(this SyntaxNode syntaxNode) where T : SyntaxNode
        {
            var p = syntaxNode?.Parent;
            while (p != null && !(p is T))
                try { p = p.Parent; }
                catch { p = null; }

            return p as T;
        }

        /// <summary> traverse the parents of this node searching for a node of the given type T </summary>
        public static IEnumerable<T> EnumerateParents<T>(this SyntaxNode syntaxNode) where T : SyntaxNode
        {
            var p = syntaxNode?.Parent;
            if (p is T tp) yield return tp;
            while (p != null)
            {
                try { p = p.Parent; }
                catch { p = null; }

                if (p is T tp2) yield return tp2;
            }
        }

        /// <summary> return true if the root of the given node matches the given root. </summary>
        public static bool HasRoot(this SyntaxNode syntaxNode, CompilationUnitSyntax root)
        {
            return syntaxNode.GetParent<CompilationUnitSyntax>() == root;
        }

        /// <summary> return true if the root contains the given node. </summary>
        public static bool HasParentNode<T>(this T parent, SyntaxNode syntaxNode) where T : SyntaxNode
        {
            return syntaxNode.EnumerateParents<T>().Any(o => o == parent);
        }

        private const char nestedClassDelimiter = '+';
        private const char namespaceClassDelimiter = '.';
        private const char typeparameterClassDelimiter = '`';

        /// <summary> get class name with namespace. handles nested classes. </summary>
        /// <param name="source"> type syntax </param>
        /// <returns> class name with namespace </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetFullName(this BaseTypeDeclarationSyntax source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var namespaces = new LinkedList<NamespaceDeclarationSyntax>();
            var types = new LinkedList<TypeDeclarationSyntax>();
            for (var parent = source.Parent; parent is object; parent = parent.Parent)
            {
                if (parent is NamespaceDeclarationSyntax @namespace)
                {
                    namespaces.AddFirst(@namespace);
                }
                else if (parent is TypeDeclarationSyntax type)
                {
                    types.AddFirst(type);
                }
            }

            var result = new StringBuilder();
            for (var item = namespaces.First; item is object; item = item.Next)
            {
                result.Append(item.Value.Name).Append(namespaceClassDelimiter);
            }

            for (var item = types.First; item is object; item = item.Next)
            {
                var type = item.Value;
                AppendName(result, type);
                result.Append(nestedClassDelimiter);
            }

            AppendName(result, source);

            return result.ToString();
        }

        private static void AppendName(StringBuilder builder, BaseTypeDeclarationSyntax btype)
        {
            builder.Append(btype.Identifier.Text);
            if (btype is TypeDeclarationSyntax type)
            {
                var typeArguments = type.TypeParameterList?.ChildNodes()
                    .Count(node => node is TypeParameterSyntax) ?? 0;

                if (typeArguments != 0)
                    builder.Append(typeparameterClassDelimiter).Append(typeArguments);
            }
        }

        /// <summary> get class name with namespace. handles nested classes. </summary>
        public static string GetFullName(this ClassDeclarationSyntax source)
        {
            Contract.Requires(null != source);

            var items = new List<string>();
            var parent = source.Parent;
            while (parent.IsKind(SyntaxKind.ClassDeclaration))
            {
                var parentClass = parent as ClassDeclarationSyntax;
                Contract.Assert(null != parentClass);
                items.Add(parentClass.Identifier.Text);

                parent = parent.Parent;
            }

            var nameSpace = parent as NamespaceDeclarationSyntax;
            if (nameSpace == null) return null;
            Contract.Assert(null != nameSpace);
            var sb = new StringBuilder().Append(nameSpace.Name).Append(namespaceClassDelimiter);
            items.Reverse();
            items.ForEach(i => { sb.Append(i).Append(nestedClassDelimiter); });
            sb.Append(source.Identifier.Text);

            var result = sb.ToString();
            return result;
        }

        /// <summary>
        /// rename a code type from the old name to the new name. All references to that type will also be changed assuming it is in the provided source.
        /// </summary>
        /// <param name="code"> source code </param>
        /// <param name="originalName">type name</param>
        /// <param name="newClassName">new type name</param>
        /// <returns></returns>
        public static async Task<string> RenameInCodeAsync(this string code, string originalName, string newClassName)
        {
            var document = GetDocumentForCode(code);
            var compilation = document.Project.GetCompilationAsync().GetAwaiter().GetResult();

            var root = document.GetSyntaxRootAsync().GetAwaiter().GetResult();
            var originalClass = root.DescendantNodesAndSelf().OfType<TypeDeclarationSyntax>()
                .First(o => o.Identifier.ToString() == originalName);

            var model = compilation.GetSemanticModel(root.SyntaxTree);
            var originalSymbol = model.GetDeclaredSymbol(originalClass);
            if (originalSymbol == null)
            {
                return code; // not found
            }

            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution,
                originalSymbol,
                new SymbolRenameOptions(false, false, false, false),
                newClassName);

            var newDocument = newSolution.GetDocument(document.Id);
            var newRoot = await newDocument.GetSyntaxRootAsync();
            var newCode = newRoot?.ToString();
            return newCode;
        }

        /// <summary> Rename a property in a given workspace, and adjust all references. </summary>
        /// <param name="documents">Workspace containing documents to refactor</param>
        /// <param name="className">Type that contains the property to rename</param>
        /// <param name="oldPropertyName">Property to rename</param>
        /// <param name="newPropertyName">The new name</param>
        /// <param name="renameOverloads"></param>
        /// <param name="renameInStrings"></param>
        /// <param name="renameInComments"></param>
        /// <param name="renameFile"></param>
        /// <returns>Returns the document that contains the property. Not that the workspace has been revised and will not match the original reference.</returns>
        public static async Task<Document> RenameProperty(this IEnumerable<Document> documents, string className,
            string oldPropertyName, string newPropertyName,
            bool renameOverloads = false, bool renameInStrings = false, bool renameInComments = false,
            bool renameFile = false)
        {
            if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(oldPropertyName) ||
                string.IsNullOrEmpty(newPropertyName)) return null;
            foreach (var document in documents)
            {
                var root = await document.GetSyntaxRootAsync();
                var classSyntax = root?.DescendantNodesAndSelf().OfType<TypeDeclarationSyntax>()
                    .FirstOrDefault(o => o.Identifier.ToString() == className);
                if (classSyntax == null) continue; // not found in this doc

                // now find property
                var propSyntax = classSyntax.DescendantNodesAndSelf().OfType<PropertyDeclarationSyntax>()
                    .FirstOrDefault(o => o.Identifier.Text == oldPropertyName);
                if (propSyntax == null) continue; // not found in this class

                // found it
                var model = await document.GetSemanticModelAsync();
                var propSymbol = model.GetDeclaredSymbol(propSyntax) ??
                                 throw new Exception("Property symbol not found");

                // rename all references to the property
                var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution,
                    propSymbol,
                    new SymbolRenameOptions(renameOverloads, renameInStrings, renameInComments, renameFile),
                    newPropertyName
                );
                // sln has been revised. return new doc
                var currentDocument = newSolution.GetDocument(document.Id);
                return currentDocument;
            }

            return null;
        }

        /// <summary> save documents to disk provided they have associated file paths </summary>
        public static async Task<int> SaveDocuments(this IEnumerable<Document> documents)
        {
            var saveCount = 0;
            foreach (var document in documents)
            {
                var path = document.FilePath ?? throw new Exception("Path unknown for document " + document.Name);
                var text = string.Join(
                    Environment.NewLine,
                    (await document.GetTextAsync()).Lines.Select(o => o.ToString())).Trim();
                var orig = File.ReadAllText(path)?.Trim();
                if (text == orig)
                {
                    continue;
                }

                File.WriteAllText(path, text);
                saveCount++;
            }

            return saveCount;
        }

        /// <summary> Helper method to build Document </summary>
        public static Document GetDocumentForCode(this string code)
        {
            var ws = new AdhocWorkspace();
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var references = new List<MetadataReference>() { mscorlib };
            var projInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "MyProject", "MyAssembly",
                "C#", metadataReferences: references);
            var project = ws.AddProject(projInfo);

            var text = SourceText.From(code);
            var myDocument = ws.AddDocument(project.Id, "MyDocument.cs", text);
            return myDocument;
        }

        /// <summary> load workspace and project for the given csproj file path </summary>
        /// <param name="csProjPath">csproj file path</param>
        /// <returns>Roslyn project</returns>
        public static async Task<Project> LoadExistingProjectAsync(string csProjPath)
        {
            try
            {
                MSBuildLocator.RegisterDefaults();
                var workspace = MSBuildWorkspace.Create();
                var project = await workspace.OpenProjectAsync(csProjPath);
                return project;
            }
            catch
            {
                return null;
            }
        }

        /// <summary> Get Roslyn workspace for list of code file paths </summary>
        public static AdhocWorkspace GetWorkspaceForFilePaths(this IEnumerable<string> filePaths,
            IEnumerable<MetadataReference> projReferences = null)
        {
            var ws = new AdhocWorkspace();
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var references = new List<MetadataReference>() { mscorlib };
            if (projReferences != null) references.AddRange(projReferences);
            var projInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "MyProject", "MyAssembly",
                "C#", metadataReferences: references);
            var projectId = ws.AddProject(projInfo).Id;

            foreach (var filePath in filePaths)
            {
                var info = new FileInfo(filePath);
                var content = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(content)) continue;
                var text = SourceText.From(content);
                var documentInfo = DocumentInfo.Create(DocumentId.CreateNewId(projectId), info.Name, null,
                        SourceCodeKind.Regular,
                        TextLoader.From(TextAndVersion.Create(text, VersionStamp.Default, info.FullName)))
                    .WithFilePath(info.FullName);
                ws.AddDocument(documentInfo);
            }

            return ws;
        }

        /// <summary> Get Roslyn workspace for list of code documents </summary>
        public static AdhocWorkspace GetProjectForDocuments(this IEnumerable<string> documents)
        {
            var ws = new AdhocWorkspace();
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var references = new List<MetadataReference>() { mscorlib };
            var projInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "MyProject", "MyAssembly",
                "C#", metadataReferences: references);
            var project = ws.AddProject(projInfo);

            var i = 0;
            foreach (var document in documents)
            {
                var text = SourceText.From(document);
                ws.AddDocument(project.Id, $"MyDocument{++i}.cs", text);
            }

            return ws;
        }
    }
}
