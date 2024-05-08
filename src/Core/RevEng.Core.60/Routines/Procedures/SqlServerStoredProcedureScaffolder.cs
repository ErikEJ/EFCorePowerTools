using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines.Procedures
{
    public class SqlServerStoredProcedureScaffolder : ProcedureScaffolder, IProcedureScaffolder
    {
        private const string ParameterPrefix = "parameter";

        public SqlServerStoredProcedureScaffolder([NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
            : base(code, typeMapper)
        {
            FileNameSuffix = "Procedures";
            ProviderUsing = "using Microsoft.Data.SqlClient";
        }

        public new SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls)
        {
            ArgumentNullException.ThrowIfNull(scaffoldedModel);

            var files = base.Save(scaffoldedModel, outputDir, nameSpaceValue, useAsyncCalls);

            var contextDir = Path.GetDirectoryName(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            var dbContextExtensionsText = ScaffoldHelper.GetDbContextExtensionsText(useAsyncCalls);
            var dbContextExtensionsName = useAsyncCalls ? "DbContextExtensions.cs" : "DbContextExtensions.Sync.cs";
            var dbContextExtensionsPath = Path.Combine(contextDir ?? string.Empty, dbContextExtensionsName);
            File.WriteAllText(dbContextExtensionsPath, dbContextExtensionsText.Replace("#NAMESPACE#", nameSpaceValue, StringComparison.OrdinalIgnoreCase), Encoding.UTF8);
            files.AdditionalFiles.Add(dbContextExtensionsPath);

            return files;
        }

        protected override void GenerateProcedure(Routine procedure, RoutineModel model, bool signatureOnly, bool useAsyncCalls)
        {
            ArgumentNullException.ThrowIfNull(procedure);

            var paramStrings = procedure.Parameters.Where(p => !p.Output)
                .Select(p => $"{Code.Reference(p.ClrTypeFromSqlParameter(asMethodParameter: true))} {Code.Identifier(p.Name)}")
                .ToList();

            var allOutParams = procedure.Parameters.Where(p => p.Output).ToList();

            var outParams = allOutParams.SkipLast(1).ToList();

            var retValueName = allOutParams[allOutParams.Count - 1].Name;

            var outParamStrings = outParams
                .Select(p => $"OutputParameter<{Code.Reference(p.ClrTypeFromSqlParameter())}> {Code.Identifier(p.Name)}")
                .ToList();

            var fullExec = GenerateProcedureStatement(procedure, retValueName, useAsyncCalls);

            var multiResultId = GenerateMultiResultId(procedure, model);

            var identifier = ScaffoldHelper.GenerateIdentifierName(procedure, model);

            var returnClass = identifier + "Result";

            if (!string.IsNullOrEmpty(procedure.MappedType))
            {
                returnClass = procedure.MappedType;
            }

            var line = GenerateMethodSignature(procedure, outParams, paramStrings, retValueName, outParamStrings, identifier, multiResultId, signatureOnly, useAsyncCalls, returnClass);

            if (signatureOnly)
            {
                Sb.Append(line);
                return;
            }

            using (Sb.Indent())
            {
                Sb.AppendLine();

                Sb.AppendLine(line);
                Sb.AppendLine("{");

                using (Sb.Indent())
                {
                    foreach (var parameter in allOutParams)
                    {
                        GenerateParameterVar(parameter, procedure);
                    }

                    Sb.AppendLine();

                    Sb.AppendLine("var sqlParameters = new []");
                    Sb.AppendLine("{");
                    using (Sb.Indent())
                    {
                        foreach (var parameter in procedure.Parameters)
                        {
                            if (parameter.Output)
                            {
                                Sb.Append($"{ParameterPrefix}{parameter.Name}");
                            }
                            else
                            {
                                GenerateParameter(parameter, procedure);
                            }

                            Sb.AppendLine(",");
                        }
                    }

                    Sb.AppendLine("};");

                    if (procedure.HasValidResultSet && (procedure.Results.Count == 0 || procedure.Results[0].Count == 0))
                    {
                        Sb.AppendLine(useAsyncCalls
                            ? $"var _ = await _context.Database.ExecuteSqlRawAsync({fullExec});"
                            : $"var _ = _context.Database.ExecuteSqlRaw({fullExec});");
                    }
                    else
                    {
                        if (procedure.SupportsMultipleResultSet)
                        {
                            Sb.AppendLine();
                            Sb.AppendLine("var dynamic = CreateDynamic(sqlParameters);");
                            Sb.AppendLine($"{multiResultId}  _;");
                            Sb.AppendLine();
                            Sb.AppendLine($"using (var reader = {(useAsyncCalls ? "await GetMultiReaderAsync" : "GetMultiReader")}(_context, dynamic, \"[{procedure.Schema}].[{procedure.Name}]\"))");
                            Sb.AppendLine("{");

                            using (Sb.Indent())
                            {
                                var statements = GenerateMultiResultStatement(procedure, model, useAsyncCalls);
                                Sb.AppendLine($"_ = {statements};");
                            }

                            Sb.AppendLine("}");
                        }
                        else
                        {
                            Sb.AppendLine(useAsyncCalls
                                ? $"var _ = await _context.SqlQueryAsync<{returnClass}>({fullExec});"
                                : $"var _ = _context.SqlQuery<{returnClass}>({fullExec});");
                        }
                    }

                    Sb.AppendLine();

                    foreach (var parameter in outParams)
                    {
                        Sb.AppendLine($"{Code.Identifier(parameter.Name)}.SetValue({ParameterPrefix}{parameter.Name}.Value);");
                    }

                    if (procedure.SupportsMultipleResultSet)
                    {
                        Sb.AppendLine($"{retValueName}?.SetValue(dynamic.Get<int>(\"{retValueName}\"));");
                    }
                    else
                    {
                        Sb.AppendLine($"{retValueName}?.SetValue({ParameterPrefix}{retValueName}.Value);");
                    }

                    Sb.AppendLine();

                    Sb.AppendLine("return _;");
                }

                Sb.AppendLine("}");
            }
        }

        private static string GenerateMultiResultId(Routine procedure, RoutineModel model)
        {
            if (procedure.Results.Count == 1)
            {
                return null;
            }

            var ids = new List<string>();
            var i = 1;
            foreach (var resultSet in procedure.Results)
            {
                var suffix = $"{i++}";

                var typeName = ScaffoldHelper.GenerateIdentifierName(procedure, model) + "Result" + suffix;
                ids.Add($"List<{typeName}> Result{suffix}");
            }

            return $"({string.Join(", ", ids)})";
        }

        private static string GenerateMultiResultStatement(Routine procedure, RoutineModel model, bool useAsyncCalls)
        {
            if (procedure.Results.Count == 1)
            {
                return null;
            }

            var ids = new List<string>();
            var i = 1;
            foreach (var resultSet in procedure.Results)
            {
                var suffix = $"{i++}";

                var typeName = ScaffoldHelper.GenerateIdentifierName(procedure, model) + "Result" + suffix;

                if (useAsyncCalls)
                {
                    ids.Add($"(await reader.ReadAsync<{typeName}>()).ToList()");
                }
                else
                {
                    ids.Add($"(reader.Read<{typeName}>()).ToList()");
                }
            }

            return $"({string.Join(", ", ids)})";
        }

        private static string GenerateProcedureStatement(Routine procedure, string retValueName, bool useAsyncCalls)
        {
            var paramList = procedure.Parameters
                .Select(p => $"@{p.Name} = @{p.Name}{(p.Output ? " OUTPUT" : string.Empty)}").ToList();

            paramList.RemoveAt(paramList.Count - 1);

            var fullExec = $"\"EXEC @{retValueName} = [{procedure.Schema}].[{procedure.Name}] {string.Join(", ", paramList)}\", sqlParameters{(useAsyncCalls ? ", cancellationToken" : string.Empty)}".Replace(" \"", "\"", StringComparison.OrdinalIgnoreCase);
            return fullExec;
        }

        private static string GenerateMethodSignature(Routine procedure, List<ModuleParameter> outParams, IList<string> paramStrings, string retValueName, List<string> outParamStrings, string identifier, string multiResultId, bool signatureOnly, bool useAsyncCalls, string returnClass)
        {
            string returnType;
            if (procedure.HasValidResultSet && (procedure.Results.Count == 0 || procedure.Results[0].Count == 0))
            {
                returnType = $"int";
            }
            else
            {
                if (procedure.SupportsMultipleResultSet)
                {
                    returnType = multiResultId;
                }
                else
                {
                    returnType = $"List<{returnClass}>";
                }
            }

            returnType = useAsyncCalls ? $"Task<{returnType}>" : returnType;

            var lineStart = signatureOnly ? string.Empty : $"public virtual {(useAsyncCalls ? "async " : string.Empty)}";
            var line = $"{lineStart}{returnType} {identifier}{(useAsyncCalls ? "Async" : string.Empty)}({string.Join(", ", paramStrings)}";

            if (outParams.Count > 0)
            {
                if (paramStrings.Any())
                {
                    line += ", ";
                }

                line += $"{string.Join(", ", outParamStrings)}";
            }

            if (paramStrings.Any() || outParams.Count > 0)
            {
                line += ", ";
            }

            line += $"OutputParameter<int> {retValueName} = null";

            line += useAsyncCalls ? ", CancellationToken cancellationToken = default)" : ")";

            return line;
        }

        private void GenerateParameterVar(ModuleParameter parameter, Routine procedure)
        {
            Sb.Append($"var {ParameterPrefix}{parameter.Name} = ");
            GenerateParameter(parameter, procedure);
            Sb.AppendLine(";");
        }

        private void GenerateParameter(ModuleParameter parameter, Routine procedure)
        {
            Sb.AppendLine("new SqlParameter");
            Sb.AppendLine("{");

            var sqlDbType = parameter.GetSqlDbType();

            using (Sb.Indent())
            {
                Sb.AppendLine($"ParameterName = \"{parameter.Name}\",");

                if (sqlDbType.IsScaleType())
                {
                    if (parameter.Precision > 0)
                    {
                        Sb.AppendLine($"Precision = {parameter.Precision},");
                    }

                    if (parameter.Scale > 0)
                    {
                        Sb.AppendLine($"Scale = {parameter.Scale},");
                    }
                }

                if (sqlDbType.IsVarTimeType() && parameter.Scale > 0)
                {
                    Sb.AppendLine($"Scale = {parameter.Scale},");
                }

                if (sqlDbType.IsLengthRequiredType())
                {
                    Sb.AppendLine($"Size = {parameter.Length},");
                }

                if (!parameter.IsReturnValue)
                {
                    if (parameter.Output)
                    {
                        Sb.AppendLine("Direction = System.Data.ParameterDirection.InputOutput,");
                        AppendValue(parameter);
                    }
                    else
                    {
                        AppendValue(parameter);
                    }
                }
                else
                {
                    if (procedure.SupportsMultipleResultSet)
                    {
                        Sb.AppendLine("Direction = System.Data.ParameterDirection.ReturnValue,");
                    }
                    else
                    {
                        Sb.AppendLine("Direction = System.Data.ParameterDirection.Output,");
                    }
                }

                Sb.AppendLine($"SqlDbType = System.Data.SqlDbType.{sqlDbType},");

                if (sqlDbType == SqlDbType.Structured)
                {
                    Sb.AppendLine($"TypeName = \"{parameter.TypeName}\",");
                }

                if (sqlDbType == SqlDbType.Udt)
                {
                    Sb.AppendLine($"UdtTypeName = \"{parameter.TypeName}\",");
                }
            }

            Sb.Append("}");
        }

        private void AppendValue(ModuleParameter parameter)
        {
            var value = parameter.Nullable ? $"{Code.Identifier(parameter.Name)} ?? Convert.DBNull" : $"{Code.Identifier(parameter.Name)}";
            if (parameter.Output)
            {
                value = parameter.Nullable ? $"{Code.Identifier(parameter.Name)}?._value ?? Convert.DBNull" : $"{Code.Identifier(parameter.Name)}?._value";
            }

            Sb.AppendLine($"Value = {value},");
        }
    }
}
