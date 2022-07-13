﻿#if CORE60
using System.Diagnostics.CodeAnalysis;
#else
using JetBrains.Annotations;
#endif
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RevEng.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RevEng.Core
{
    public class ColumnRemovingScaffoldingModelFactory : RelationalScaffoldingModelFactory
    {
        private readonly List<SerializationTableModel> tables;
        private readonly DatabaseType databaseType;
#if CORE60
        private readonly bool ignoreManyToMany;
#endif

#if CORE60 && !CORE70
        public ColumnRemovingScaffoldingModelFactory([NotNull] IOperationReporter reporter, [NotNull] ICandidateNamingService candidateNamingService, [NotNull] IPluralizer pluralizer, [NotNull] ICSharpUtilities cSharpUtilities, [NotNull] IScaffoldingTypeMapper scaffoldingTypeMapper, [NotNull] LoggingDefinitions loggingDefinitions, [NotNull] IModelRuntimeInitializer modelRuntimeInitializer, List<SerializationTableModel> tables, DatabaseType databaseType, bool ignoreManyToMany)
            : base(reporter, candidateNamingService, pluralizer, cSharpUtilities, scaffoldingTypeMapper, loggingDefinitions, modelRuntimeInitializer)
#elif CORE70
        public ColumnRemovingScaffoldingModelFactory([NotNull] IOperationReporter reporter, [NotNull] ICandidateNamingService candidateNamingService, [NotNull] IPluralizer pluralizer, [NotNull] ICSharpUtilities cSharpUtilities, [NotNull] IScaffoldingTypeMapper scaffoldingTypeMapper, [NotNull] LoggingDefinitions loggingDefinitions, [NotNull] IModelRuntimeInitializer modelRuntimeInitializer, List<SerializationTableModel> tables, DatabaseType databaseType, bool ignoreManyToMany)
            : base(reporter, candidateNamingService, pluralizer, cSharpUtilities, scaffoldingTypeMapper, modelRuntimeInitializer)
#else
        public ColumnRemovingScaffoldingModelFactory([NotNull] IOperationReporter reporter, [NotNull] ICandidateNamingService candidateNamingService, [NotNull] IPluralizer pluralizer, [NotNull] ICSharpUtilities cSharpUtilities, [NotNull] IScaffoldingTypeMapper scaffoldingTypeMapper, [NotNull] LoggingDefinitions loggingDefinitions, List<SerializationTableModel> tables, DatabaseType databaseType)
            : base(reporter, candidateNamingService, pluralizer, cSharpUtilities, scaffoldingTypeMapper, loggingDefinitions)
#endif
        {
            this.tables = tables;
            this.databaseType = databaseType;
#if CORE60
            this.ignoreManyToMany = ignoreManyToMany;
#endif
        }

        protected override EntityTypeBuilder VisitTable(ModelBuilder modelBuilder, DatabaseTable table)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            string name;
            if (databaseType == DatabaseType.SQLServer || databaseType == DatabaseType.SQLServerDacpac)
            {
                name = $"[{table.Schema}].[{table.Name}]";
            }
            else
            {
                name = string.IsNullOrEmpty(table.Schema)
                    ? table.Name
                    : $"{table.Schema}.{table.Name}";
            }

            var excludedColumns = new List<DatabaseColumn>();
            var tableDefinition = tables.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (tableDefinition?.ExcludedColumns != null)
            {
                foreach (var column in tableDefinition?.ExcludedColumns)
                {
                    var columnToRemove = table.Columns.FirstOrDefault(c => c.Name.Equals(column, StringComparison.OrdinalIgnoreCase));
                    if (columnToRemove != null)
                    {
                        excludedColumns.Add(columnToRemove);
                        table.Columns.Remove(columnToRemove);
                    }
                }
            }

            if (excludedColumns.Count > 0)
            {
                var indexesToBeRemoved = new List<DatabaseIndex>();
                foreach (var index in table.Indexes)
                {
                    foreach (var column in index.Columns)
                    {
                        if (excludedColumns.Contains(column))
                        {
                            indexesToBeRemoved.Add(index);
                        }
                    }
                }

                foreach (var index in indexesToBeRemoved)
                {
                    table.Indexes.Remove(index);
                }
            }

            return base.VisitTable(modelBuilder, table);
        }

#if CORE60
        protected override ModelBuilder VisitForeignKeys(ModelBuilder modelBuilder, IList<DatabaseForeignKey> foreignKeys)
        {
            ArgumentNullException.ThrowIfNull(foreignKeys);

            ArgumentNullException.ThrowIfNull(modelBuilder);

            if (ignoreManyToMany)
            {
                foreach (var fk in foreignKeys)
                {
                    VisitForeignKey(modelBuilder, fk);
                }

                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var foreignKey in entityType.GetForeignKeys())
                    {
                        AddNavigationProperties(foreignKey);
                    }
                }

                return modelBuilder;
            }

            return base.VisitForeignKeys(modelBuilder, foreignKeys);
        }
#endif
    }
}
