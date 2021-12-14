using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Modules;
using RevEng.Shared;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RevEng.Core.Functions
{
    public class SqlServerFunctionScaffolder : SqlServerRoutineScaffolder, IFunctionScaffolder
    {
        public SqlServerFunctionScaffolder([NotNull] ICSharpHelper code)
            : base(code)
        {
        }

        protected override string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, RoutineModel model)
        {
            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);

            _sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Data;");
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
                    foreach (var function in model.Routines)
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

        private void GenerateModelCreation(RoutineModel model)
        {
            _sb.AppendLine();
            _sb.AppendLine("protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                foreach (var function in model.Routines.Cast<Function>().Where(f => !f.IsScalar))
                {
                    var typeName = GenerateIdentifierName(function, model) + "Result";

                    _sb.AppendLine($"modelBuilder.Entity<{typeName}>().HasNoKey();");
                }
            }

            _sb.AppendLine("}");
        }

        private void GenerateFunctionStub(Routine function, RoutineModel model)
        {
            var paramStrings = function.Parameters
                .Select(p => $"{code.Reference(p.ClrType())} {p.Name}");

            var identifier = GenerateIdentifierName(function, model);

            _sb.AppendLine();

            _sb.AppendLine($"[DbFunction(\"{function.Name}\", \"{function.Schema}\")]");

            if ((function as Function)!.IsScalar)
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
