using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Modules;
using RevEng.Shared;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RevEng.Core.Procedures
{
    public class SqlServerStoredProcedureScaffolder : SqlServerRoutineScaffolder, IProcedureScaffolder
    {
        private const string parameterPrefix = "parameter";

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


        public SqlServerStoredProcedureScaffolder([NotNull] ICSharpHelper code)
            : base(code)
        {
        }

        public new SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace)
        {
            var files = base.Save(scaffoldedModel, outputDir, nameSpace);

            var contextDir = Path.GetDirectoryName(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            var dbContextExtensionsText = GetDbContextExtensionsText();
            var dbContextExtensionsPath = Path.Combine(contextDir, "DbContextExtensions.cs");
            File.WriteAllText(dbContextExtensionsPath, dbContextExtensionsText.Replace("#NAMESPACE#", nameSpace), Encoding.UTF8);
            files.AdditionalFiles.Add(dbContextExtensionsPath);

            return files;
        }

        private string GetDbContextExtensionsText()
        {
            var assembly = typeof(SqlServerStoredProcedureScaffolder).GetTypeInfo().Assembly;
            using Stream stream = assembly.GetManifestResourceStream("RevEng.Core.DbContextExtensions");
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        protected override string WriteDbContext(ModuleScaffolderOptions procedureScaffolderOptions, RoutineModel model)
        {
            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);

            _sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            _sb.AppendLine("using Microsoft.Data.SqlClient;");
            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Collections.Generic;");
            //To support System.Data.DataTable
            _sb.AppendLine("using System.Data;");
            _sb.AppendLine("using System.Threading;");
            _sb.AppendLine("using System.Threading.Tasks;");
            _sb.AppendLine($"using {procedureScaffolderOptions.ModelNamespace};");

            _sb.AppendLine();
            _sb.AppendLine($"namespace {procedureScaffolderOptions.ContextNamespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                _sb.AppendLine($"public partial class {procedureScaffolderOptions.ContextName}");
                _sb.AppendLine("{");
                using (_sb.Indent())
                {
                    _sb.AppendLine($"private {procedureScaffolderOptions.ContextName}Procedures _procedures;");
                    _sb.AppendLine();
                    _sb.AppendLine($"public virtual {procedureScaffolderOptions.ContextName}Procedures Procedures");
                    _sb.AppendLine("{");
                    using (_sb.Indent())
                    {
                        _sb.AppendLine("get");
                        _sb.AppendLine("{");
                        using (_sb.Indent())
                        {
                            _sb.AppendLine($"if (_procedures is null) _procedures = new {procedureScaffolderOptions.ContextName}Procedures(this);");
                            _sb.AppendLine("return _procedures;");

                        }
                        _sb.AppendLine("}");
                        _sb.AppendLine("set");
                        _sb.AppendLine("{");
                        using (_sb.Indent())
                        {
                            _sb.AppendLine("_procedures = value;");

                        }
                        _sb.AppendLine("}");
                    }
                    _sb.AppendLine("}");
                    _sb.AppendLine("");
                    _sb.AppendLine($"public {procedureScaffolderOptions.ContextName}Procedures GetProcedures()");
                    _sb.AppendLine("{");
                    using (_sb.Indent())
                    {
                        _sb.AppendLine("return Procedures;");
                    }
                    _sb.AppendLine("}");
                }
                _sb.AppendLine("}");
                _sb.AppendLine();

                _sb.AppendLine($"public partial class {procedureScaffolderOptions.ContextName}Procedures");
                _sb.AppendLine("{");

                using (_sb.Indent())
                {
                    _sb.AppendLine($"private readonly {procedureScaffolderOptions.ContextName} _context;");
                    _sb.AppendLine();
                    _sb.AppendLine($"public {procedureScaffolderOptions.ContextName}Procedures({procedureScaffolderOptions.ContextName} context)");
                    _sb.AppendLine("{");

                    using (_sb.Indent())
                    {
                        _sb.AppendLine($"_context = context;");
                    }

                    _sb.AppendLine("}");
                }

                foreach (var procedure in model.Routines)
                {
                    GenerateProcedure(procedure, model);
                }

                _sb.AppendLine("}");
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private void GenerateProcedure(Routine procedure, RoutineModel model)
        {
            var paramStrings = procedure.Parameters.Where(p => !p.Output)
                .Select(p => $"{code.Reference(p.ClrType())} {p.Name}")
                .ToList();

            var allOutParams = procedure.Parameters.Where(p => p.Output).ToList();

            var outParams = allOutParams.SkipLast(1).ToList();

            var retValueName = allOutParams.Last().Name;

            var outParamStrings = outParams
                .Select(p => $"OutputParameter<{code.Reference(p.ClrType())}> {p.Name}")
                .ToList();

            string fullExec = GenerateProcedureStatement(procedure, retValueName);

            var identifier = GenerateIdentifierName(procedure, model);

            var line = GenerateMethodSignature(procedure, outParams, paramStrings, retValueName, outParamStrings, identifier);

            using (_sb.Indent())
            {
                _sb.AppendLine();

                _sb.AppendLine(line);
                _sb.AppendLine("{");

                using (_sb.Indent())
                {
                    foreach (var parameter in allOutParams)
                    {
                        GenerateParameterVar(parameter);
                    }

                    _sb.AppendLine();

                    _sb.AppendLine("var sqlParameters = new []");
                    _sb.AppendLine("{");
                    using (_sb.Indent())
                    {
                        foreach (var parameter in procedure.Parameters)
                        {
                            if (parameter.Output)
                            {
                                _sb.Append($"{parameterPrefix}{parameter.Name}");
                            }
                            else
                            {
                                GenerateParameter(parameter);
                            }
                            _sb.AppendLine(",");
                        }
                    }
                    _sb.AppendLine("};");

                    if (procedure.HasValidResultSet && procedure.ResultElements.Count == 0)
                    {
                        _sb.AppendLine($"var _ = await _context.Database.ExecuteSqlRawAsync({fullExec});");
                    }
                    else
                    {
                        _sb.AppendLine($"var _ = await _context.SqlQueryAsync<{identifier}Result>({fullExec});");
                    }

                    _sb.AppendLine();

                    foreach (var parameter in outParams)
                    {
                        _sb.AppendLine($"{parameter.Name}.SetValue({parameterPrefix}{parameter.Name}.Value);");
                    }

                    _sb.AppendLine($"{retValueName}?.SetValue({parameterPrefix}{retValueName}.Value);");

                    _sb.AppendLine();

                    _sb.AppendLine("return _;");
                }

                _sb.AppendLine("}");
            }
        }

        private static string GenerateProcedureStatement(Routine procedure, string retValueName)
        {
            var paramNames = procedure.Parameters
                .Select(p => $"{parameterPrefix}{p.Name}");

            var paramList = procedure.Parameters
                .Select(p => p.Output ? $"@{p.Name} OUTPUT" : $"@{p.Name}").ToList();

            paramList.RemoveAt(paramList.Count - 1);

            var fullExec = $"\"EXEC @{retValueName} = [{procedure.Schema}].[{procedure.Name}] {string.Join(", ", paramList)}\", sqlParameters, cancellationToken".Replace(" \"", "\"");
            return fullExec;
        }

        private static string GenerateMethodSignature(Routine procedure, List<ModuleParameter> outParams, IEnumerable<string> paramStrings, string retValueName, List<string> outParamStrings, string identifier)
        {
            string returnType;

            if (procedure.HasValidResultSet && procedure.ResultElements.Count == 0)
            {
                returnType = $"Task<int>";
            }
            else
            {
                returnType = $"Task<List<{identifier}Result>>";
            }

            var line = $"public virtual async {returnType} {identifier}Async({string.Join(", ", paramStrings)}";

            if (outParams.Count() > 0)
            {
                if (paramStrings.Count() > 0)
                {
                    line += ", ";
                }

                line += $"{string.Join(", ", outParamStrings)}";
            }

            if (paramStrings.Count() > 0 || outParams.Count > 0)
            {
                line += ", ";
            }

            line += $"OutputParameter<int> {retValueName} = null";

            line += ", CancellationToken cancellationToken = default)";

            return line;
        }

        private void GenerateParameterVar(ModuleParameter parameter)
        {
            _sb.Append($"var {parameterPrefix}{parameter.Name} = ");
            GenerateParameter(parameter);
            _sb.AppendLine(";");
        }

        private void GenerateParameter(ModuleParameter parameter)
        {
            _sb.AppendLine("new SqlParameter");
            _sb.AppendLine("{");

            var sqlDbType = parameter.DbType();

            using (_sb.Indent())
            {
                _sb.AppendLine($"ParameterName = \"{parameter.Name}\",");

                if (_scaleTypes.Contains(sqlDbType))
                {
                    if (parameter.Precision > 0)
                    {
                        _sb.AppendLine($"Precision = {parameter.Precision},");
                    }
                    if (parameter.Scale > 0)
                    {
                        _sb.AppendLine($"Scale = {parameter.Scale},");
                    }
                }

                if (_lengthRequiredTypes.Contains(sqlDbType))
                {
                    _sb.AppendLine($"Size = {parameter.Length},");
                }


                if (!parameter.IsReturnValue)
                {
                    if (parameter.Output)
                    {
                        _sb.AppendLine("Direction = System.Data.ParameterDirection.InputOutput,");
                        AppendValue(parameter);
                    }
                    else
                    {
                        AppendValue(parameter);
                    }
                }
                else
                {
                    _sb.AppendLine("Direction = System.Data.ParameterDirection.Output,");
                }

                _sb.AppendLine($"SqlDbType = System.Data.SqlDbType.{sqlDbType},");

                if (sqlDbType == System.Data.SqlDbType.Structured)
                {
                    _sb.AppendLine($"TypeName = \"{parameter.TypeName}\",");
                }
            }

            _sb.Append("}");
        }

        private void AppendValue(ModuleParameter parameter)
        {

            var value = parameter.Nullable ? $"{parameter.Name} ?? Convert.DBNull" : $"{parameter.Name}";
            if (parameter.Output)
            {
                value = parameter.Nullable ? $"{parameter.Name}?._value ?? Convert.DBNull" : $"{parameter.Name}?._value";
            }
            _sb.AppendLine($"Value = {value},");
        }
    }
}
