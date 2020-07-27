using EFCorePowerTools.Shared.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Procedures.Model;
using RevEng.Core.Procedures.Model.Metadata;
using RevEng.Core.Procedures.Scaffolding;
using ReverseEngineer20;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RevEng.Core.Procedures
{
    public class SqlServerProcedureScaffolder : IProcedureScaffolder
    {
        private readonly IProcedureModelFactory procedureModelFactory;
        private readonly ICSharpHelper code;

        private IndentedStringBuilder _sb;

        public SqlServerProcedureScaffolder(IProcedureModelFactory procedureModelFactory, [NotNull] ICSharpHelper code)
        {
            this.procedureModelFactory = procedureModelFactory;
            this.code = code;
        }

        public ScaffoldedModel ScaffoldModel(string connectionString, ProcedureScaffolderOptions procedureScaffolderOptions, ProcedureModelFactoryOptions procedureModelFactoryOptions)
        {
            var result = new ScaffoldedModel();

            var model = procedureModelFactory.Create(connectionString, procedureModelFactoryOptions);

            foreach (var procedure in model.Procedures)
            {
                var name = GenerateIdentifierName(procedure.Name) + "Result";

                var classContent = WriteResultClass(procedure, procedureScaffolderOptions.ModelNamespace, name);

                result.AdditionalFiles.Add(new ScaffoldedFile
                {
                    Code = classContent,
                    Path = $"{name}.cs",
                });
            }

            //TODO Generate DbContext class
            foreach (var procedure in model.Procedures)
            {

            }

            return result;
        }

        private string WriteResultClass(StoredProcedure storedProcedure, string @namespace, string name)
        {
            _sb = new IndentedStringBuilder();

            _sb.AppendLine(PathHelper.Header);

            _sb.AppendLine();
            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Collections.Generic;");

            _sb.AppendLine();
            _sb.AppendLine($"namespace {@namespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateClass(storedProcedure, name);
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        private void GenerateClass(StoredProcedure storedProcedure, string name)
        {
            _sb.AppendLine($"public partial class {name}");

            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateProperties(storedProcedure);
            }

            _sb.AppendLine("}");
        }

        private void GenerateProperties(StoredProcedure storedProcedure)
        {
            foreach (var property in storedProcedure.ResultElements.OrderBy(e => e.Ordinal))
            {
                _sb.AppendLine($"public {code.Reference(property.ClrType())} {property.Name} {{ get; set; }}");
            }
        }

        private string GenerateIdentifierName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            var isValid = System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);

            if (!isValid)
            {
                // File name contains invalid chars, remove them
                var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.None, TimeSpan.FromSeconds(5));
                name = regex.Replace(name, "");

                // Class name doesn't begin with a letter, insert an underscore
                if (!char.IsLetter(name, 0))
                {
                    name = name.Insert(0, "_");
                }
            }

            return name.Replace(" ", string.Empty);
        }
    }
}
