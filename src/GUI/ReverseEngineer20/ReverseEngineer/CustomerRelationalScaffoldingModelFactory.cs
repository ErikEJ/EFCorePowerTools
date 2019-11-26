using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ReverseEngineer20.ReverseEngineer
{
    public class CustomerRelationalScaffoldingModelFactory : RelationalScaffoldingModelFactory
    {
        private const string NavigationNameUniquifyingPattern = "{0}Navigation";
        private const string SelfReferencingPrincipalEndNavigationNamePattern = "Inverse{0}";
        private CustomerCSharpUniqueNamer<DatabaseTable> _dbSetNamer;
        private CustomerCSharpUniqueNamer<DatabaseTable> _tableNamer;
        private Dictionary<DatabaseTable, CSharpUniqueNamer<DatabaseColumn>> _columnNamers;
        private CustomerOptionSelector _option;
        private bool _useDatabaseNames;
        private readonly IEnhanceCandidateNamingService _candidateNamingService;
        private readonly IPluralizer _pluralizer;
        private readonly ICSharpUtilities _cSharpUtilities;

        public CustomerRelationalScaffoldingModelFactory([NotNull] IOperationReporter reporter,
            [NotNull] IEnhanceCandidateNamingService candidateNamingService, 
            [NotNull] IPluralizer pluralizer, [NotNull] ICSharpUtilities cSharpUtilities, 
            [NotNull] IScaffoldingTypeMapper scaffoldingTypeMapper, CustomerOptionSelector schemas) :
            base(reporter, candidateNamingService, pluralizer, cSharpUtilities, scaffoldingTypeMapper)
        {
            _cSharpUtilities = cSharpUtilities;
            _pluralizer = pluralizer;
            _candidateNamingService = candidateNamingService;
            _option = schemas;
        }

        public override IModel Create(DatabaseModel databaseModel, bool useDatabaseNames)
        {
            CheckNotNull(databaseModel, nameof(databaseModel));

            var modelBuilder = new ModelBuilder(new ConventionSet());

            _tableNamer = GenerateCustomerCSharpUniqueNamer(GenerateTableFuncCandidate(useDatabaseNames),
                useDatabaseNames ? (Func<string, string>)null : _pluralizer.Singularize);

            _dbSetNamer = GenerateCustomerCSharpUniqueNamer(GenerateDbsetFuncCandidate(useDatabaseNames),
                useDatabaseNames ? (Func<string, string>)null : _pluralizer.Pluralize);

            _columnNamers = new Dictionary<DatabaseTable, CSharpUniqueNamer<DatabaseColumn>>();
            _useDatabaseNames = useDatabaseNames;

            VisitDatabaseModel(modelBuilder, databaseModel);

            return modelBuilder.Model;
        }

        protected override string GetEntityTypeName([NotNull] DatabaseTable table)
            => _tableNamer.GetName(CheckNotNull(table, nameof(table)));

        protected override string GetDbSetName([NotNull] DatabaseTable table)
            => _dbSetNamer.GetName(table);

        protected override string GetPropertyName([NotNull] DatabaseColumn column)
        {
            CheckNotNull(column, nameof(column));

            var table = column.Table ?? new DatabaseTable();
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

        protected void AddNavigationProperties([NotNull] IMutableForeignKey foreignKey, DatabaseTable table)
        {
            CheckNotNull(foreignKey, nameof(foreignKey));

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
                if (!_option.HasVariableName(table?.Schema, table?.Name))
                    principalEndNavigationPropertyCandidateName = _pluralizer.Pluralize(principalEndNavigationPropertyCandidateName);
                else
                    principalEndNavigationPropertyCandidateName = _option.GetTable(table.Schema, table.Name).VariableName;
            }

            var principalEndNavigationPropertyName =
                _cSharpUtilities.GenerateCSharpIdentifier(
                    principalEndNavigationPropertyCandidateName,
                    principalEndExistingIdentifiers,
                    singularizePluralizer: null,
                    uniquifier: NavigationUniquifier);

            foreignKey.HasPrincipalToDependent(principalEndNavigationPropertyName);
        }

        protected override ModelBuilder VisitForeignKeys([NotNull] ModelBuilder modelBuilder, [NotNull] IList<DatabaseForeignKey> foreignKeys)
        {
            CheckNotNull(modelBuilder, nameof(modelBuilder));
            CheckNotNull(foreignKeys, nameof(foreignKeys));
            var fkCach = new Dictionary<string, DatabaseTable>();
            foreach (var fk in foreignKeys)
            {
                var foreign = VisitForeignKey(modelBuilder, fk);
                if(!string.IsNullOrEmpty(foreign.DeclaringEntityType.Name) && !fkCach.ContainsKey(foreign.DeclaringEntityType.Name))
                    fkCach.Add(foreign.DeclaringEntityType.Name, fk.Table);
            }

            // Note: must completely assign all foreign keys before assigning
            // navigation properties otherwise naming of navigation properties
            // when there are multiple foreign keys does not work.
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(et => et.GetForeignKeys()))
            {
                if (!string.IsNullOrEmpty(foreignKey.DeclaringEntityType.Name) && fkCach.ContainsKey(foreignKey.DeclaringEntityType.Name))
                    AddNavigationProperties(foreignKey, fkCach[foreignKey.DeclaringEntityType.Name]);
                else
                    AddNavigationProperties(foreignKey, null);
            }

            return modelBuilder;
        }

        private CustomerCSharpUniqueNamer<DatabaseTable> GenerateCustomerCSharpUniqueNamer(Func<DatabaseTable, string> nameGatter,
            Func<string, string> pluralizer)
        {
            return
                new CustomerCSharpUniqueNamer<DatabaseTable>(
                    (t) => _option.HasCustomTableName(t.Schema, t.Name),
                    nameGatter,
                    _cSharpUtilities,
                    pluralizer); ;
        }

        private Func<DatabaseTable, string> GenerateTableFuncCandidate(bool useDatabaseNames)
        {
            Func<DatabaseTable, string> result;
            if (useDatabaseNames)
                result = t => t.Name;
            else
                result = t => _candidateNamingService.GenerateCandidateIdentifier(t);
            return result;
        }

        private Func<DatabaseTable, string> GenerateDbsetFuncCandidate(bool useDatabaseNames)
        {
            Func<DatabaseTable, string> result;
            if (useDatabaseNames)
                result = t => t.Name;
            else
                result = t => _candidateNamingService.GenerateDbSetCandidateIdentifier(t);
            return result;
        }

        private T CheckNotNull<T>(T obj, string objName)
        {
            if (obj == null)
                throw new ArgumentNullException($"{objName}");

            return obj;
        }

        private static string NavigationUniquifier([NotNull] string proposedIdentifier, [CanBeNull] ICollection<string> existingIdentifiers)
        {
            if (existingIdentifiers?.Contains(proposedIdentifier) != true)
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
