using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using NetTopologySuite.Geometries;
using RevEng.Common;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines.Procedures
{
    public abstract class ProcedureScaffolder : IRoutineScaffolder
    {
#pragma warning disable SA1401 // Fields should be private
        internal readonly ICSharpHelper Code;
        internal IndentedStringBuilder Sb;
#pragma warning restore SA1401 // Fields should be private
        private readonly IClrTypeMapper typeMapper;
        private readonly Scaffolder scaffolder;

        protected ProcedureScaffolder([System.Diagnostics.CodeAnalysis.NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
        {
            ArgumentNullException.ThrowIfNull(code);
            Code = code;
            this.typeMapper = typeMapper;
            scaffolder = new Scaffolder(code, typeMapper);
        }

        public string FileNameSuffix { get; set; }

        public string ProviderUsing { get; set; }

        public SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls, bool useInternalAccessModifier)
        {
            return ScaffoldHelper.Save(scaffoldedModel, outputDir, nameSpaceValue, useAsyncCalls);
        }

        public ScaffoldedModel ScaffoldModel(RoutineModel model, ModuleScaffolderOptions scaffolderOptions, List<string> schemas, ref List<string> errors)
        {
            ArgumentNullException.ThrowIfNull(model);

            ArgumentNullException.ThrowIfNull(errors);
            ArgumentNullException.ThrowIfNull(scaffolderOptions);

            var result = new ScaffoldedModel();
            var path = string.Empty;

            errors.AddRange(model.Errors);

            schemas = schemas ?? new List<string>();

            foreach (var routine in model.Routines.Where(r => string.IsNullOrEmpty(r.MappedType) && (!(r is Function f) || !f.IsScalar)))
            {
                var i = 1;

                foreach (var resultSet in routine.Results)
                {
                    if (routine.NoResultSet)
                    {
                        continue;
                    }

                    var suffix = string.Empty;
                    if (routine.Results.Count > 1)
                    {
                        suffix = $"{i++}";
                    }

                    var typeName = ScaffoldHelper.GenerateIdentifierName(routine, model, Code, scaffolderOptions.UsePascalIdentifiers) + "Result" + suffix;

                    var classContent = scaffolder.WriteResultClass(resultSet, scaffolderOptions, typeName, routine.Schema);

                    if (!string.IsNullOrEmpty(routine.Schema))
                    {
                        schemas.Add($"{routine.Schema}Schema");
                    }

                    path = scaffolderOptions.UseSchemaFolders
                                ? Path.Combine(routine.Schema, $"{typeName}.cs")
                                : $"{typeName}.cs";
#if CORE90
                    result.AdditionalFiles.Add(new ScaffoldedFile(path, classContent));
#else
                    result.AdditionalFiles.Add(new ScaffoldedFile
                    {
                        Code = classContent,
                        Path = path,
                    });
#endif
                }
            }

            var dbContextInterface = WriteDbContextInterface(scaffolderOptions, model, schemas.Distinct().ToList());

            if (!string.IsNullOrEmpty(dbContextInterface))
            {
                path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, $"I{scaffolderOptions.ContextName}{FileNameSuffix}.cs"));
#if CORE90
                result.AdditionalFiles.Add(new ScaffoldedFile(path, dbContextInterface));
#else
                result.AdditionalFiles.Add(new ScaffoldedFile
                {
                    Code = dbContextInterface,
                    Path = path,
                });
#endif
            }

            var dbContext = WriteDbContext(scaffolderOptions, model, schemas.Distinct().ToList());

            path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + $"{FileNameSuffix}.cs"));
#if CORE90
            result.ContextFile = new ScaffoldedFile(path, dbContext);
#else
            result.ContextFile = new ScaffoldedFile
            {
                Code = dbContext,
                Path = path,
            };
