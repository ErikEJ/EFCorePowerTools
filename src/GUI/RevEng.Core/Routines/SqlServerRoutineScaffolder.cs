using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using NetTopologySuite.Geometries;
using RevEng.Common;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;

#if CORE60
using Microsoft.EntityFrameworkCore.Infrastructure;
#else
using Microsoft.EntityFrameworkCore.Internal;
#endif

namespace RevEng.Core.Modules
{
    public abstract class SqlServerRoutineScaffolder : IRoutineScaffolder
    {
#pragma warning disable SA1401 // Fields should be private
        internal readonly ICSharpHelper Code;
        internal IndentedStringBuilder Sb;
#pragma warning restore SA1401 // Fields should be private

        private static readonly HashSet<string> KeyWords = new HashSet<string>
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while",
        };

        protected SqlServerRoutineScaffolder([NotNull] ICSharpHelper code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            this.Code = code;
        }

        public string FileNameSuffix { get; set; }

        public SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls)
        {
            if (scaffoldedModel == null)
            {
                throw new ArgumentNullException(nameof(scaffoldedModel));
            }

            Directory.CreateDirectory(outputDir);

            var contextPath = Path.GetFullPath(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            var path = Path.GetDirectoryName(contextPath);
            if (path != null)
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(contextPath, scaffoldedModel.ContextFile.Code, Encoding.UTF8);
            }

            var additionalFiles = new List<string>();

            foreach (var entityTypeFile in scaffoldedModel.AdditionalFiles)
            {
                var additionalFilePath = Path.Combine(outputDir, entityTypeFile.Path);
                var addpath = Path.GetDirectoryName(additionalFilePath);
                if (addpath != null)
                {
                    Directory.CreateDirectory(addpath);
                    File.WriteAllText(additionalFilePath, entityTypeFile.Code, Encoding.UTF8);
                    additionalFiles.Add(additionalFilePath);
                }
            }

            return new SavedModelFiles(contextPath, additionalFiles);
        }

        public ScaffoldedModel ScaffoldModel(RoutineModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            if (scaffolderOptions == null)
            {
                throw new ArgumentNullException(nameof(scaffolderOptions));
            }

            var result = new ScaffoldedModel();

            errors.AddRange(model.Errors);

            foreach (var routine in model.Routines.Where(r => string.IsNullOrEmpty(r.MappedType) && (!(r is Function f) || !f.IsScalar)))
            {
                int i = 1;

                foreach (var resultSet in routine.Results)
                {
                    if (routine.NoResultSet)
                    {
                        continue;
                    }

                    var suffix = string.Empty;
                    if (routine.Results.Count > 1)
                    {
                        suffix = $"{i++}";
                    }

                    var typeName = GenerateIdentifierName(routine, model) + "Result" + suffix;

                    var classContent = WriteResultClass(resultSet, scaffolderOptions, typeName);

                    result.AdditionalFiles.Add(new ScaffoldedFile
                    {
                        Code = classContent,
                        Path = scaffolderOptions.UseSchemaFolders
                                ? Path.Combine(routine.Schema, $"{typeName}.cs")
                                : $"{typeName}.cs",
                    });
                }
            }

            var dbContextInterface = WriteDbContextInterface(scaffolderOptions, model);

            if (!string.IsNullOrEmpty(dbContextInterface))
            {
                result.AdditionalFiles.Add(new ScaffoldedFile
                {
                    Code = dbContextInterface,
                    Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, $"I{scaffolderOptions.ContextName}Procedures.cs")),
                });
            }

            var dbContext = WriteDbContext(scaffolderOptions, model);

            result.ContextFile = new ScaffoldedFile
            {
                Code = dbContext,
                Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + $"{FileNameSuffix}.cs")),
            };

            return result;
        }

        protected static string GenerateIdentifierName(Routine routine, RoutineModel model)
        {
            if (routine == null)
            {
                throw new ArgumentNullException(nameof(routine));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return CreateIdentifier(GenerateUniqueName(routine, model)).Item1;
        }

        protected abstract string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, RoutineModel model);

        protected abstract string WriteDbContextInterface(ModuleScaffolderOptions scaffolderOptions, RoutineModel model);

        private static Tuple<string, string> CreateIdentifier(string name)
        {
            var original = name;

            var isValid = System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);

            string columAttribute = null;

            if (!isValid)
            {
                columAttribute = $"[Column(\"{name}\")]";

                // File name contains invalid chars, remove them
                var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                name = regex.Replace(name, string.Empty);

                if (string.IsNullOrWhiteSpace(name))
                {
                    // we cannot fix it
                    name = original;
                }
                else if (!char.IsLetter(name, 0))
                {
                    name = name.Insert(0, "_");
                }
            }

            if (KeyWords.Contains(name))
            {
                name = "@" + name;
            }

            return new Tuple<string, string>(name.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase), columAttribute);
        }

        private static string GenerateUniqueName(Routine routine, RoutineModel model)
        {
            if (!string.IsNullOrEmpty(routine.NewName))
            {
                return routine.NewName;
            }

            var numberOfNames = model.Routines.Count(p => p.Name == routine.Name);

            if (numberOfNames > 1)
            {
                return routine.Name + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(routine.Schema);
            }

            return routine.Name;
        }

        private static Tuple<string, string> GeneratePropertyName(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return CreateIdentifier(propertyName);
        }

        private string WriteResultClass(List<ModuleResultElement> resultElements, ModuleScaffolderOptions options, string name)
        {
            var @namespace = options.ModelNamespace;

            Sb = new IndentedStringBuilder();

            Sb.AppendLine(PathHelper.Header);

            if (resultElements.Any(p => p.ClrType() == typeof(Geometry)))
            {
                Sb.AppendLine("using NetTopologySuite.Geometries;");
            }

            Sb.AppendLine("using System;");
            Sb.AppendLine("using System.Collections.Generic;");
            Sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            Sb.AppendLine();

            if (options.NullableReferences)
            {
                Sb.AppendLine("#nullable enable");
                Sb.AppendLine();
            }

            Sb.AppendLine($"namespace {@namespace}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                GenerateClass(resultElements, name, options.NullableReferences);
            }

            Sb.AppendLine("}");

            return Sb.ToString();
        }

        private void GenerateClass(List<ModuleResultElement> resultElements, string name, bool nullableReferences)
        {
            Sb.AppendLine($"public partial class {name}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                GenerateProperties(resultElements, nullableReferences);
            }

            Sb.AppendLine("}");
        }

        private void GenerateProperties(List<ModuleResultElement> resultElements, bool nullableReferences)
        {
            foreach (var property in resultElements.OrderBy(e => e.Ordinal))
            {
                var propertyNames = GeneratePropertyName(property.Name);

                if (!string.IsNullOrEmpty(propertyNames.Item2))
                {
                    Sb.AppendLine(propertyNames.Item2);
                }

                var propertyType = property.ClrType();
                string nullableAnnotation = string.Empty;
                string defaultAnnotation = string.Empty;

                if (nullableReferences && !propertyType.IsValueType)
                {
                    if (property.Nullable)
                    {
                        nullableAnnotation = "?";
                    }
                    else
                    {
                        defaultAnnotation = $" = default!;";
                    }
                }

                if (property.StoreType == "decimal")
                {
                    Sb.AppendLine($"[Column(TypeName = \"{property.StoreType}({property.Precision},{property.Scale})\")]");
                }

                Sb.AppendLine($"public {Code.Reference(propertyType)}{nullableAnnotation} {propertyNames.Item1} {{ get; set; }}{defaultAnnotation}");
            }
        }
    }
}
