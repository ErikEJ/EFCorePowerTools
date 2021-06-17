using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using RevEng.Core.Abstractions;
using RevEng.Core.Procedures;

#if CORE50 || CORE60
using Microsoft.EntityFrameworkCore.Infrastructure;
#else
using Microsoft.EntityFrameworkCore.Internal;
#endif

namespace RevEng.Core.Modules
{
    public class SqlServerModuleScaffolder : IModuleScaffolder
    {
        protected readonly ICSharpHelper code;
        protected IndentedStringBuilder _sb;

        public SqlServerModuleScaffolder([NotNull] ICSharpHelper code)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            this.code = code;
        }

        public SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace)
        {
            Directory.CreateDirectory(outputDir);

            var contextPath = Path.GetFullPath(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
            Directory.CreateDirectory(Path.GetDirectoryName(contextPath));
            File.WriteAllText(contextPath, scaffoldedModel.ContextFile.Code, Encoding.UTF8);

            var additionalFiles = new List<string>();

            foreach (var entityTypeFile in scaffoldedModel.AdditionalFiles)
            {
                var additionalFilePath = Path.Combine(outputDir, entityTypeFile.Path);
                Directory.CreateDirectory(Path.GetDirectoryName(additionalFilePath));
                File.WriteAllText(additionalFilePath, entityTypeFile.Code, Encoding.UTF8);
                additionalFiles.Add(additionalFilePath);
            }

            return new SavedModelFiles(contextPath, additionalFiles);
        }

        public ScaffoldedModel ScaffoldModel(ModuleModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var result = new ScaffoldedModel();

            errors = model.Errors;

            foreach (var function in model.Routines.Where(f => !(f is Function func) || !func.IsScalar))
            {
                var typeName = GenerateIdentifierName(function, model) + "Result";

                var classContent = WriteResultClass(function, scaffolderOptions, typeName);

                result.AdditionalFiles.Add(new ScaffoldedFile
                {
                    Code = classContent,
                    Path = scaffolderOptions.UseSchemaFolders
                            ? Path.Combine(function.Schema, $"{typeName}.cs")
                            : $"{typeName}.cs"
                });
            }

            var dbContext = WriteDbContext(scaffolderOptions, model);

            var fileNameSuffix = (this is SqlServerStoredProcedureScaffolder)
                ? "Procedures"
                : ".Functions";
            result.ContextFile = new ScaffoldedFile
            {
                Code = dbContext,
                Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + $"{fileNameSuffix}.cs")),
            };

            return result;
        }

        protected virtual string WriteDbContext(ModuleScaffolderOptions scaffolderOptions, ModuleModel model)
        {
            throw new NotImplementedException();
        }

        protected virtual string WriteResultClass(Module module, ModuleScaffolderOptions scaffolderOptions, string typeName)
        {
            throw new NotImplementedException();
        }

        protected virtual string GenerateIdentifierName(Module module, ModuleModel model)
        {
            throw new NotImplementedException();
        }

        protected string GenerateUniqueName(Module module, ModuleModel model)
        {
            if (!string.IsNullOrEmpty(module.NewName))
                return module.NewName;

            var numberOfNames = model.Routines.Where(p => p.Name == module.Name).Count();

            if (numberOfNames > 1)
                return module.Name + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(module.Schema);

            return module.Name;
        }
    }
}
