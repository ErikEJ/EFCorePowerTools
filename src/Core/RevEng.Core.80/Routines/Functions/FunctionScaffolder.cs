using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines.Functions
{
    public abstract class FunctionScaffolder : IRoutineScaffolder
    {
#pragma warning disable SA1401 // Fields should be private
        internal readonly ICSharpHelper Code;
        internal IndentedStringBuilder Sb;
#pragma warning restore SA1401 // Fields should be private
        private readonly Scaffolder scaffolder;

        protected FunctionScaffolder([System.Diagnostics.CodeAnalysis.NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
        {
            ArgumentNullException.ThrowIfNull(code);

            Code = code;
            scaffolder = new Scaffolder(code, typeMapper);
        }

        public string FileNameSuffix { get; set; }

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

        protected abstract string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, RoutineModel model, List<string> schemas);
    }
}