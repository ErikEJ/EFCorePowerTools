using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RevEng.Core.Functions
{
    public class SqlServerFunctionScaffolder : IFunctionScaffolder
    {
        private readonly ICSharpHelper code;

        private IndentedStringBuilder _sb;

        public SqlServerFunctionScaffolder([NotNull] ICSharpHelper code)
        {
            this.code = code;
        }

        public ScaffoldedModel ScaffoldModel(FunctionModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var result = new ScaffoldedModel();

            errors = model.Errors;

            var dbContext = WriteFunctionsClass(scaffolderOptions, model);

            result.ContextFile = new ScaffoldedFile
            {
                Code = dbContext,
                Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + ".Functions.cs")),
            };

            return result;
        }

        public SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace)
        {
            Directory.CreateDirectory(outputDir);

            var contextPath = Path.GetFullPath(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            Directory.CreateDirectory(Path.GetDirectoryName(contextPath));
            File.WriteAllText(contextPath, scaffoldedModel.ContextFile.Code, Encoding.UTF8);

            var additionalFiles = new List<string>();

            return new SavedModelFiles(contextPath, additionalFiles);
        }

        private string WriteFunctionsClass(ModuleScaffolderOptions procedureScaffolderOptions, FunctionModel model)
        {
            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);

            _sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            _sb.AppendLine("using System;");

            _sb.AppendLine();
            _sb.AppendLine($"namespace {procedureScaffolderOptions.ContextNamespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                _sb.AppendLine($"public partial class {procedureScaffolderOptions.ContextName}");

                _sb.AppendLine("{");

                using (_sb.Indent())
                {
                    foreach (var function in model.Functions)
                    {
                        GenerateFunctionStub(function, model);
                    }
                }
                _sb.AppendLine("}");
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private void GenerateFunctionStub(Function function, FunctionModel model)
        {
            var paramStrings = function.Parameters
                .Select(p => $"{code.Reference(p.ClrType())} {p.Name}");

            var identifier = GenerateIdentifierName(function, model);

            _sb.AppendLine();

            _sb.AppendLine($"[DbFunction(\"{function.Name}\", \"{function.Schema}\")]");

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

        private string GenerateIdentifierName(Function function, FunctionModel model)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            return CreateIdentifier(GenerateUniqueName(function, model));
        }

        private string CreateIdentifier(string name)
        {
            var isValid = System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);

            if (!isValid)
            {
                // File name contains invalid chars, remove them
                var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                name = regex.Replace(name, "");

                // Class name doesn't begin with a letter, insert an underscore
                if (!char.IsLetter(name, 0))
                {
                    name = name.Insert(0, "_");
                }
            }

            return name.Replace(" ", string.Empty);
        }

        private string GenerateUniqueName(Function function, FunctionModel model)
        {
            var numberOfNames = model.Functions.Where(p => p.Name == function.Name).Count();

            if (numberOfNames > 1)
            {
                return function.Name + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(function.Schema);
            }

            return function.Name;
        }
    }
}
