﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Procedures;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

            foreach (var routine in model.Routines.Where(r => !(r is Function f) || !f.IsScalar))
            {
                var typeName = GenerateIdentifierName(routine, model) + "Result";

                var classContent = WriteResultClass(routine, scaffolderOptions, typeName);

                result.AdditionalFiles.Add(new ScaffoldedFile
                {
                    Code = classContent,
                    Path = scaffolderOptions.UseSchemaFolders
                            ? Path.Combine(routine.Schema, $"{typeName}.cs")
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

        private string WriteResultClass(Module module, ModuleScaffolderOptions options, string name)
        {
            var @namespace = options.ModelNamespace;

            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);
            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Collections.Generic;");
            _sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            _sb.AppendLine();

            if (options.NullableReferences)
            {
                _sb.AppendLine("#nullable enable");
                _sb.AppendLine();
            }

            _sb.AppendLine($"namespace {@namespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateClass(module, name, options.NullableReferences);
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private void GenerateClass(Module module, string name, bool nullableReferences)
        {
            _sb.AppendLine($"public partial class {name}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateProperties(module, nullableReferences);
            }

            _sb.AppendLine("}");
        }

        private void GenerateProperties(Module module, bool nullableReferences)
        {
            foreach (var property in module.ResultElements.OrderBy(e => e.Ordinal))
            {
                var propertyNames = GeneratePropertyName(property.Name);

                if (!string.IsNullOrEmpty(propertyNames.Item2))
                {
                    _sb.AppendLine(propertyNames.Item2);
                }

                var propertyType = property.ClrType();
                string nullableAnnotation = string.Empty;
                string defaultAnnotation = string.Empty;

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

                _sb.AppendLine($"public {code.Reference(propertyType)}{nullableAnnotation} {propertyNames.Item1} {{ get; set; }}{defaultAnnotation}");
            }
        }

        private Tuple<string, string> GeneratePropertyName(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return CreateIdentifier(propertyName);
        }

        protected string GenerateIdentifierName(Module module, ModuleModel model)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            return CreateIdentifier(GenerateUniqueName(module, model)).Item1;
        }

        private Tuple<string, string> CreateIdentifier(string name)
        {
            var isValid = System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);

            string columAttribute = null;

            if (!isValid)
            {
                columAttribute = $"[Column(\"{name}\")]";
                // File name contains invalid chars, remove them
                var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                name = regex.Replace(name, "");

                // Class name doesn't begin with a letter, insert an underscore
                if (!char.IsLetter(name, 0))
                {
                    name = name.Insert(0, "_");
                }
            }

            return new Tuple<string, string>(name.Replace(" ", string.Empty), columAttribute);
        }

        private string GenerateUniqueName(Module module, ModuleModel model)
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
