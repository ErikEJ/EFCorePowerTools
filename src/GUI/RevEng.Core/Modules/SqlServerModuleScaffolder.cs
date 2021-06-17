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

        //public ScaffoldedModel ScaffoldModel(FunctionModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors)
        //{
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    var result = new ScaffoldedModel();

        //    errors = model.Errors;

        //    foreach (var function in model.Routines.Where(f => !f.IsScalar))
        //    {
        //        var typeName = GenerateIdentifierName(function, model) + "Result";

        //        var classContent = WriteResultClass(function, scaffolderOptions, typeName);

        //        result.AdditionalFiles.Add(new ScaffoldedFile
        //        {
        //            Code = classContent,
        //            Path = scaffolderOptions.UseSchemaFolders
        //                    ? Path.Combine(function.Schema, $"{typeName}.cs")
        //                    : $"{typeName}.cs"
        //        });
        //    }

        //    var dbContext = WriteFunctionsClass(scaffolderOptions, model);

        //    result.ContextFile = new ScaffoldedFile
        //    {
        //        Code = dbContext,
        //        Path = Path.GetFullPath(Path.Combine(scaffolderOptions.ContextDir, scaffolderOptions.ContextName + ".Functions.cs")),
        //    };

        //    return result;
        //}

        protected string GenerateUniqueName(ModuleBase module, ModuleModel model)
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
