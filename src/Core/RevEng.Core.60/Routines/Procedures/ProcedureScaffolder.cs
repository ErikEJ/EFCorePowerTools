using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        protected ProcedureScaffolder([System.Diagnostics.CodeAnalysis.NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
        {
            ArgumentNullException.ThrowIfNull(code);

            Code = code;
            this.typeMapper = typeMapper;
        }

        public string FileNameSuffix { get; set; }

        public string ProviderUsing { get; set; }

        public SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue, bool useAsyncCalls)
        {
            ArgumentNullException.ThrowIfNull(scaffoldedModel);

            Directory.CreateDirectory(outputDir);

            var contextPath = Path.GetFullPath(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            var path = Path.GetDirectoryName(contextPath);
            if (path != null)
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(contextPath, scaffoldedModel.ContextFile.Code, Encoding.UTF8);
            }

            var additionalFiles = new List<string>();

            foreach (var entityTypeFile in scaffoldedModel.AdditionalFiles)
            {
                var additionalFilePath = Path.Combine(outputDir, entityTypeFile.Path);
                var addpath = Path.GetDirectoryName(additionalFilePath);
                if (addpath != null)
                {
                    Directory.CreateDirectory(addpath);
                    File.WriteAllText(additionalFilePath, entityTypeFile.Code, Encoding.UTF8);
                    additionalFiles.Add(additionalFilePath);
                }
            }

            return new SavedModelFiles(contextPath, additionalFiles);
        }

        public ScaffoldedModel ScaffoldModel(RoutineModel model, ModuleScaffolderOptions scaffolderOptions, List<string> schemas, ref List<string> errors)
        {
            ArgumentNullException.ThrowIfNull(model);

            ArgumentNullException.ThrowIfNull(errors);

            ArgumentNullException.ThrowIfNull(scaffolderOptions);

            var result = new ScaffoldedModel();

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

                    var typeName = ScaffoldHelper.GenerateIdentifierName(routine, model) + "Result" + suffix;

                    var classContent = WriteResultClass(resultSet, scaffolderOptions, typeName, routine.Schema);

                    if (!string.IsNullOrEmpty(routine.Schema))
                    {
                        schemas.Add($"{routine.Schema}Schema");
                    }

#if CORE90
                    result.AdditionalFiles.Add(new ScaffoldedFile(Path.Combine(routine.Schema, $"{typeName}.cs"), classContent));
#else
                    result.AdditionalFiles.Add(new ScaffoldedFile
                    {
                        Code = classContent,
                        Path = scaffolderOptions.UseSchemaFolders
                                ? Path.Combine(routine.Schema, $"{typeName}.cs")
                                : $"{typeName}.cs",
                    });
#endif
                }
            }

            var dbContextInterface = WriteDbContextInterface(scaffolderOptions, model, schemas.Distinct().ToList());

            if (!string.IsNullOrEmpty(dbContextInterface))
            {
#if CORE90
                result.AdditionalFiles.Add(new ScaffoldedFile($"I{scaffolderOptions.ContextName}{FileNameSuffix}.cs", dbContextInterface));
#else
                result.AdditionalFiles.Add(new ScaffoldedFile
                {
                    Code = dbContextInterface,
                    Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, $"I{scaffolderOptions.ContextName}{FileNameSuffix}.cs")),
                });
#endif
            }

            var dbContext = WriteDbContext(scaffolderOptions, model, schemas.Distinct().ToList());
#if CORE90
            result.ContextFile = new ScaffoldedFile(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + $"{FileNameSuffix}.cs"), dbContext);
#else
            result.ContextFile = new ScaffoldedFile
            {
                Code = dbContext,
                Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + $"{FileNameSuffix}.cs")),
            };
