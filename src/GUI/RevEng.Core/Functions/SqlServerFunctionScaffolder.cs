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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace RevEng.Core.Functions
{
    public class SqlServerFunctionScaffolder : IFunctionScaffolder
    {
        private const string parameterPrefix = "parameter";

        private readonly ICSharpHelper code;

        private static readonly ISet<SqlDbType> _scaleTypes = new HashSet<SqlDbType>
        {
            SqlDbType.DateTimeOffset,
            SqlDbType.DateTime2,
            SqlDbType.Time,
            SqlDbType.Decimal,
        };

        private static readonly ISet<SqlDbType> _lengthRequiredTypes = new HashSet<SqlDbType>
        {
            SqlDbType.Binary,
            SqlDbType.VarBinary,
            SqlDbType.Char,
            SqlDbType.VarChar,
            SqlDbType.NChar,
            SqlDbType.NVarChar,
        };

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
                Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + "Functions.cs")),
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
            _sb.AppendLine("using System.Data;");

            _sb.AppendLine();
            _sb.AppendLine($"namespace {procedureScaffolderOptions.ContextNamespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                _sb.AppendLine($"public partial static class {procedureScaffolderOptions.ContextName}Functions");

                _sb.AppendLine("{");

                foreach (var function in model.Functions)
                {
                    GenerateFunctionStub(function, model);
                }

                _sb.AppendLine("}");
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private void GenerateFunctionStub(Function function, FunctionModel model)
        {
            var paramStrings = function.Parameters.Where(p => !p.Output)
                .Select(p => $"{code.Reference(p.ClrType())} {p.Name}");

            var identifier = GenerateIdentifierName(function, model);

            //var line = GenerateMethodSignature(procedure, outParams, paramStrings, retValueName, outParamStrings, identifier);
        }

        //private static string GenerateMethodSignature(Procedure procedure, List<ModuleParameter> outParams, IEnumerable<string> paramStrings, string retValueName, List<string> outParamStrings, string identifier)
        //{
        //    var returnType = $"Task<{identifier}Result[]>";

        //    if (procedure.HasValidResultSet && procedure.ResultElements.Count == 0)
        //    {
        //        returnType = $"Task<int>";
        //    }

        //    var line = $"public async {returnType} {identifier}Async({string.Join(", ", paramStrings)}";

        //    if (outParams.Count() > 0)
        //    {
        //        if (paramStrings.Count() > 0)
        //        {
        //            line += ", ";
        //        }

        //        line += $"{string.Join(", ", outParamStrings)}";
        //    }

        //    if (paramStrings.Count() > 0 || outParams.Count > 0)
        //    {
        //        line += ", ";
        //    }

        //    line += $"OutputParameter<int> {retValueName} = null";

        //    line += ", CancellationToken cancellationToken = default)";

        //    return line;
        //}

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
