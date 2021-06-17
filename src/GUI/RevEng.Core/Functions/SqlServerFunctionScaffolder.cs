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

        protected override string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, ModuleModel model)
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

        private void GenerateModelCreation(ModuleModel model)
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

        private void GenerateFunctionStub(Function function, ModuleModel model)
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
    }
}
