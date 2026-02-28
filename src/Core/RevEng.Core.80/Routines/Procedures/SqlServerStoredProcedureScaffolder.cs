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
        private bool useTypedTvpParameters;

        public SqlServerStoredProcedureScaffolder([NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
            : base(code, typeMapper)
        {
            FileNameSuffix = "Procedures";
            ProviderUsing = "using Microsoft.Data.SqlClient";
        }

        public new SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls, bool useInternalAccessModifier, bool useNullableReferences)
        {
            ArgumentNullException.ThrowIfNull(scaffoldedModel);

            var files = base.Save(scaffoldedModel, outputDir, nameSpaceValue, useAsyncCalls, useInternalAccessModifier, useNullableReferences);
            var accessModifier = useInternalAccessModifier ? "internal" : "public";

            var contextDir = Path.GetDirectoryName(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            var dbContextExtensionsText = ScaffoldHelper.GetDbContextExtensionsText(useAsyncCalls);
            var dbContextExtensionsName = useAsyncCalls ? "DbContextExtensions.cs" : "DbContextExtensions.Sync.cs";
            var dbContextExtensionsPath = Path.Combine(contextDir ?? string.Empty, dbContextExtensionsName);
            File.WriteAllText(
                dbContextExtensionsPath,
                dbContextExtensionsText
                    .Replace("#NAMESPACE#", nameSpaceValue, StringComparison.OrdinalIgnoreCase)
                    .Replace("#ACCESSMODIFIER#", accessModifier, StringComparison.OrdinalIgnoreCase)
                    .Replace("#NULLABLE#", useNullableReferences ? "?" : string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Replace("#NULLABLEENABLE#", useNullableReferences ? "enable" : "disable", StringComparison.OrdinalIgnoreCase),
                Encoding.UTF8);
            files.AdditionalFiles.Add(dbContextExtensionsPath);

            return files;
        }

        protected override void GenerateProcedure(Routine procedure, RoutineModel model, bool signatureOnly, bool useAsyncCalls, bool usePascalCase, bool useNullableReferences, bool useTypedTvpParameters)
        {
            ArgumentNullException.ThrowIfNull(procedure);

            // Store for use in helper methods
            this.useTypedTvpParameters = useTypedTvpParameters;

            var paramStrings = procedure.Parameters.Where(p => !p.Output)
                .Select(p =>
                {
                    var type = Code.Reference(p.ClrTypeFromSqlParameter(asMethodParameter: true));

                    // For structured (TVP) parameters, use strongly-typed IEnumerable if enabled
                    if (useTypedTvpParameters && p.GetSqlDbType() == SqlDbType.Structured && p.TvpColumns?.Count > 0)
                    {
                        var tvpTypeName = Code.Identifier(ScaffoldHelper.CreateIdentifier(p.TypeName).Item1, capitalize: true);
                        type = $"IEnumerable<{tvpTypeName}>";
                    }

                    if (useNullableReferences && !type.EndsWith('?'))
                    {
                        type += '?';
                    }

                    return $"{type} {Code.Identifier(p.Name, capitalize: false)}";
                })
                .ToList();

            var allOutParams = procedure.Parameters.Where(p => p.Output).ToList();

            var outParams = allOutParams.SkipLast(1).ToList();

            var retValueName = allOutParams[allOutParams.Count - 1].Name;

            // Check for naming conflicts with the return value parameter
            var allNonReturnParamNames = procedure.Parameters
                .Where(p => !p.IsReturnValue)
                .Select(p => Code.Identifier(p.Name, capitalize: false))
                .ToHashSet();

            // Also collect all output parameter identifiers (excluding the return value itself)
            var outParamIdentifiers = outParams
                .Select(p => Code.Identifier(p.Name, capitalize: false))
                .ToHashSet();

            // Combine all names that should not be duplicated
            var allUsedIdentifiers = new HashSet<string>(allNonReturnParamNames);
            foreach (var outParamId in outParamIdentifiers)
            {
                allUsedIdentifiers.Add(outParamId);
            }

            // If there's a conflict, append a suffix to make it unique
            var retValueIdentifier = Code.Identifier(retValueName, capitalize: false);
            if (allUsedIdentifiers.Contains(retValueIdentifier))
            {
                var suffix = 1;
                while (allUsedIdentifiers.Contains($"{retValueIdentifier}{suffix}"))
                {
                    suffix++;
                }

                retValueIdentifier = $"{retValueIdentifier}{suffix}";
            }

            var outParamStrings = outParams
                .Select(p => $"OutputParameter<{Code.Reference(p.ClrTypeFromSqlParameter())}> {Code.Identifier(p.Name, capitalize: false)}")
                .ToList();

            var fullExec = GenerateProcedureStatement(procedure, retValueName, useAsyncCalls);

            var multiResultId = GenerateMultiResultId(procedure, model, usePascalCase);

            var identifier = ScaffoldHelper.GenerateIdentifierName(procedure, model, Code, usePascalCase);

            var returnClass = identifier + "Result";

            if (!string.IsNullOrEmpty(procedure.MappedType))
            {
                returnClass = procedure.MappedType;
            }

            var line = GenerateMethodSignature(procedure, outParams, paramStrings, retValueIdentifier, outParamStrings, identifier, multiResultId, signatureOnly, useAsyncCalls, returnClass, useNullableReferences);

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
                                var statements = GenerateMultiResultStatement(procedure, model, useAsyncCalls, usePascalCase);
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

                    foreach (var name in outParams.Select(p => p.Name))
                    {
                        Sb.AppendLine($"{Code.Identifier(name, capitalize: false)}?.SetValue({ParameterPrefix}{name}.Value);");
                    }

                    if (procedure.SupportsMultipleResultSet)
                    {
                        Sb.AppendLine($"{retValueIdentifier}?.SetValue(dynamic.Get<int>(\"{retValueName}\"));");
                    }
                    else
                    {
                        Sb.AppendLine($"{retValueIdentifier}?.SetValue({ParameterPrefix}{retValueName}.Value);");
                    }

                    Sb.AppendLine();

                    Sb.AppendLine("return _;");
                }

                Sb.AppendLine("}");
            }
        }

        private static string GenerateProcedureStatement(Routine procedure, string retValueName, bool useAsyncCalls)
        {
            var paramList = procedure.Parameters
                .Select(p => $"@{p.Name} = @{p.Name}{(p.Output ? " OUTPUT" : string.Empty)}").ToList();

            paramList.RemoveAt(paramList.Count - 1);

            var fullExec = $"\"EXEC @{retValueName} = [{procedure.Schema}].[{procedure.Name}] {string.Join(", ", paramList)}\", sqlParameters{(useAsyncCalls ? ", cancellationToken" : string.Empty)}".Replace(" \"", "\"", StringComparison.OrdinalIgnoreCase);
            return fullExec;
        }

        private static string GenerateMethodSignature(
            Routine procedure,
            List<ModuleParameter> outParams,
            IList<string> paramStrings,
            string retValueIdentifier,
            List<string> outParamStrings,
            string identifier,
            string multiResultId,
            bool signatureOnly,
            bool useAsyncCalls,
            string returnClass,
            bool useNullableReferences)
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

                // Do not add nullable annotation to List<T> when nullable references are enabled
                // The list itself is never null (empty list instead)
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

            var nullable = useNullableReferences ? "?" : string.Empty;

            line += $"OutputParameter<int>{nullable} {retValueIdentifier} = null";

            line += useAsyncCalls ? $", CancellationToken cancellationToken = default)" : ")";

            return line;
        }

        private string GenerateMultiResultId(Routine procedure, RoutineModel model, bool usePascalCase)
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

                var typeName = ScaffoldHelper.GenerateIdentifierName(procedure, model, Code, usePascalCase) + "Result" + suffix;
                ids.Add($"List<{typeName}> Result{suffix}");
            }

            return $"({string.Join(", ", ids)})";
        }

        private string GenerateMultiResultStatement(Routine procedure, RoutineModel model, bool useAsyncCalls, bool usePascalCase)
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

                var typeName = ScaffoldHelper.GenerateIdentifierName(procedure, model, Code, usePascalCase) + "Result" + suffix;

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
                    Sb.AppendLine($"TypeName = \"[{parameter.TypeSchemaName}].[{parameter.TypeName}]\",");
                }

                if (sqlDbType == SqlDbType.Udt)
                {
                    Sb.AppendLine($"UdtTypeName = \"[{parameter.TypeSchemaName}].[{parameter.TypeName}]\",");
                }
            }

            Sb.Append("}");
        }

        private void AppendValue(ModuleParameter parameter)
        {
            var name = Code.Identifier(parameter.Name, capitalize: false);

            // For structured (TVP) parameters with typed TVP support, call ToDataTable()
            if (useTypedTvpParameters && parameter.GetSqlDbType() == SqlDbType.Structured && parameter.TvpColumns?.Count > 0)
            {
                var value = parameter.Nullable ? $"ToDataTable({name}) ?? Convert.DBNull" : $"ToDataTable({name})";
                Sb.AppendLine($"Value = {name} == null ? Convert.DBNull : {value},");
            }
            else
            {
                var value = parameter.Nullable ? $"{name} ?? Convert.DBNull" : $"{name}";
                if (parameter.Output)
                {
                    value = parameter.Nullable ? $"{name}?._value ?? Convert.DBNull" : $"{name}?._value";
                }

                Sb.AppendLine($"Value = {value},");
            }
        }
    }
}