using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Modules;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RevEng.Core.Functions
{
    public class SqlServerFunctionScaffolder : SqlServerModuleScaffolder, IFunctionScaffolder
    {
        public SqlServerFunctionScaffolder([NotNull] ICSharpHelper code)
            : base(code)
        {
        }

        public ScaffoldedModel ScaffoldModel(FunctionModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var result = new ScaffoldedModel();

            errors = model.Errors;

            foreach (var function in model.Routines.OfType<Function>().Where(f => !f.IsScalar))
            {
                var typeName = GenerateIdentifierName(function, model) + "Result";

                var classContent = WriteResultClass(function, scaffolderOptions, typeName);

                result.AdditionalFiles.Add(new ScaffoldedFile
                {
                    Code = classContent,
                    Path = scaffolderOptions.UseSchemaFolders
                            ? Path.Combine(function.Schema, $"{typeName}.cs")
                            : $"{typeName}.cs"
                });
            }

            var dbContext = WriteFunctionsClass(scaffolderOptions, model);

            result.ContextFile = new ScaffoldedFile
            {
                Code = dbContext,
                Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + ".Functions.cs")),
            };

            return result;
        }

        private string WriteFunctionsClass(ModuleScaffolderOptions scaffolderOptions, FunctionModel model)
        {
            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);

            _sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Linq;");
            _sb.AppendLine($"using {scaffolderOptions.ModelNamespace};");

            _sb.AppendLine();
            _sb.AppendLine($"namespace {scaffolderOptions.ContextNamespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                _sb.AppendLine($"public partial class {scaffolderOptions.ContextName}");

                _sb.AppendLine("{");

                using (_sb.Indent())
                {
                    foreach (var function in model.Routines.OfType<Function>())
                    {
                        GenerateFunctionStub(function, model);
                    }

                    GenerateModelCreation(model);
                }
                _sb.AppendLine("}");
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private void GenerateModelCreation(FunctionModel model)
        {
            _sb.AppendLine();
            _sb.AppendLine("protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                foreach (var function in model.Routines.OfType<Function>().Where(f => !f.IsScalar))
                {
                    var typeName = GenerateIdentifierName(function, model) + "Result";

                    _sb.AppendLine($"modelBuilder.Entity<{typeName}>().HasNoKey();");
                }
            }

            _sb.AppendLine("}");
        }

        private void GenerateFunctionStub(Function function, FunctionModel model)
        {
            var paramStrings = function.Parameters
                .Select(p => $"{code.Reference(p.ClrType())} {p.Name}");

            var identifier = GenerateIdentifierName(function, model);

            _sb.AppendLine();

            _sb.AppendLine($"[DbFunction(\"{function.Name}\", \"{function.Schema}\")]");

            if (function.IsScalar)
            {
            var returnType = paramStrings.First();
            var parameters = string.Empty;

            if (function.Parameters.Count > 1)
            {
                parameters = string.Join(", ", paramStrings.Skip(1));
            }

            _sb.AppendLine($"public static {returnType}{identifier}({parameters})");

            _sb.AppendLine("{");
            using (_sb.Indent())
            {
                _sb.AppendLine("throw new NotSupportedException(\"This method can only be called from Entity Framework Core queries\");");
            }
            _sb.AppendLine("}");
        }
            else
            {
                var typeName = $"{identifier}Result";
                var returnType = $"IQueryable<{typeName}>";

                var parameters = string.Empty;

                if (function.Parameters.Any())
                {
                    parameters = string.Join(", ", paramStrings);
                }

                _sb.AppendLine($"public {returnType} {identifier}({parameters})");

                _sb.AppendLine("{");
                using (_sb.Indent())
                {
                    var argumentStrings = function.Parameters
                        .Select(p => p.Name);
                    var arguments = string.Join(", ", argumentStrings);
                    _sb.AppendLine($"return FromExpression(() => {identifier}({arguments}));");
                }
                _sb.AppendLine("}");
            }
        }

        private string WriteResultClass(Function function, ModuleScaffolderOptions options, string name)
        {
            var @namespace = options.ModelNamespace;

            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);
            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Collections.Generic;");
            _sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            _sb.AppendLine();

            if (options.NullableReferences)
            {
                _sb.AppendLine("#nullable enable");
                _sb.AppendLine();
            }

            _sb.AppendLine($"namespace {@namespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateClass(function, name, options.NullableReferences);
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private void GenerateClass(Function function, string name, bool nullableReferences)
        {
            _sb.AppendLine($"public partial class {name}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateProperties(function, nullableReferences);
            }

            _sb.AppendLine("}");
        }

        private void GenerateProperties(Function function, bool nullableReferences)
        {
            foreach (var property in function.ResultElements.OfType<TableFunctionResultElement>().OrderBy(e => e.Ordinal))
            {
                var propertyNames = GeneratePropertyName(property.Name);

                if (!string.IsNullOrEmpty(propertyNames.Item2))
                {
                    _sb.AppendLine(propertyNames.Item2);
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

                _sb.AppendLine($"public {code.Reference(propertyType)}{nullableAnnotation} {propertyNames.Item1} {{ get; set; }}{defaultAnnotation}");
            }
        }

        private Tuple<string, string> GeneratePropertyName(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return CreateIdentifier(propertyName);
        }

        private string GenerateIdentifierName(Function function, FunctionModel model)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            return CreateIdentifier(GenerateUniqueName(function, model)).Item1;
        }

        private Tuple<string, string> CreateIdentifier(string name)
        {
            var isValid = System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);

            string columAttribute = null;

            if (!isValid)
            {
                columAttribute = $"[Column(\"{name}\")]";
                // File name contains invalid chars, remove them
                var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                name = regex.Replace(name, "");

                // Class name doesn't begin with a letter, insert an underscore
                if (!char.IsLetter(name, 0))
                {
                    name = name.Insert(0, "_");
                }
            }

            return new Tuple<string, string>(name.Replace(" ", string.Empty), columAttribute);
        }
    }
}
