﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using NetTopologySuite.Geometries;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Modules;
using RevEng.Common;
using System;
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

        public SqlServerStoredProcedureScaffolder([NotNull] ICSharpHelper code)
            : base(code)
        {
            FileNameSuffix = "Procedures";
        }

        public new SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls)
        {
            if (scaffoldedModel == null)
            {
                throw new ArgumentNullException(nameof(scaffoldedModel));
            }
            var files = base.Save(scaffoldedModel, outputDir, nameSpaceValue, useAsyncCalls);

            var contextDir = Path.GetDirectoryName(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            var dbContextExtensionsText = GetDbContextExtensionsText(useAsyncCalls);
            var dbContextExtensionsName = useAsyncCalls ? "DbContextExtensions.cs": "DbContextExtensions.Sync.cs";
            var dbContextExtensionsPath = Path.Combine(contextDir, dbContextExtensionsName);
            File.WriteAllText(dbContextExtensionsPath, dbContextExtensionsText.Replace("#NAMESPACE#", nameSpaceValue, StringComparison.OrdinalIgnoreCase), Encoding.UTF8);
            files.AdditionalFiles.Add(dbContextExtensionsPath);

            return files;
        }

        private static string GetDbContextExtensionsText(bool useAsyncCalls)
        {
            var dbContextExtensionTemplateName = useAsyncCalls ? "RevEng.Core.DbContextExtensions" : "RevEng.Core.DbContextExtensions.Sync";
            var assembly = typeof(SqlServerStoredProcedureScaffolder).GetTypeInfo().Assembly;
            using Stream stream = assembly.GetManifestResourceStream(dbContextExtensionTemplateName);
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        protected override string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, RoutineModel model)
        {
            if (scaffolderOptions is null)
            {
                throw new ArgumentNullException(nameof(scaffolderOptions));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);

            var usings = CreateUsings(scaffolderOptions, model);

            foreach (var statement in usings)
            {
                _sb.AppendLine($"{statement};");
            }

            _sb.AppendLine();
            _sb.AppendLine($"namespace {scaffolderOptions.ContextNamespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                _sb.AppendLine($"public partial class {scaffolderOptions.ContextName}");
                _sb.AppendLine("{");
                using (_sb.Indent())
                {
                    _sb.AppendLine($"private I{scaffolderOptions.ContextName}Procedures _procedures;");
                    _sb.AppendLine();
                    _sb.AppendLine($"public virtual I{scaffolderOptions.ContextName}Procedures Procedures");
                    _sb.AppendLine("{");
                    using (_sb.Indent())
                    {
                        _sb.AppendLine("get");
                        _sb.AppendLine("{");
                        using (_sb.Indent())
                        {
                            _sb.AppendLine($"if (_procedures is null) _procedures = new {scaffolderOptions.ContextName}Procedures(this);");
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
                    _sb.AppendLine();
                    _sb.AppendLine($"public I{scaffolderOptions.ContextName}Procedures GetProcedures()");
                    _sb.AppendLine("{");
                    using (_sb.Indent())
                    {
                        _sb.AppendLine("return Procedures;");
                    }
                    _sb.AppendLine("}");
                    GenerateOnModelCreating(model);
                }

                _sb.AppendLine("}");
                _sb.AppendLine();

                _sb.AppendLine($"public partial class {scaffolderOptions.ContextName}Procedures : I{scaffolderOptions.ContextName}Procedures");
                _sb.AppendLine("{");

                using (_sb.Indent())
                {
                    _sb.AppendLine($"private readonly {scaffolderOptions.ContextName} _context;");
                    _sb.AppendLine();
                    _sb.AppendLine($"public {scaffolderOptions.ContextName}Procedures({scaffolderOptions.ContextName} context)");
                    _sb.AppendLine("{");

                    using (_sb.Indent())
                    {
                        _sb.AppendLine($"_context = context;");
                    }

                    _sb.AppendLine("}");
                }

                foreach (var procedure in model.Routines)
                {
                    GenerateProcedure(procedure, model, false, scaffolderOptions.UseAsyncCalls);
                }

                if (model.Routines.Any(r => r.SupportsMultipleResultSet))
                {
                    GenerateDapperSupport(scaffolderOptions.UseAsyncCalls);
                }

                _sb.AppendLine("}");
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        protected override string WriteDbContextInterface(ModuleScaffolderOptions scaffolderOptions, RoutineModel model)
        {
            if (scaffolderOptions is null)
            {
                throw new ArgumentNullException(nameof(scaffolderOptions));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);

            var usings = CreateUsings(scaffolderOptions, model);

            foreach (var statement in usings)
            {
                _sb.AppendLine($"{statement};");
            }

            _sb.AppendLine();
            _sb.AppendLine($"namespace {scaffolderOptions.ContextNamespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                _sb.AppendLine($"public partial interface I{scaffolderOptions.ContextName}Procedures");
                _sb.AppendLine("{");
                using (_sb.Indent())
                {
                    foreach (var procedure in model.Routines)
                    {
                        GenerateProcedure(procedure, model, true, scaffolderOptions.UseAsyncCalls);
                        _sb.AppendLine(";");
                    }
                }
                _sb.AppendLine("}");
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private static List<string> CreateUsings(ModuleScaffolderOptions scaffolderOptions, RoutineModel model)
        {
            var usings = new List<string>()
            {
                "using Microsoft.EntityFrameworkCore",
                "using Microsoft.Data.SqlClient",
                "using System",
                "using System.Collections.Generic",
                "using System.Data",
                "using System.Threading",
                "using System.Threading.Tasks",
                $"using {scaffolderOptions.ModelNamespace}",
            };

            if (model.Routines.Any(r => r.SupportsMultipleResultSet))
            {
                usings.AddRange(new List<string>()
                {
                    "using Dapper",
                    "using Microsoft.EntityFrameworkCore.Storage",
                    "using System.Linq",
                });
            }

            if (model.Routines.SelectMany(r => r.Parameters).Any(p => p.ClrType() == typeof(Geometry)))
            {
                usings.AddRange(new List<string>()
                {
                    "using NetTopologySuite.Geometries",
                });

            }

            usings.Sort();
            return usings;
        }

        private void GenerateDapperSupport(bool useAsyncCalls)
        {
            _sb.AppendLine();
            using (_sb.Indent())
            {
                _sb.AppendLine("private static DynamicParameters CreateDynamic(SqlParameter[] sqlParameters)");
                _sb.AppendLine("{");
                using (_sb.Indent())
                {
                    _sb.AppendLine("var dynamic = new DynamicParameters();");
                    _sb.AppendLine("foreach (var sqlParameter in sqlParameters)");
                    _sb.AppendLine("{");
                    using (_sb.Indent())
                    {
                        _sb.AppendLine("dynamic.Add(sqlParameter.ParameterName, sqlParameter.Value, sqlParameter.DbType, sqlParameter.Direction, sqlParameter.Size, sqlParameter.Precision, sqlParameter.Scale);");
                    }
                    _sb.AppendLine("}");
                    _sb.AppendLine();
                    _sb.AppendLine("return dynamic;");
                }
                _sb.AppendLine("}");
            }

            _sb.AppendLine();

            using (_sb.Indent())
            {
                if (useAsyncCalls)
                {
                    _sb.AppendLine("private async Task<SqlMapper.GridReader> GetMultiReaderAsync(DbContext db, DynamicParameters dynamic, string sql)");
                }
                else
                {
                    _sb.AppendLine("private SqlMapper.GridReader GetMultiReader(DbContext db, DynamicParameters dynamic, string sql)");
                }
                _sb.AppendLine("{");
                using (_sb.Indent())
                {
                    _sb.AppendLine("IDbTransaction tran = null;");
                    _sb.AppendLine("if (db.Database.CurrentTransaction is IDbContextTransaction ctxTran)");
                    _sb.AppendLine("{");
                    using (_sb.Indent())
                    {
                        _sb.AppendLine("tran = ctxTran.GetDbTransaction();");
                    }
                    _sb.AppendLine("}");
                    _sb.AppendLine();
                    _sb.AppendLine($"return {(useAsyncCalls ? "await " : "")}((IDbConnection)db.Database.GetDbConnection())");
                    using (_sb.Indent())
                    {
                        _sb.AppendLine($".QueryMultiple{(useAsyncCalls ? "Async" : "")}(sql, dynamic, tran, db.Database.GetCommandTimeout(), CommandType.StoredProcedure);");
                    }
                }
                _sb.AppendLine("}");
            }
        }

        private void GenerateProcedure(Routine procedure, RoutineModel model, bool signatureOnly, bool useAsyncCalls)
        {
            var paramStrings = procedure.Parameters.Where(p => !p.Output)
                .Select(p => $"{code.Reference(p.ClrType(asMethodParameter: true))} {code.Identifier(p.Name)}")
                .ToList();

            var allOutParams = procedure.Parameters.Where(p => p.Output).ToList();

            var outParams = allOutParams.SkipLast(1).ToList();

            var retValueName = allOutParams.Last().Name;

            var outParamStrings = outParams
                .Select(p => $"OutputParameter<{code.Reference(p.ClrType())}> {code.Identifier(p.Name)}")
                .ToList();

            string fullExec = GenerateProcedureStatement(procedure, retValueName, useAsyncCalls);

            var multiResultId = GenerateMultiResultId(procedure, model);

            var identifier = GenerateIdentifierName(procedure, model);

            var returnClass = identifier + "Result";

            if (!string.IsNullOrEmpty(procedure.MappedType))
            {
                returnClass = procedure.MappedType;
            }

            var line = GenerateMethodSignature(procedure, outParams, paramStrings, retValueName, outParamStrings, identifier, multiResultId, signatureOnly, useAsyncCalls, returnClass);

            if (signatureOnly)
            {
                _sb.Append(line);
                return;
            }

            using (_sb.Indent())
            {
                _sb.AppendLine();

                _sb.AppendLine(line);
                _sb.AppendLine("{");

                using (_sb.Indent())
                {
                    foreach (var parameter in allOutParams)
                    {
                        GenerateParameterVar(parameter, procedure);
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
                                GenerateParameter(parameter, procedure);
                            }
                            _sb.AppendLine(",");
                        }
                    }
                    _sb.AppendLine("};");

                    if (procedure.HasValidResultSet && (procedure.Results.Count == 0 || procedure.Results[0].Count == 0))
                    {
                        _sb.AppendLine(useAsyncCalls
                            ? $"var _ = await _context.Database.ExecuteSqlRawAsync({fullExec});"
                            : $"var _ = _context.Database.ExecuteSqlRaw({fullExec});");
                    }
                    else
                    {
                        if (procedure.SupportsMultipleResultSet)
                        {
                            _sb.AppendLine();
                            _sb.AppendLine("var dynamic = CreateDynamic(sqlParameters);");
                            _sb.AppendLine($"{multiResultId}  _;");
                            _sb.AppendLine();
                            _sb.AppendLine($"using (var reader = {(useAsyncCalls ? "await GetMultiReaderAsync" : "GetMultiReader")}(_context, dynamic, \"[{procedure.Schema}].[{procedure.Name}]\"))");
                            _sb.AppendLine("{");

                            using (_sb.Indent())
                            {
                                var statements = GenerateMultiResultStatement(procedure, model, useAsyncCalls);
                                _sb.AppendLine($"_ = {statements};");
                            }

                            _sb.AppendLine("}");
                        }
                        else
                        {
                            _sb.AppendLine(useAsyncCalls 
                                ? $"var _ = await _context.SqlQueryAsync<{returnClass}>({fullExec});"
                                : $"var _ = _context.SqlQuery<{returnClass}>({fullExec});");
                        }
                    }

                    _sb.AppendLine();

                    foreach (var parameter in outParams)
                    {
                        _sb.AppendLine($"{code.Identifier(parameter.Name)}.SetValue({parameterPrefix}{parameter.Name}.Value);");
                    }

                    if (procedure.SupportsMultipleResultSet)
                    {
                        _sb.AppendLine($"{retValueName}?.SetValue(dynamic.Get<int>(\"{retValueName}\"));");
                    }
                    else
                    {
                        _sb.AppendLine($"{retValueName}?.SetValue({parameterPrefix}{retValueName}.Value);");
                    }
                    _sb.AppendLine();

                    _sb.AppendLine("return _;");
                }

                _sb.AppendLine("}");
            }
        }

        private void GenerateOnModelCreating(RoutineModel model)
        {
            _sb.AppendLine();
            _sb.AppendLine("protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                foreach (var procedure in model.Routines.Cast<Procedure>())
                {
                    if (procedure.NoResultSet)
                    {
                        continue;
                    }

                    int i = 1;
                    foreach (var resultSet in procedure.Results)
                    {

                        var suffix = $"{i++}";

                        if (!procedure.SupportsMultipleResultSet)
                        {
                            suffix = string.Empty;
                        }

                        var typeName = GenerateIdentifierName(procedure, model) + "Result" + suffix;

                        _sb.AppendLine($"modelBuilder.Entity<{typeName}>().HasNoKey().ToView(null);");
                    }
                }
            }

            _sb.AppendLine("}");
        }

        private static string GenerateMultiResultId(Routine procedure, RoutineModel model)
        {
            if (procedure.Results.Count == 1)
            {
                return null;
            }

            var ids = new List<string>();
            int i = 1;
            foreach (var resultSet in procedure.Results)
            {
                var suffix = $"{i++}";

                var typeName = GenerateIdentifierName(procedure, model) + "Result" + suffix;
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
            int i = 1;
            foreach (var resultSet in procedure.Results)
            {
                var suffix = $"{i++}";

                var typeName = GenerateIdentifierName(procedure, model) + "Result" + suffix;
                
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
                .Select(p => p.Output ? $"@{p.Name} OUTPUT" : $"@{p.Name}").ToList();

            paramList.RemoveAt(paramList.Count - 1);

            var fullExec = $"\"EXEC @{retValueName} = [{procedure.Schema}].[{procedure.Name}] {string.Join(", ", paramList)}\", sqlParameters{(useAsyncCalls ? ", cancellationToken" : "")}".Replace(" \"", "\"", StringComparison.OrdinalIgnoreCase);
            return fullExec;
        }

        private static string GenerateMethodSignature(Routine procedure, List<ModuleParameter> outParams, IEnumerable<string> paramStrings, string retValueName, List<string> outParamStrings, string identifier, string multiResultId, bool signatureOnly, bool useAsyncCalls, string returnClass)
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

            var lineStart = signatureOnly ? "" : $"public virtual {(useAsyncCalls ? "async " : "")}";
            var line = $"{lineStart}{returnType} {identifier}{(useAsyncCalls ? "Async" : "")}({string.Join(", ", paramStrings)}";

            if (outParams.Any())
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
            _sb.Append($"var {parameterPrefix}{parameter.Name} = ");
            GenerateParameter(parameter, procedure);
            _sb.AppendLine(";");
        }

        private void GenerateParameter(ModuleParameter parameter, Routine procedure)
        {
            _sb.AppendLine("new SqlParameter");
            _sb.AppendLine("{");

            var sqlDbType = parameter.DbType();

            using (_sb.Indent())
            {
                _sb.AppendLine($"ParameterName = \"{code.Identifier(parameter.Name)}\",");

                if (sqlDbType.IsScaleType())
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

                if (sqlDbType.IsVarTimeType() && parameter.Scale > 0)
                {
                    _sb.AppendLine($"Scale = {parameter.Scale},");
                }

                if (sqlDbType.IsLengthRequiredType())
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
                    if (procedure.SupportsMultipleResultSet)
                    {
                        _sb.AppendLine("Direction = System.Data.ParameterDirection.ReturnValue,");
                    }
                    else
                    {
                        _sb.AppendLine("Direction = System.Data.ParameterDirection.Output,");
                    }
                }

                _sb.AppendLine($"SqlDbType = System.Data.SqlDbType.{sqlDbType},");

                if (sqlDbType == SqlDbType.Structured)
                {
                    _sb.AppendLine($"TypeName = \"{parameter.TypeName}\",");
                }

                if (sqlDbType == SqlDbType.Udt)
                {
                    _sb.AppendLine($"UdtTypeName = \"{parameter.TypeName}\",");
                }
            }

            _sb.Append("}");
        }

        private void AppendValue(ModuleParameter parameter)
        {

            var value = parameter.Nullable ? $"{code.Identifier(parameter.Name)} ?? Convert.DBNull" : $"{code.Identifier(parameter.Name)}";
            if (parameter.Output)
            {
                value = parameter.Nullable ? $"{code.Identifier(parameter.Name)}?._value ?? Convert.DBNull" : $"{code.Identifier(parameter.Name)}?._value";
            }
            _sb.AppendLine($"Value = {value},");
        }
    }
}
