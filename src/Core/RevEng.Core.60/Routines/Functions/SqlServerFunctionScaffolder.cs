using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetTopologySuite.Geometries;
using RevEng.Common;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines.Functions
{
    public class SqlServerFunctionScaffolder : FunctionScaffolder, IFunctionScaffolder
    {
        public SqlServerFunctionScaffolder([NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
            : base(code, typeMapper)
        {
            FileNameSuffix = ".Functions";
        }

        protected override string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, RoutineModel model, List<string> schemas)
        {
            ArgumentNullException.ThrowIfNull(scaffolderOptions);

            ArgumentNullException.ThrowIfNull(model);

            Sb = new IndentedStringBuilder();

            Sb.AppendLine(PathHelper.Header);

            Sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            Sb.AppendLine("using System;");
            Sb.AppendLine("using System.Data;");
            Sb.AppendLine("using System.Linq;");
            Sb.AppendLine($"using {scaffolderOptions.ModelNamespace};");

            if (scaffolderOptions.UseSchemaNamespaces)
            {
                schemas.Distinct().OrderBy(s => s).ToList().ForEach(schema => Sb.AppendLine($"using {scaffolderOptions.ModelNamespace}.{schema}"));
            }

            if (model.Routines.SelectMany(r => r.Parameters).Any(p => p.ClrTypeFromSqlParameter() == typeof(Geometry)))
            {
                Sb.AppendLine("using NetTopologySuite.Geometries;");
            }

            Sb.AppendLine();
            Sb.AppendLine($"namespace {scaffolderOptions.ContextNamespace}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                Sb.AppendLine($"public partial class {scaffolderOptions.ContextName}");

                Sb.AppendLine("{");

                using (Sb.Indent())
                {
                    foreach (var function in model.Routines)
                    {
                        GenerateFunctionStub(function, model);
                    }

                    GenerateModelCreation(model);
                }

                Sb.AppendLine("}");
            }

            Sb.AppendLine("}");

            return Sb.ToString();
        }

        private void GenerateModelCreation(RoutineModel model)
        {
            Sb.AppendLine();
            Sb.AppendLine("protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                foreach (var function in model.Routines.Cast<Function>().Where(f => !f.IsScalar))
                {
                    var typeName = ScaffoldHelper.GenerateIdentifierName(function, model) + "Result";

                    Sb.AppendLine($"modelBuilder.Entity<{typeName}>().HasNoKey();");
                }
            }

            Sb.AppendLine("}");
        }

        private void GenerateFunctionStub(Routine function, RoutineModel model)
        {
            var paramStrings = function.Parameters
                .Select(p => $"{Code.Reference(p.ClrTypeFromSqlParameter())} {p.Name}");

            var identifier = ScaffoldHelper.GenerateIdentifierName(function, model);

            Sb.AppendLine();

            Sb.AppendLine($"[DbFunction(\"{function.Name}\", \"{function.Schema}\")]");

            if ((function as Function)!.IsScalar)
            {
                var returnType = paramStrings.First();
                var parameters = string.Empty;

                if (function.Parameters.Count > 1)
                {
                    parameters = string.Join(", ", paramStrings.Skip(1));
                }

                Sb.AppendLine($"public static {returnType}{identifier}({parameters})");

                Sb.AppendLine("{");
                using (Sb.Indent())
                {
                    Sb.AppendLine("throw new NotSupportedException(\"This method can only be called from Entity Framework Core queries\");");
                }

                Sb.AppendLine("}");
            }
            else
            {
                var typeName = $"{identifier}Result";
                var returnType = $"IQueryable<{typeName}>";

                var parameters = string.Empty;

                if (function.Parameters.Count != 0)
                {
                    parameters = string.Join(", ", paramStrings);
                }

                Sb.AppendLine($"public {returnType} {identifier}({parameters})");

                Sb.AppendLine("{");
                using (Sb.Indent())
                {
                    var argumentStrings = function.Parameters
                        .Select(p => p.Name);
                    var arguments = string.Join(", ", argumentStrings);
                    Sb.AppendLine($"return FromExpression(() => {identifier}({arguments}));");
                }

                Sb.AppendLine("}");
            }
        }
    }
}
