using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ScaffoldingAnnotationNames = Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal.ScaffoldingAnnotationNames;

namespace ReverseEngineer20.ReplacementServices
{
    public class ReplacementScaffoldingModelFactory : IScaffoldingModelFactory
    {
        internal const string NavigationNameUniquifyingPattern = "{0}Navigation";
        internal const string SelfReferencingPrincipalEndNavigationNamePattern = "Inverse{0}";

        private readonly IOperationReporter _reporter;
        private readonly ICandidateNamingService _candidateNamingService;
        private Dictionary<DatabaseTable, CSharpUniqueNamer<DatabaseColumn>> _columnNamers;
        private bool _useDatabaseNames;
        private readonly DatabaseTable _nullTable = new DatabaseTable();
        private CSharpUniqueNamer<DatabaseTable> _tableNamer;
        private CSharpUniqueNamer<DatabaseTable> _dbSetNamer;
        private readonly HashSet<DatabaseColumn> _unmappedColumns = new HashSet<DatabaseColumn>();
        private readonly IPluralizer _pluralizer;
        private readonly ICSharpUtilities _cSharpUtilities;
        private readonly IScaffoldingTypeMapper _scaffoldingTypeMapper;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public ReplacementScaffoldingModelFactory(
            IOperationReporter reporter,
            ICandidateNamingService candidateNamingService,
            IPluralizer pluralizer,
            ICSharpUtilities cSharpUtilities,
            IScaffoldingTypeMapper scaffoldingTypeMapper)
        {

            _reporter = reporter;
            _candidateNamingService = candidateNamingService;
            _pluralizer = pluralizer;
            _cSharpUtilities = cSharpUtilities;
            _scaffoldingTypeMapper = scaffoldingTypeMapper;
        }


        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual IModel Create(DatabaseModel databaseModel, bool useDatabaseNames)
        {

            var modelBuilder = new ModelBuilder(new ConventionSet());

            _tableNamer = new CSharpUniqueNamer<DatabaseTable>(
                useDatabaseNames
                    ? (Func<DatabaseTable, string>)(t => t.Name)
                    : t => _candidateNamingService.GenerateCandidateIdentifier(t),
                _cSharpUtilities,
                _pluralizer.Singularize);
            _dbSetNamer = new CSharpUniqueNamer<DatabaseTable>(
                useDatabaseNames
                    ? (Func<DatabaseTable, string>)(t => t.Name)
                    : t => _candidateNamingService.GenerateCandidateIdentifier(t),
                _cSharpUtilities,
                _pluralizer.Pluralize);
            _columnNamers = new Dictionary<DatabaseTable, CSharpUniqueNamer<DatabaseColumn>>();
            _useDatabaseNames = useDatabaseNames;

            VisitDatabaseModel(modelBuilder, databaseModel);

            return modelBuilder.Model;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual string GetEntityTypeName(DatabaseTable table)
            => _tableNamer.GetName(table);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual string GetDbSetName(DatabaseTable table)
            => _dbSetNamer.GetName(table);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual string GetPropertyName(DatabaseColumn column)
        {

            var table = column.Table ?? _nullTable;
            var usedNames = new List<string>();
            // TODO - need to clean up the way CSharpNamer & CSharpUniqueNamer work (see issue #1671)
            if (column.Table != null)
            {
                usedNames.Add(GetEntityTypeName(table));
            }

            if (!_columnNamers.ContainsKey(table))
            {
                if (_useDatabaseNames)
                {
                    _columnNamers.Add(
                        table,
                        new CSharpUniqueNamer<DatabaseColumn>(
                            c => c.Name,
                            usedNames,
                            _cSharpUtilities,
                            singularizePluralizer: null));
                }
                else
                {
                    _columnNamers.Add(
                        table,
                        new CSharpUniqueNamer<DatabaseColumn>(
                            c => _candidateNamingService.GenerateCandidateIdentifier(c),
                            usedNames,
                            _cSharpUtilities,
                            singularizePluralizer: null));
                }
            }

            return _columnNamers[table].GetName(column);
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual ModelBuilder VisitDatabaseModel(ModelBuilder modelBuilder, DatabaseModel databaseModel)
        {

            if (!string.IsNullOrEmpty(databaseModel.DefaultSchema))
            {
                modelBuilder.HasDefaultSchema(databaseModel.DefaultSchema);
            }

            if (!string.IsNullOrEmpty(databaseModel.DatabaseName))
            {
                modelBuilder.Model.Scaffolding().DatabaseName = databaseModel.DatabaseName;
            }

            VisitSequences(modelBuilder, databaseModel.Sequences);
            VisitTables(modelBuilder, databaseModel.Tables);
            VisitForeignKeys(modelBuilder, databaseModel.Tables.SelectMany(table => table.ForeignKeys).ToList());

            modelBuilder.Model.AddAnnotations(databaseModel.GetAnnotations());

            return modelBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual ModelBuilder VisitSequences(ModelBuilder modelBuilder, ICollection<DatabaseSequence> sequences)
        {

            foreach (var sequence in sequences)
            {
                VisitSequence(modelBuilder, sequence);
            }

            return modelBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual SequenceBuilder VisitSequence(ModelBuilder modelBuilder, DatabaseSequence sequence)
        {

            if (string.IsNullOrEmpty(sequence.Name))
            {
                _reporter.WriteWarning(DesignStrings.SequencesRequireName);
                return null;
            }

            Type sequenceType = null;
            if (sequence.StoreType != null)
            {
                sequenceType = _scaffoldingTypeMapper.FindMapping(
                        sequence.StoreType,
                        keyOrIndex: false,
                        rowVersion: false)
                    ?.ClrType;
            }

            if (sequenceType != null
                && !Sequence.SupportedTypes.Contains(sequenceType))
            {
                _reporter.WriteWarning(DesignStrings.BadSequenceType(sequence.Name, sequence.StoreType));
                return null;
            }

            var builder = sequenceType != null
                ? modelBuilder.HasSequence(sequenceType, sequence.Name, sequence.Schema)
                : modelBuilder.HasSequence(sequence.Name, sequence.Schema);

            if (sequence.IncrementBy.HasValue)
            {
                builder.IncrementsBy(sequence.IncrementBy.Value);
            }

            if (sequence.MaxValue.HasValue)
            {
                builder.HasMax(sequence.MaxValue.Value);
            }

            if (sequence.MinValue.HasValue)
            {
                builder.HasMin(sequence.MinValue.Value);
            }

            if (sequence.StartValue.HasValue)
            {
                builder.StartsAt(sequence.StartValue.Value);
            }

            if (sequence.IsCyclic.HasValue)
            {
                builder.IsCyclic(sequence.IsCyclic.Value);
            }

            return builder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual ModelBuilder VisitTables(ModelBuilder modelBuilder, ICollection<DatabaseTable> tables)
        {

            foreach (var table in tables)
            {
                VisitTable(modelBuilder, table);
            }

            return modelBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual EntityTypeBuilder VisitTable(ModelBuilder modelBuilder, DatabaseTable table)
        {

            var entityTypeName = GetEntityTypeName(table);

            var builder = modelBuilder.Entity(entityTypeName);

            var dbSetName = GetDbSetName(table);
            builder.Metadata.Scaffolding().DbSetName = dbSetName;

            builder.ToTable(table.Name, table.Schema);

            VisitColumns(builder, table.Columns);

            var keyBuilder = VisitPrimaryKey(builder, table);

            if (keyBuilder == null)
            {
                var errorMessage = DesignStrings.UnableToGenerateEntityType(table.DisplayName());
                _reporter.WriteWarning(errorMessage);

                var model = modelBuilder.Model;
                model.RemoveEntityType(entityTypeName);
                model.Scaffolding().EntityTypeErrors.Add(entityTypeName, errorMessage);
                return null;
            }

            VisitUniqueConstraints(builder, table.UniqueConstraints);
            VisitIndexes(builder, table.Indexes);

            builder.Metadata.AddAnnotations(table.GetAnnotations());

            return builder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual EntityTypeBuilder VisitColumns(EntityTypeBuilder builder, ICollection<DatabaseColumn> columns)
        {

            foreach (var column in columns)
            {
                VisitColumn(builder, column);
            }

            return builder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual PropertyBuilder VisitColumn(EntityTypeBuilder builder, DatabaseColumn column)
        {

            var typeScaffoldingInfo = GetTypeScaffoldingInfo(column);

            if (typeScaffoldingInfo == null)
            {
                _unmappedColumns.Add(column);
                _reporter.WriteWarning(
                    DesignStrings.CannotFindTypeMappingForColumn(column.DisplayName(), column.StoreType));
                return null;
            }

            var clrType = typeScaffoldingInfo.ClrType;
            if (column.IsNullable)
            {
                clrType = clrType.MakeNullable();
            }

            if (clrType == typeof(bool)
                && column.DefaultValueSql != null)
            {
                _reporter.WriteWarning(
                    DesignStrings.NonNullableBoooleanColumnHasDefaultConstraint(column.DisplayName()));

                clrType = clrType.MakeNullable();
            }

            var property = builder.Property(clrType, GetPropertyName(column));

            property.HasColumnName(column.Name);

            if (!typeScaffoldingInfo.IsInferred
                && !string.IsNullOrWhiteSpace(column.StoreType))
            {
                property.HasColumnType(column.StoreType);
            }

            if (typeScaffoldingInfo.ScaffoldUnicode.HasValue)
            {
                property.IsUnicode(typeScaffoldingInfo.ScaffoldUnicode.Value);
            }

            if (typeScaffoldingInfo.ScaffoldFixedLength == true)
            {
                property.IsFixedLength();
            }

            if (typeScaffoldingInfo.ScaffoldMaxLength.HasValue)
            {
                property.HasMaxLength(typeScaffoldingInfo.ScaffoldMaxLength.Value);
            }

            if (column.ValueGenerated == ValueGenerated.OnAdd)
            {
                property.ValueGeneratedOnAdd();
            }

            if (column.ValueGenerated == ValueGenerated.OnUpdate)
            {
                property.ValueGeneratedOnUpdate();
            }

            if (column.ValueGenerated == ValueGenerated.OnAddOrUpdate)
            {
                property.ValueGeneratedOnAddOrUpdate();
            }

            if (column.DefaultValueSql != null)
            {
                property.HasDefaultValueSql(column.DefaultValueSql);
            }

            if (column.ComputedColumnSql != null)
            {
                property.HasComputedColumnSql(column.ComputedColumnSql);
            }

            if (!(column.Table.PrimaryKey?.Columns.Contains(column) ?? false))
            {
                property.IsRequired(!column.IsNullable);
            }

            if ((bool?)column[ScaffoldingAnnotationNames.ConcurrencyToken] == true)
            {
                property.IsConcurrencyToken();
            }

            property.Metadata.Scaffolding().ColumnOrdinal = column.Table.Columns.IndexOf(column);

            property.Metadata.AddAnnotations(
                column.GetAnnotations().Where(
                    a => a.Name != ScaffoldingAnnotationNames.UnderlyingStoreType
                         && a.Name != ScaffoldingAnnotationNames.ConcurrencyToken));

            return property;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual KeyBuilder VisitPrimaryKey(EntityTypeBuilder builder, DatabaseTable table)
        {

            var primaryKey = table.PrimaryKey;
            if (primaryKey == null)
            {
                _reporter.WriteWarning(DesignStrings.MissingPrimaryKey(table.DisplayName()));
                return null;
            }

            var unmappedColumns = primaryKey.Columns
                .Where(c => _unmappedColumns.Contains(c))
                .Select(c => c.Name)
                .ToList();
            if (unmappedColumns.Any())
            {
                _reporter.WriteWarning(
                    DesignStrings.PrimaryKeyErrorPropertyNotFound(
                        table.DisplayName(),
                        string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, unmappedColumns)));
                return null;
            }

            var keyBuilder = builder.HasKey(primaryKey.Columns.Select(GetPropertyName).ToArray());

            if (primaryKey.Columns.Count == 1
                && primaryKey.Columns[0].ValueGenerated == null
                && primaryKey.Columns[0].DefaultValueSql == null)
            {
                var property = builder.Metadata.FindProperty(GetPropertyName(primaryKey.Columns[0]))?.AsProperty();
                if (property != null)
                {
                    var conventionalValueGenerated = new RelationalValueGeneratorConvention().GetValueGenerated(property);
                    if (conventionalValueGenerated == ValueGenerated.OnAdd)
                    {
                        property.ValueGenerated = ValueGenerated.Never;
                    }
                }
            }

            if (!string.IsNullOrEmpty(primaryKey.Name)
                && primaryKey.Name != ConstraintNamer.GetDefaultName(keyBuilder.Metadata))
            {
                keyBuilder.HasName(primaryKey.Name);
            }

            keyBuilder.Metadata.AddAnnotations(primaryKey.GetAnnotations());

            return keyBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual EntityTypeBuilder VisitUniqueConstraints(EntityTypeBuilder builder, ICollection<DatabaseUniqueConstraint> uniqueConstraints)
        {

            foreach (var uniqueConstraint in uniqueConstraints)
            {
                VisitUniqueConstraint(builder, uniqueConstraint);
            }

            return builder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual IndexBuilder VisitUniqueConstraint(EntityTypeBuilder builder, DatabaseUniqueConstraint uniqueConstraint)
        {

            var unmappedColumns = uniqueConstraint.Columns
                .Where(c => _unmappedColumns.Contains(c))
                .Select(c => c.Name)
                .ToList();
            if (unmappedColumns.Any())
            {
                _reporter.WriteWarning(
                    DesignStrings.UnableToScaffoldIndexMissingProperty(
                        uniqueConstraint.Name,
                        string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, unmappedColumns)));
                return null;
            }

            var propertyNames = uniqueConstraint.Columns.Select(GetPropertyName).ToArray();
            var indexBuilder = builder.HasIndex(propertyNames).IsUnique();

            if (!string.IsNullOrEmpty(uniqueConstraint.Name)
                && uniqueConstraint.Name != ConstraintNamer.GetDefaultName(indexBuilder.Metadata))
            {
                indexBuilder.HasName(uniqueConstraint.Name);
            }

            indexBuilder.Metadata.AddAnnotations(uniqueConstraint.GetAnnotations());

            return indexBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual EntityTypeBuilder VisitIndexes(EntityTypeBuilder builder, ICollection<DatabaseIndex> indexes)
        {

            foreach (var index in indexes)
            {
                VisitIndex(builder, index);
            }

            return builder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual IndexBuilder VisitIndex(EntityTypeBuilder builder, DatabaseIndex index)
        {

            var unmappedColumns = index.Columns
                .Where(c => _unmappedColumns.Contains(c))
                .Select(c => c.Name)
                .ToList();
            if (unmappedColumns.Any())
            {
                _reporter.WriteWarning(
                    DesignStrings.UnableToScaffoldIndexMissingProperty(
                        index.Name,
                        string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, unmappedColumns)));
                return null;
            }

            var propertyNames = index.Columns.Select(GetPropertyName).ToArray();
            var indexBuilder = builder.HasIndex(propertyNames)
                .IsUnique(index.IsUnique);

            if (index.Filter != null)
            {
                indexBuilder.HasFilter(index.Filter);
            }

            if (!string.IsNullOrEmpty(index.Name)
                && index.Name != ConstraintNamer.GetDefaultName(indexBuilder.Metadata))
            {
                indexBuilder.HasName(index.Name);
            }

            indexBuilder.Metadata.AddAnnotations(index.GetAnnotations());

            return indexBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual ModelBuilder VisitForeignKeys(ModelBuilder modelBuilder, IList<DatabaseForeignKey> foreignKeys)
        {
            foreach (var fk in foreignKeys)
            {
                VisitForeignKey(modelBuilder, fk);
            }

            // Note: must completely assign all foreign keys before assigning
            // navigation properties otherwise naming of navigation properties
            // when there are multiple foreign keys does not work.
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(et => et.GetForeignKeys()))
            {
                AddNavigationProperties(foreignKey);
            }

            return modelBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual IMutableForeignKey VisitForeignKey(ModelBuilder modelBuilder, DatabaseForeignKey foreignKey)
        {

            if (foreignKey.PrincipalTable == null)
            {
                _reporter.WriteWarning(
                    DesignStrings.ForeignKeyScaffoldErrorPrincipalTableNotFound(foreignKey.DisplayName()));
                return null;
            }

            if (foreignKey.Table == null)
            {
                return null;
            }

            var dependentEntityType = modelBuilder.Model.FindEntityType(GetEntityTypeName(foreignKey.Table));

            if (dependentEntityType == null)
            {
                return null;
            }

            var unmappedDependentColumns = foreignKey.Columns
                .Where(c => _unmappedColumns.Contains(c))
                .Select(c => c.Name)
                .ToList();
            if (unmappedDependentColumns.Any())
            {
                _reporter.WriteWarning(
                    DesignStrings.ForeignKeyScaffoldErrorPropertyNotFound(
                        foreignKey.DisplayName(),
                        string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, unmappedDependentColumns)));
                return null;
            }

            var dependentProperties = foreignKey.Columns
                .Select(GetPropertyName)
                .Select(name => dependentEntityType.FindProperty(name))
                .ToList()
                .AsReadOnly();

            var principalEntityType = modelBuilder.Model.FindEntityType(GetEntityTypeName(foreignKey.PrincipalTable));
            if (principalEntityType == null)
            {
                _reporter.WriteWarning(
                    DesignStrings.ForeignKeyScaffoldErrorPrincipalTableScaffoldingError(
                        foreignKey.DisplayName(),
                        foreignKey.PrincipalTable.DisplayName()));
                return null;
            }

            var unmappedPrincipalColumns = foreignKey.PrincipalColumns
                .Where(pc => principalEntityType.FindProperty(GetPropertyName(pc)) == null)
                .Select(pc => pc.Name)
                .ToList();
            if (unmappedPrincipalColumns.Any())
            {
                _reporter.WriteWarning(
                    DesignStrings.ForeignKeyScaffoldErrorPropertyNotFound(
                        foreignKey.DisplayName(),
                        string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, unmappedPrincipalColumns)));
                return null;
            }

            var principalPropertiesMap = foreignKey.PrincipalColumns
                .Select(
                    fc => (property: principalEntityType.FindProperty(GetPropertyName(fc)), column: fc)).ToList();
            var principalProperties = principalPropertiesMap
                .Select(tuple => tuple.property)
                .ToList();

            var principalKey = principalEntityType.FindKey(principalProperties);
            if (principalKey == null)
            {
                var index = principalEntityType.FindIndex(principalProperties.AsReadOnly());
                if (index != null
                    && index.IsUnique)
                {
                    // ensure all principal properties are non-nullable even if the columns
                    // are nullable on the database. EF's concept of a key requires this.
                    var nullablePrincipalProperties =
                        principalPropertiesMap.Where(tuple => tuple.property.IsNullable).ToList();
                    if (nullablePrincipalProperties.Any())
                    {
                        _reporter.WriteWarning(
                            DesignStrings.ForeignKeyPrincipalEndContainsNullableColumns(
                                foreignKey.DisplayName(),
                                index.Relational().Name,
                                nullablePrincipalProperties.Select(tuple => tuple.column.DisplayName()).ToList()
                                    .Aggregate((a, b) => a + "," + b)));

                        nullablePrincipalProperties
                            .ToList()
                            .ForEach(tuple => tuple.property.IsNullable = false);
                    }

                    principalKey = principalEntityType.AddKey(principalProperties);
                }
                else
                {
                    var principalColumns = foreignKey.PrincipalColumns.Select(c => c.Name).ToList();

                    _reporter.WriteWarning(
                        DesignStrings.ForeignKeyScaffoldErrorPrincipalKeyNotFound(
                            foreignKey.DisplayName(),
                            string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, principalColumns),
                            principalEntityType.DisplayName()));

                    return null;
                }
            }

            var key = dependentEntityType.GetOrAddForeignKey(
                dependentProperties, principalKey, principalEntityType);

            var dependentKey = dependentEntityType.FindKey(dependentProperties);
            var dependentIndex = dependentEntityType.FindIndex(dependentProperties);
            key.IsUnique = dependentKey != null
                           || dependentIndex != null && dependentIndex.IsUnique;

            if (!string.IsNullOrEmpty(foreignKey.Name)
                && foreignKey.Name != ConstraintNamer.GetDefaultName(key))
            {
                key.Relational().Name = foreignKey.Name;
            }

            AssignOnDeleteAction(foreignKey, key);

            key.AddAnnotations(foreignKey.GetAnnotations());

            return key;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual void AddNavigationProperties(IMutableForeignKey foreignKey)
        {

            var dependentEndExistingIdentifiers = ExistingIdentifiers(foreignKey.DeclaringEntityType);
            var dependentEndNavigationPropertyCandidateName =
                _candidateNamingService.GetDependentEndCandidateNavigationPropertyName(foreignKey);
            var dependentEndNavigationPropertyName =
                _cSharpUtilities.GenerateCSharpIdentifier(
                    dependentEndNavigationPropertyCandidateName,
                    dependentEndExistingIdentifiers,
                    singularizePluralizer: null,
                    uniquifier: NavigationUniquifier);

            foreignKey.HasDependentToPrincipal(dependentEndNavigationPropertyName);

            var principalEndExistingIdentifiers = ExistingIdentifiers(foreignKey.PrincipalEntityType);
            var principalEndNavigationPropertyCandidateName = foreignKey.IsSelfReferencing()
                ? string.Format(
                    CultureInfo.CurrentCulture,
                    SelfReferencingPrincipalEndNavigationNamePattern,
                    dependentEndNavigationPropertyName)
                : _candidateNamingService.GetPrincipalEndCandidateNavigationPropertyName(
                    foreignKey, dependentEndNavigationPropertyName);

            if (!foreignKey.IsUnique
                && !foreignKey.IsSelfReferencing())
            {
                principalEndNavigationPropertyCandidateName = _pluralizer.Pluralize(principalEndNavigationPropertyCandidateName);
            }

            var principalEndNavigationPropertyName =
                _cSharpUtilities.GenerateCSharpIdentifier(
                    principalEndNavigationPropertyCandidateName,
                    principalEndExistingIdentifiers,
                    singularizePluralizer: null,
                    uniquifier: NavigationUniquifier);

            foreignKey.HasPrincipalToDependent(principalEndNavigationPropertyName);
        }

        // Stores the names of the EntityType itself and its Properties, but does not include any Navigation Properties
        private readonly Dictionary<IEntityType, List<string>> _entityTypeAndPropertyIdentifiers = new Dictionary<IEntityType, List<string>>();

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual List<string> ExistingIdentifiers(IEntityType entityType)
        {

            if (!_entityTypeAndPropertyIdentifiers.TryGetValue(entityType, out var existingIdentifiers))
            {
                existingIdentifiers = new List<string>
                {
                    entityType.Name
                };
                existingIdentifiers.AddRange(entityType.GetProperties().Select(p => p.Name));
                _entityTypeAndPropertyIdentifiers[entityType] = existingIdentifiers;
            }

            existingIdentifiers.AddRange(entityType.GetNavigations().Select(p => p.Name));
            return existingIdentifiers;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual TypeScaffoldingInfo GetTypeScaffoldingInfo(DatabaseColumn column)
        {
            if (column.StoreType == null)
            {
                return null;
            }

            var typeScaffoldingInfo = _scaffoldingTypeMapper.FindMapping(
                column.GetUnderlyingStoreType() ?? column.StoreType,
                column.IsKeyOrIndex(),
                column.IsRowVersion());

            if (typeScaffoldingInfo == null)
            {
                return null;
            }

            return column.GetUnderlyingStoreType() != null
                ? new TypeScaffoldingInfo(
                    typeScaffoldingInfo.ClrType,
                    inferred: false,
                    scaffoldUnicode: typeScaffoldingInfo.ScaffoldUnicode,
                    scaffoldMaxLength: typeScaffoldingInfo.ScaffoldMaxLength,
                    scaffoldFixedLength: typeScaffoldingInfo.ScaffoldFixedLength)
                : typeScaffoldingInfo;
        }

        private static void AssignOnDeleteAction(
            DatabaseForeignKey databaseForeignKey, IMutableForeignKey foreignKey)
        {

            switch (databaseForeignKey.OnDelete)
            {
                case ReferentialAction.Cascade:
                    foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
                    break;

                case ReferentialAction.SetNull:
                    foreignKey.DeleteBehavior = DeleteBehavior.SetNull;
                    break;

                default:
                    foreignKey.DeleteBehavior = DeleteBehavior.ClientSetNull;
                    break;
            }
        }

        // TODO use CSharpUniqueNamer
        private static string NavigationUniquifier(string proposedIdentifier, ICollection<string> existingIdentifiers)
        {
            if (existingIdentifiers == null
                || !existingIdentifiers.Contains(proposedIdentifier))
            {
                return proposedIdentifier;
            }

            var finalIdentifier =
                string.Format(CultureInfo.CurrentCulture, NavigationNameUniquifyingPattern, proposedIdentifier);
            var suffix = 1;
            while (existingIdentifiers.Contains(finalIdentifier))
            {
                finalIdentifier = proposedIdentifier + suffix;
                suffix++;
            }

            return finalIdentifier;
        }
    }
}
