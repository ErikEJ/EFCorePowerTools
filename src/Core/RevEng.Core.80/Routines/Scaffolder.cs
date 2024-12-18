using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetTopologySuite.Geometries;
using RevEng.Common;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;

namespace RevEng.Core.Routines
{
    public class Scaffolder
    {
#pragma warning disable SA1401 // Fields should be private
        internal readonly ICSharpHelper Code;
        internal IndentedStringBuilder Sb;
#pragma warning restore SA1401 // Fields should be private
        private readonly IClrTypeMapper typeMapper;

        public Scaffolder([System.Diagnostics.CodeAnalysis.NotNull] ICSharpHelper code, IClrTypeMapper typeMapper)
        {
            ArgumentNullException.ThrowIfNull(code);

            Code = code;
            this.typeMapper = typeMapper;
        }

        public string WriteResultClass(List<ModuleResultElement> resultElements, ModuleScaffolderOptions options, string name, string schemaName)
        {
            ArgumentNullException.ThrowIfNull(resultElements);
            ArgumentNullException.ThrowIfNull(options);

            var @namespace = options.ModelNamespace;

            Sb = new IndentedStringBuilder();

            Sb.AppendLine(PathHelper.Header);

            if (resultElements.Exists(p => typeMapper.GetClrType(p) == typeof(Geometry)))
            {
                Sb.AppendLine("using NetTopologySuite.Geometries;");
            }

            Sb.AppendLine("using System;");
            Sb.AppendLine("using System.Collections.Generic;");

            if (options.UseDecimalDataAnnotation)
            {
                Sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            }

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
                GenerateClass(resultElements, name, options.NullableReferences, options.UseDecimalDataAnnotation, options.UsePascalIdentifiers);
            }

            Sb.AppendLine("}");

            return Sb.ToString();
        }

        private void GenerateClass(List<ModuleResultElement> resultElements, string name, bool nullableReferences, bool useDecimalDataAnnotation, bool usePascalCase)
        {
            Sb.AppendLine($"public partial class {name}");
            Sb.AppendLine("{");

            using (Sb.Indent())
            {
                GenerateProperties(resultElements, nullableReferences, useDecimalDataAnnotation, usePascalCase);
            }

            Sb.AppendLine("}");
        }

        private void GenerateProperties(List<ModuleResultElement> resultElements, bool nullableReferences, bool useDecimalDataAnnotation, bool usePascalCase)
        {
            var propertyNames = new List<string>();

            int i = 0;

            foreach (var property in resultElements.OrderBy(e => e.Ordinal))
            {
                var propertyNameAndAttribute = ScaffoldHelper.GeneratePropertyName(property.Name, Code, usePascalCase);

                if (property.StoreType == "decimal" && useDecimalDataAnnotation)
                {
                    Sb.AppendLine($"[Column(\"{property.Name}\", TypeName = \"{property.StoreType}({property.Precision},{property.Scale})\")]");
                }
                else if (property.StoreType == "money" && useDecimalDataAnnotation)
                {
                    Sb.AppendLine($"[Column(\"{property.Name}\", TypeName = \"{property.StoreType}\")]");
                }
                else
                {
                    if (!string.IsNullOrEmpty(propertyNameAndAttribute.Item2))
                    {
                        Sb.AppendLine(propertyNameAndAttribute.Item2);
                    }
                }

                if (useDecimalDataAnnotation
                    && ((property.StoreType.StartsWith("varchar", StringComparison.OrdinalIgnoreCase)
                        || property.StoreType.StartsWith("nvarchar", StringComparison.OrdinalIgnoreCase))
                    && property.MaxLength > 0))
                {
                    Sb.AppendLine($"[StringLength({property.MaxLength})]");
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

                var propertyName = propertyNameAndAttribute.Item1;

                if (propertyNames.Contains(propertyName, StringComparer.Ordinal))
                {
                    propertyName = $"{propertyName}_Duplicate{i++}";
                }

                propertyNames.Add(propertyNameAndAttribute.Item1);

                Sb.AppendLine($"public {Code.Reference(propertyType)}{nullableAnnotation} {propertyName} {{ get; set; }}{defaultAnnotation}");
            }
        }
    }
}