#endif

            return result;
        }

        protected abstract void GenerateProcedure(Routine procedure, RoutineModel model, bool signatureOnly, bool useAsyncCalls);

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
                Sb.AppendLine($"public partial interface I{scaffolderOptions.ContextName}{FileNameSuffix}");
                Sb.AppendLine("{");
                using (Sb.Indent())
                {
                    foreach (var procedure in model.Routines)
                    {
                        GenerateProcedure(procedure, model, true, scaffolderOptions.UseAsyncCalls);
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
                Sb.AppendLine($"public partial class {scaffolderOptions.ContextName}");
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
#if !CORE80
                    GenerateOnModelCreating(model);
#endif
                }

                Sb.AppendLine("}");
                Sb.AppendLine();

                Sb.AppendLine($"public partial class {scaffolderOptions.ContextName}{FileNameSuffix} : I{scaffolderOptions.ContextName}{FileNameSuffix}");
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
                    GenerateProcedure(procedure, model, false, scaffolderOptions.UseAsyncCalls);
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

#if !CORE80
        private void GenerateOnModelCreating(RoutineModel model)
        {
            Sb.AppendLine();
            Sb.AppendLine($"protected void OnModelCreatingGenerated{FileNameSuffix}(ModelBuilder modelBuilder)");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                foreach (var procedure in model.Routines.Cast<Procedure>().Where(p => string.IsNullOrEmpty(p.MappedType)))
                {
                    if (procedure.NoResultSet)
                    {
                        continue;
                    }

                    var i = 1;
                    foreach (var resultSet in procedure.Results)
                    {
                        var suffix = $"{i++}";

                        if (!procedure.SupportsMultipleResultSet)
                        {
                            suffix = string.Empty;
                        }

                        var typeName = ScaffoldHelper.GenerateIdentifierName(procedure, model) + "Result" + suffix;

                        Sb.AppendLine($"modelBuilder.Entity<{typeName}>().HasNoKey().ToView(null);");
                    }
                }
            }

            Sb.AppendLine("}");
        }
#endif

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

        private string WriteResultClass(List<ModuleResultElement> resultElements, ModuleScaffolderOptions options, string name, string schemaName)
        {
            var @namespace = options.ModelNamespace;

            Sb = new IndentedStringBuilder();

            Sb.AppendLine(PathHelper.Header);

            if (resultElements.Exists(p => typeMapper.GetClrType(p) == typeof(Geometry)))
            {
                Sb.AppendLine("using NetTopologySuite.Geometries;");
            }

            Sb.AppendLine("using System;");
            Sb.AppendLine("using System.Collections.Generic;");
            Sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            Sb.AppendLine();

            if (options.NullableReferences)
            {
                Sb.AppendLine("#nullable enable");
                Sb.AppendLine();
            }

            Sb.AppendLine($"namespace {@namespace}{(options.UseSchemaNamespaces ? $".{schemaName}Schema" : string.Empty)}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                GenerateClass(resultElements, name, options.NullableReferences, options.UseDecimalDataAnnotation);
            }

            Sb.AppendLine("}");

            return Sb.ToString();
        }

        private void GenerateClass(List<ModuleResultElement> resultElements, string name, bool nullableReferences, bool useDecimalDataAnnotation)
        {
            Sb.AppendLine($"public partial class {name}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                GenerateProperties(resultElements, nullableReferences, useDecimalDataAnnotation);
            }

            Sb.AppendLine("}");
        }

        private void GenerateProperties(List<ModuleResultElement> resultElements, bool nullableReferences, bool useDecimalDataAnnotation)
        {
            foreach (var property in resultElements.OrderBy(e => e.Ordinal))
            {
                var propertyNames = ScaffoldHelper.GeneratePropertyName(property.Name);

                if (property.StoreType == "decimal" && useDecimalDataAnnotation)
                {
                    Sb.AppendLine($"[Column(\"{property.Name}\", TypeName = \"{property.StoreType}({property.Precision},{property.Scale})\")]");
                }
                else
                {
                    if (!string.IsNullOrEmpty(propertyNames.Item2))
                    {
                        Sb.AppendLine(propertyNames.Item2);
                    }
                }

                var propertyType = typeMapper.GetClrType(property);
                var nullableAnnotation = string.Empty;
                var defaultAnnotation = string.Empty;

                if (nullableReferences && !propertyType.IsValueType)
                {
                    if (property.Nullable)
                    {
                        nullableAnnotation = "?";
                    }
                    else
                    {
                        defaultAnnotation = $" = default!;";
                    }
                }

                Sb.AppendLine($"public {Code.Reference(propertyType)}{nullableAnnotation} {propertyNames.Item1} {{ get; set; }}{defaultAnnotation}");
            }
        }
    }
}
