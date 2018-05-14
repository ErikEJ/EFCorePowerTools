// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Modifications copyright(C) 2017 Tony Sneed.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EntityFrameworkCore.Scaffolding.Handlebars.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Generator for entity type classes using Handlebars templates.
    /// </summary>
    public class HbsCSharpEntityTypeGenerator : ICSharpEntityTypeGenerator
    {
        private bool _useDataAnnotations;
        private Dictionary<string, object> _templateData;
        private List<Dictionary<string, object>> _propertyAnnotations;
        private List<Dictionary<string, object>> _navPropertyAnnotations;

        private ICSharpUtilities CSharpUtilities { get; }

        /// <summary>
        /// Template service for the entity types generator.
        /// </summary>
        public virtual IEntityTypeTemplateService EntityTypeTemplateService { get; }

        /// <summary>
        /// Constructor for the Handlebars entity types generator.
        /// </summary>
        /// <param name="cSharpUtilities">CSharp utilities.</param>
        /// <param name="entityTypeTemplateService">Template service for the entity types generator.</param>
        public HbsCSharpEntityTypeGenerator(
            ICSharpUtilities cSharpUtilities,
            IEntityTypeTemplateService entityTypeTemplateService)
        {
            CSharpUtilities = cSharpUtilities ?? throw new ArgumentNullException(nameof(cSharpUtilities));
            EntityTypeTemplateService = entityTypeTemplateService ?? throw new ArgumentNullException(nameof(entityTypeTemplateService));
        }

        /// <summary>
        /// Generate entity type class.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        /// <param name="namespace">Entity type namespace.</param>
        /// <param name="useDataAnnotations">If true use data annotations.</param>
        /// <returns>Generated entity type.</returns>
        public virtual string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            if (@namespace == null) throw new ArgumentNullException(nameof(@namespace));

            _useDataAnnotations = useDataAnnotations;
            _templateData = new Dictionary<string, object>();
            _templateData.Add("use-data-annotations", _useDataAnnotations);

            GenerateImports(entityType);

            _templateData.Add("namespace", @namespace);

            GenerateClass(entityType);

            string output = EntityTypeTemplateService.GenerateEntityType(_templateData);
            return output;
        }

        /// <summary>
        /// Generate entity type imports.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        protected virtual void GenerateImports(IEntityType entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            var imports = new List<Dictionary<string, object>>();

            foreach (var ns in entityType.GetProperties()
                .SelectMany(p => p.ClrType.GetNamespaces())
                .Where(ns => ns != "System" && ns != "System.Collections.Generic")
                .Distinct())
            {
                imports.Add(new Dictionary<string, object> { { "import", ns } });
            }

            _templateData.Add("imports", imports);
        }

        /// <summary>
        /// Generate entity type class.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        protected virtual void GenerateClass(IEntityType entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            if (_useDataAnnotations)
            {
                GenerateEntityTypeDataAnnotations(entityType);
            }

            _templateData.Add("class", entityType.Name);

            GenerateConstructor(entityType);
            GenerateProperties(entityType);
            GenerateNavigationProperties(entityType);
        }

        /// <summary>
        /// Generate entity type constructor.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        protected virtual void GenerateConstructor(IEntityType entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            var collectionNavigations = entityType.GetNavigations().Where(n => n.IsCollection()).ToList();

            if (collectionNavigations.Count > 0)
            {
                var lines = new List<Dictionary<string, object>>();

                foreach (var navigation in collectionNavigations)
                {
                    lines.Add(new Dictionary<string, object>
                    {
                        { "property-name", navigation.Name },
                        { "property-type", navigation.GetTargetType().Name },
                    });
                }

                _templateData.Add("lines", lines);
            }
        }

        /// <summary>
        /// Generate entity type properties.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        protected virtual void GenerateProperties(IEntityType entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            var properties = new List<Dictionary<string, object>>();

            foreach (var property in entityType.GetProperties().OrderBy(p => p.Scaffolding().ColumnOrdinal))
            {
                _propertyAnnotations = new List<Dictionary<string, object>>();

                if (_useDataAnnotations)
                {
                    GeneratePropertyDataAnnotations(property);
                }

                properties.Add(new Dictionary<string, object>
                {
                    { "property-type", CSharpUtilities.GetTypeName(property.ClrType) },
                    { "property-name", property.Name },
                    { "property-annotations",  _propertyAnnotations}
                });
            }

            _templateData.Add("properties", properties);
        }

        /// <summary>
        /// Generate entity type navigation properties.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        protected virtual void GenerateNavigationProperties(IEntityType entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            var sortedNavigations = entityType.GetNavigations()
                .OrderBy(n => n.IsDependentToPrincipal() ? 0 : 1)
                .ThenBy(n => n.IsCollection() ? 1 : 0);

            if (sortedNavigations.Any())
            {
                var navProperties = new List<Dictionary<string, object>>();

                foreach (var navigation in sortedNavigations)
                {
                    _navPropertyAnnotations = new List<Dictionary<string, object>>();

                    if (_useDataAnnotations)
                    {
                        GenerateNavigationDataAnnotations(navigation);
                    }

                    navProperties.Add(new Dictionary<string, object>
                    {
                        { "nav-property-collection", navigation.IsCollection() },
                        { "nav-property-type", navigation.GetTargetType().Name },
                        { "nav-property-name", navigation.Name },
                        { "nav-property-annotations", _navPropertyAnnotations },
                    });
                }

                _templateData.Add("nav-properties", navProperties);
            }
        }

        /// <summary>
        /// Generate entity type data annotations.
        /// </summary>
        /// <param name="entityType">Represents an entity type in an <see cref="T:Microsoft.EntityFrameworkCore.Metadata.IModel" />.</param>
        protected virtual void GenerateEntityTypeDataAnnotations(IEntityType entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            GenerateTableAttribute(entityType);
        }

        /// <summary>
        /// Generate property data annotations.
        /// </summary>
        /// <param name="property">Represents a scalar property of an entity.</param>
        protected virtual void GeneratePropertyDataAnnotations(IProperty property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            GenerateKeyAttribute(property);
            GenerateRequiredAttribute(property);
            GenerateColumnAttribute(property);
            GenerateMaxLengthAttribute(property);
        }

        private void GenerateTableAttribute(IEntityType entityType)
        {
            var tableName = entityType.Relational().TableName;
            var schema = entityType.Relational().Schema;
            var defaultSchema = entityType.Model.Relational().DefaultSchema;

            var schemaParameterNeeded = schema != null && schema != defaultSchema;
            var tableAttributeNeeded = schemaParameterNeeded || tableName != null && tableName != entityType.Scaffolding().DbSetName;

            if (tableAttributeNeeded)
            {
                var tableAttribute = new AttributeWriter(nameof(TableAttribute));

                tableAttribute.AddParameter(CSharpUtilities.DelimitString(tableName));

                if (schemaParameterNeeded)
                {
                    tableAttribute.AddParameter($"{nameof(TableAttribute.Schema)} = {CSharpUtilities.DelimitString(schema)}");
                }

                _templateData.Add("class-annotation", tableAttribute.ToString());
            }
        }

        private void GenerateKeyAttribute(IProperty property)
        {
            var key = property.AsProperty().PrimaryKey;

            if (key?.Properties.Count == 1)
            {
                if (key is Key concreteKey
                    && key.Properties.SequenceEqual(new KeyDiscoveryConvention().DiscoverKeyProperties(concreteKey.DeclaringEntityType, concreteKey.DeclaringEntityType.GetProperties().ToList())))
                {
                    return;
                }

                if (key.Relational().Name != ConstraintNamer.GetDefaultName(key))
                {
                    return;
                }

                _propertyAnnotations.Add(new Dictionary<string, object>
                {
                    { "property-annotation", new AttributeWriter(nameof(KeyAttribute)) },
                });
            }
        }

        private void GenerateColumnAttribute(IProperty property)
        {
            var columnName = property.Relational().ColumnName;
            var columnType = property.GetConfiguredColumnType();

            var delimitedColumnName = columnName != null && columnName != property.Name ? CSharpUtilities.DelimitString(columnName) : null;
            var delimitedColumnType = columnType != null ? CSharpUtilities.DelimitString(columnType) : null;

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

                _propertyAnnotations.Add(new Dictionary<string, object>
                {
                    { "property-annotation", columnAttribute },
                });
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

                lengthAttribute.AddParameter(CSharpUtilities.GenerateLiteral(maxLength.Value));

                _propertyAnnotations.Add(new Dictionary<string, object>
                {
                    { "property-annotation", lengthAttribute.ToString() },
                });
            }
        }

        private void GenerateRequiredAttribute(IProperty property)
        {
            if (!property.IsNullable
                && property.ClrType.IsNullableType()
                && !property.IsPrimaryKey())
            {
                _propertyAnnotations.Add(new Dictionary<string, object>
                {
                    { "property-annotation", new AttributeWriter(nameof(RequiredAttribute)).ToString() },
                });
            }
        }

        private void GenerateNavigationDataAnnotations(INavigation navigation)
        {
            if (navigation == null) throw new ArgumentNullException(nameof(navigation));

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

                    foreignKeyAttribute.AddParameter(
                        CSharpUtilities.DelimitString(
                            string.Join(",", navigation.ForeignKey.Properties.Select(p => p.Name))));

                    _navPropertyAnnotations.Add(new Dictionary<string, object>
                    {
                        { "nav-property-annotation", foreignKeyAttribute.ToString() },
                    });
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

                    inversePropertyAttribute.AddParameter(CSharpUtilities.DelimitString(inverseNavigation.Name));

                    _navPropertyAnnotations.Add(new Dictionary<string, object>
                    {
                        { "nav-property-annotation", inversePropertyAttribute.ToString() },
                    });
                }
            }
        }

        private class AttributeWriter
        {
            private readonly string _attibuteName;
            private readonly List<string> _parameters = new List<string>();

            public AttributeWriter(string attributeName)
            {
                _attibuteName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
            }

            public void AddParameter(string parameter)
            {
                if (parameter == null) throw new ArgumentNullException(nameof(parameter));

                _parameters.Add(parameter);
            }

            public override string ToString()
                => "[" + (_parameters.Count == 0
                       ? StripAttribute(_attibuteName)
                       : StripAttribute(_attibuteName) + "(" + string.Join(", ", _parameters) + ")") + "]";

            private static string StripAttribute(string attributeName)
            {
                if (attributeName == null) throw new ArgumentNullException(nameof(attributeName));
                return attributeName.EndsWith("Attribute", StringComparison.Ordinal)
                    ? attributeName.Substring(0, attributeName.Length - 9)
                    : attributeName;
            }
        }
    }
}
