﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal
{
    public class CommentCSharpEntityTypeGenerator : CSharpEntityTypeGenerator
    {
        private readonly ICSharpHelper _code;

        private IndentedStringBuilder _sb;
        private bool _useDataAnnotations;

        public CommentCSharpEntityTypeGenerator(
            [NotNull] ICSharpHelper cSharpHelper) : base(cSharpHelper)
        {
            _code = cSharpHelper;
        }

        public override string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            _sb = new IndentedStringBuilder();
            _useDataAnnotations = useDataAnnotations;

            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Collections.Generic;");

            if (_useDataAnnotations)
            {
                _sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                _sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            }

            foreach (var ns in entityType.GetProperties()
                .SelectMany(p => p.ClrType.GetNamespaces())
                .Where(ns => ns != "System" && ns != "System.Collections.Generic")
                .Distinct()
                .OrderBy(x => x, new NamespaceComparer()))
            {
                _sb.AppendLine($"using {ns};");
            }

            _sb.AppendLine();
            _sb.AppendLine($"namespace {@namespace}");
            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateClass(entityType);
            }

            _sb.AppendLine("}");

            return _sb.ToString();
        }

        protected override void GenerateClass(
            [NotNull] IEntityType entityType)
        {
            if (_useDataAnnotations)
            {
                GenerateEntityTypeDataAnnotations(entityType);
            }

            WriteComment(entityType.GetComment());

            _sb.AppendLine($"public partial class {entityType.Name}");

            _sb.AppendLine("{");

            using (_sb.Indent())
            {
                GenerateConstructor(entityType);
                GenerateProperties(entityType);
                GenerateNavigationProperties(entityType);
            }

            _sb.AppendLine("}");
        }

        protected override void GenerateEntityTypeDataAnnotations(
            [NotNull] IEntityType entityType)
        {
            GenerateTableAttribute(entityType);
        }

        private void GenerateTableAttribute(IEntityType entityType)
        {
            var tableName = entityType.GetTableName();
            var schema = entityType.GetSchema();
            var defaultSchema = entityType.Model.GetDefaultSchema();

            var schemaParameterNeeded = schema != null && schema != defaultSchema;
            var isView = entityType.FindAnnotation(RelationalAnnotationNames.ViewDefinition) != null;
            var tableAttributeNeeded = !isView && (schemaParameterNeeded || tableName != null && tableName != entityType.GetDbSetName());

            if (tableAttributeNeeded)
            {
                var tableAttribute = new AttributeWriter(nameof(TableAttribute));

                tableAttribute.AddParameter(_code.Literal(tableName));

                if (schemaParameterNeeded)
                {
                    tableAttribute.AddParameter($"{nameof(TableAttribute.Schema)} = {_code.Literal(schema)}");
                }

                _sb.AppendLine(tableAttribute.ToString());
            }
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        protected override void GenerateConstructor(
            [NotNull] IEntityType entityType)
        {
            var collectionNavigations = entityType.GetNavigations().Where(n => n.IsCollection()).ToList();

            if (collectionNavigations.Count > 0)
            {
                _sb.AppendLine($"public {entityType.Name}()");
                _sb.AppendLine("{");

                using (_sb.Indent())
                {
                    foreach (var navigation in collectionNavigations)
                    {
                        _sb.AppendLine($"{navigation.Name} = new HashSet<{navigation.GetTargetType().Name}>();");
                    }
                }

                _sb.AppendLine("}");
                _sb.AppendLine();
            }
        }

        protected override void GenerateProperties(
            [NotNull] IEntityType entityType)
        {
            foreach (var property in entityType.GetProperties().OrderBy(p => p.GetColumnOrdinal()))
            {
                if (_useDataAnnotations)
                {
                    GeneratePropertyDataAnnotations(property);
                }

                WriteComment(property.GetComment());

                _sb.AppendLine($"public {_code.Reference(property.ClrType)} {property.Name} {{ get; set; }}");
            }
        }

        protected override void GeneratePropertyDataAnnotations(
            [NotNull] IProperty property)
        {
            GenerateKeyAttribute(property);
            GenerateRequiredAttribute(property);
            GenerateColumnAttribute(property);
            GenerateMaxLengthAttribute(property);
        }

        private void WriteComment(string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment))
            {
                _sb.AppendLine("/// <summary>");
                _sb.AppendLine($"/// {comment}");
                _sb.AppendLine("/// </summary>");
            }
        }

        private void GenerateKeyAttribute(IProperty property)
        {
            var key = property.FindContainingPrimaryKey();
            if (key != null)
            {
                _sb.AppendLine(new AttributeWriter(nameof(KeyAttribute)));
            }
        }

        private void GenerateColumnAttribute(IProperty property)
        {
            var columnName = property.GetColumnName();
            var columnType = property.GetConfiguredColumnType();

            var delimitedColumnName = columnName != null && columnName != property.Name ? _code.Literal(columnName) : null;
            var delimitedColumnType = columnType != null ? _code.Literal(columnType) : null;

            if ((delimitedColumnName ?? delimitedColumnType) != null)
            {
                var columnAttribute = new AttributeWriter(nameof(ColumnAttribute));

                if (delimitedColumnName != null)
                {
                    columnAttribute.AddParameter(delimitedColumnName);
                }

                if (delimitedColumnType != null)
                {
                    columnAttribute.AddParameter($"{nameof(ColumnAttribute.TypeName)} = {delimitedColumnType}");
                }

                _sb.AppendLine(columnAttribute);
            }
        }

        private void GenerateMaxLengthAttribute(IProperty property)
        {
            var maxLength = property.GetMaxLength();

            if (maxLength.HasValue)
            {
                var lengthAttribute = new AttributeWriter(
                    property.ClrType == typeof(string)
                        ? nameof(StringLengthAttribute)
                        : nameof(MaxLengthAttribute));

                lengthAttribute.AddParameter(_code.Literal(maxLength.Value));

                _sb.AppendLine(lengthAttribute.ToString());
            }
        }

        private void GenerateRequiredAttribute(IProperty property)
        {
            if (!property.IsNullable
                && property.ClrType.IsNullableType()
                && !property.IsPrimaryKey())
            {
                _sb.AppendLine(new AttributeWriter(nameof(RequiredAttribute)).ToString());
            }
        }

        protected override void GenerateNavigationProperties(
            [NotNull] IEntityType entityType)
        {
            var sortedNavigations = entityType.GetNavigations()
                .OrderBy(n => n.IsDependentToPrincipal() ? 0 : 1)
                .ThenBy(n => n.IsCollection() ? 1 : 0)
                .ToList();

            if (sortedNavigations.Any())
            {
                _sb.AppendLine();

                foreach (var navigation in sortedNavigations)
                {
                    if (_useDataAnnotations)
                    {
                        GenerateNavigationDataAnnotations(navigation);
                    }

                    var referencedTypeName = navigation.GetTargetType().Name;
                    var navigationType = navigation.IsCollection() ? $"ICollection<{referencedTypeName}>" : referencedTypeName;
                    _sb.AppendLine($"public virtual {navigationType} {navigation.Name} {{ get; set; }}");
                }
            }
        }

        private void GenerateNavigationDataAnnotations(INavigation navigation)
        {
            GenerateForeignKeyAttribute(navigation);
            GenerateInversePropertyAttribute(navigation);
        }

        private void GenerateForeignKeyAttribute(INavigation navigation)
        {
            if (navigation.IsDependentToPrincipal())
            {
                if (navigation.ForeignKey.PrincipalKey.IsPrimaryKey())
                {
                    var foreignKeyAttribute = new AttributeWriter(nameof(ForeignKeyAttribute));

                    if (navigation.ForeignKey.Properties.Count > 1)
                    {
                        foreignKeyAttribute.AddParameter(
                            _code.Literal(
                                string.Join(",", navigation.ForeignKey.Properties.Select(p => p.Name))));
                    }
                    else
                    {
                        foreignKeyAttribute.AddParameter($"nameof({navigation.ForeignKey.Properties.First().Name})");
                    }

                    _sb.AppendLine(foreignKeyAttribute.ToString());
                }
            }
        }

        private void GenerateInversePropertyAttribute(INavigation navigation)
        {
            if (navigation.ForeignKey.PrincipalKey.IsPrimaryKey())
            {
                var inverseNavigation = navigation.FindInverse();

                if (inverseNavigation != null)
                {
                    var inversePropertyAttribute = new AttributeWriter(nameof(InversePropertyAttribute));

                    inversePropertyAttribute.AddParameter(
                        !navigation.DeclaringEntityType.GetPropertiesAndNavigations().Any(
                                m => m.Name == inverseNavigation.DeclaringEntityType.Name)
                            ? $"nameof({inverseNavigation.DeclaringEntityType.Name}.{inverseNavigation.Name})"
                            : _code.Literal(inverseNavigation.Name));

                    _sb.AppendLine(inversePropertyAttribute.ToString());
                }
            }
        }

        private class AttributeWriter
        {
            private readonly string _attributeName;
            private readonly List<string> _parameters = new List<string>();

            public AttributeWriter([NotNull] string attributeName)
            {
                _attributeName = attributeName;
            }

            public void AddParameter([NotNull] string parameter)
            {
                _parameters.Add(parameter);
            }

            public override string ToString()
                => "[" + (_parameters.Count == 0
                       ? StripAttribute(_attributeName)
                       : StripAttribute(_attributeName) + "(" + string.Join(", ", _parameters) + ")") + "]";

            private static string StripAttribute([NotNull] string attributeName)
                => attributeName.EndsWith("Attribute", StringComparison.Ordinal)
                    ? attributeName.Substring(0, attributeName.Length - 9)
                    : attributeName;
        }
    }
}