#endif
            return result;
        }

        protected abstract void GenerateProcedure(Routine procedure, RoutineModel model, bool signatureOnly, bool useAsyncCalls, bool usePascalCase);

        private List<string> CreateUsings(ModuleScaffolderOptions scaffolderOptions, RoutineModel model, List<string> schemas)
        {
            var usings = new List<string>()
            {
                "using Microsoft.EntityFrameworkCore",
                "using System",
                "using System.Collections.Generic",
                "using System.Data",
                "using System.Threading",
                "using System.Threading.Tasks",
                $"using {scaffolderOptions.ModelNamespace}",
            };

            usings.Add(ProviderUsing);

            if (scaffolderOptions.UseSchemaNamespaces)
            {
                schemas.Distinct().OrderBy(s => s).ToList().ForEach(schema => usings.Add($"using {scaffolderOptions.ModelNamespace}.{schema}"));
            }

            if (model.Routines.Exists(r => r.SupportsMultipleResultSet))
            {
                usings.AddRange(new List<string>()
                {
                    "using Dapper",
                    "using Microsoft.EntityFrameworkCore.Storage",
                    "using System.Linq",
                });
            }

            if (model.Routines.SelectMany(r => r.Parameters).Any(p => typeMapper.GetClrType(p) == typeof(Geometry)))
            {
                usings.AddRange(new List<string>()
                {
                    "using NetTopologySuite.Geometries",
                });
            }

            usings.Sort();
            return usings;
        }

        private string WriteDbContextInterface(ModuleScaffolderOptions scaffolderOptions, RoutineModel model, List<string> schemas)
        {
            ArgumentNullException.ThrowIfNull(scaffolderOptions);

            ArgumentNullException.ThrowIfNull(model);
            string accessModifier = scaffolderOptions.UseInternalAccessModifier ? "internal" : "public";

            Sb = new IndentedStringBuilder();

            Sb.AppendLine(PathHelper.Header);
            Sb.AppendLine("#nullable disable"); // procedure parameters are always nullable

            var usings = CreateUsings(scaffolderOptions, model, schemas);

            foreach (var statement in usings)
            {
                Sb.AppendLine($"{statement};");
            }

            Sb.AppendLine();
            Sb.AppendLine($"namespace {scaffolderOptions.ContextNamespace}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                Sb.AppendLine($"{accessModifier} partial interface I{scaffolderOptions.ContextName}{FileNameSuffix}");
                Sb.AppendLine("{");
                using (Sb.Indent())
                {
                    foreach (var procedure in model.Routines)
                    {
                        GenerateProcedure(procedure, model, true, scaffolderOptions.UseAsyncCalls, scaffolderOptions.UsePascalIdentifiers);
                        Sb.AppendLine(";");
                    }
                }

                Sb.AppendLine("}");
            }

            Sb.AppendLine("}");

            return Sb.ToString();
        }

        private string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, RoutineModel model, List<string> schemas)
        {
            ArgumentNullException.ThrowIfNull(scaffolderOptions);

            ArgumentNullException.ThrowIfNull(model);

            string accessModifier = scaffolderOptions.UseInternalAccessModifier ? "internal" : "public";

            Sb = new IndentedStringBuilder();

            Sb.AppendLine(PathHelper.Header);
            Sb.AppendLine("#nullable disable"); // procedure parameters are always nullable

            var usings = CreateUsings(scaffolderOptions, model, schemas);

            foreach (var statement in usings)
            {
                Sb.AppendLine($"{statement};");
            }

            Sb.AppendLine();
            Sb.AppendLine($"namespace {scaffolderOptions.ContextNamespace}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                Sb.AppendLine($"{accessModifier} partial class {scaffolderOptions.ContextName}");
                Sb.AppendLine("{");
                using (Sb.Indent())
                {
                    Sb.AppendLine($"private I{scaffolderOptions.ContextName}{FileNameSuffix} _procedures;");
                    Sb.AppendLine();
                    Sb.AppendLine($"public virtual I{scaffolderOptions.ContextName}{FileNameSuffix} {FileNameSuffix}");
                    Sb.AppendLine("{");
                    using (Sb.Indent())
                    {
                        Sb.AppendLine("get");
                        Sb.AppendLine("{");
                        using (Sb.Indent())
                        {
                            Sb.AppendLine($"if (_procedures is null) _procedures = new {scaffolderOptions.ContextName}{FileNameSuffix}(this);");
                            Sb.AppendLine("return _procedures;");
                        }

                        Sb.AppendLine("}");
                        Sb.AppendLine("set");
                        Sb.AppendLine("{");
                        using (Sb.Indent())
                        {
                            Sb.AppendLine("_procedures = value;");
                        }

                        Sb.AppendLine("}");
                    }

                    Sb.AppendLine("}");
                    Sb.AppendLine();
                    Sb.AppendLine($"public I{scaffolderOptions.ContextName}{FileNameSuffix} Get{FileNameSuffix}()");
                    Sb.AppendLine("{");
                    using (Sb.Indent())
                    {
                        Sb.AppendLine($"return {FileNameSuffix};");
                    }

                    Sb.AppendLine("}");
                }

                Sb.AppendLine("}");
                Sb.AppendLine();

                Sb.AppendLine($"{accessModifier} partial class {scaffolderOptions.ContextName}{FileNameSuffix} : I{scaffolderOptions.ContextName}{FileNameSuffix}");
                Sb.AppendLine("{");

                using (Sb.Indent())
                {
                    Sb.AppendLine($"private readonly {scaffolderOptions.ContextName} _context;");
                    Sb.AppendLine();
                    Sb.AppendLine($"public {scaffolderOptions.ContextName}{FileNameSuffix}({scaffolderOptions.ContextName} context)");
                    Sb.AppendLine("{");

                    using (Sb.Indent())
                    {
                        Sb.AppendLine($"_context = context;");
                    }

                    Sb.AppendLine("}");
                }

                foreach (var procedure in model.Routines)
                {
                    GenerateProcedure(procedure, model, false, scaffolderOptions.UseAsyncCalls, scaffolderOptions.UsePascalIdentifiers);
                }

                if (model.Routines.Exists(r => r.SupportsMultipleResultSet))
                {
                    GenerateDapperSupport(scaffolderOptions.UseAsyncCalls);
                }

                Sb.AppendLine("}");
            }

            Sb.AppendLine("}");

            return Sb.ToString();
        }

        private void GenerateDapperSupport(bool useAsyncCalls)
        {
            Sb.AppendLine();
            using (Sb.Indent())
            {
                Sb.AppendLine("private static DynamicParameters CreateDynamic(SqlParameter[] sqlParameters)");
                Sb.AppendLine("{");
                using (Sb.Indent())
                {
                    Sb.AppendLine("var dynamic = new DynamicParameters();");
                    Sb.AppendLine("foreach (var sqlParameter in sqlParameters)");
                    Sb.AppendLine("{");
                    using (Sb.Indent())
                    {
                        Sb.AppendLine("dynamic.Add(sqlParameter.ParameterName, sqlParameter.Value, sqlParameter.DbType, sqlParameter.Direction, sqlParameter.Size, sqlParameter.Precision, sqlParameter.Scale);");
                    }

                    Sb.AppendLine("}");
                    Sb.AppendLine();
                    Sb.AppendLine("return dynamic;");
                }

                Sb.AppendLine("}");
            }

            Sb.AppendLine();

            using (Sb.Indent())
            {
                if (useAsyncCalls)
                {
                    Sb.AppendLine("private async Task<SqlMapper.GridReader> GetMultiReaderAsync(DbContext db, DynamicParameters dynamic, string sql)");
                }
                else
                {
                    Sb.AppendLine("private SqlMapper.GridReader GetMultiReader(DbContext db, DynamicParameters dynamic, string sql)");
                }

                Sb.AppendLine("{");
                using (Sb.Indent())
                {
                    Sb.AppendLine("IDbTransaction tran = null;");
                    Sb.AppendLine("if (db.Database.CurrentTransaction is IDbContextTransaction ctxTran)");
                    Sb.AppendLine("{");
                    using (Sb.Indent())
                    {
                        Sb.AppendLine("tran = ctxTran.GetDbTransaction();");
                    }

                    Sb.AppendLine("}");
                    Sb.AppendLine();
                    Sb.AppendLine($"return {(useAsyncCalls ? "await " : string.Empty)}((IDbConnection)db.Database.GetDbConnection())");
                    using (Sb.Indent())
                    {
                        Sb.AppendLine($".QueryMultiple{(useAsyncCalls ? "Async" : string.Empty)}(sql, dynamic, tran, db.Database.GetCommandTimeout(), CommandType.StoredProcedure);");
                    }
                }

                Sb.AppendLine("}");
            }
        }
    }
}
