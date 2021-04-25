﻿#if CORE60
using System.Diagnostics.CodeAnalysis;
#else
using JetBrains.Annotations;
#endif
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RevEng.Shared;
using System;
using System.Collections.Generic;

using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RevEng.Core
{
    public class ColumnRemovingScaffoldingModelFactory : RelationalScaffoldingModelFactory
    {
        private readonly List<SerializationTableModel> _tables;
        private readonly DatabaseType _databaseType;

#if CORE60
        public ColumnRemovingScaffoldingModelFactory([NotNull] IOperationReporter reporter, [NotNull] ICandidateNamingService candidateNamingService, [NotNull] IPluralizer pluralizer, [NotNull] ICSharpUtilities cSharpUtilities, [NotNull] IScaffoldingTypeMapper scaffoldingTypeMapper, [NotNull] LoggingDefinitions loggingDefinitions, [NotNull] IModelRuntimeInitializer modelRuntimeInitializer, List<SerializationTableModel> tables, DatabaseType databaseType) :
            base(reporter, candidateNamingService, pluralizer, cSharpUtilities, scaffoldingTypeMapper, loggingDefinitions, modelRuntimeInitializer)
#else
        public ColumnRemovingScaffoldingModelFactory([NotNull] IOperationReporter reporter, [NotNull] ICandidateNamingService candidateNamingService, [NotNull] IPluralizer pluralizer, [NotNull] ICSharpUtilities cSharpUtilities, [NotNull] IScaffoldingTypeMapper scaffoldingTypeMapper, [NotNull] LoggingDefinitions loggingDefinitions, List<SerializationTableModel> tables, DatabaseType databaseType) :
            base(reporter, candidateNamingService, pluralizer, cSharpUtilities, scaffoldingTypeMapper, loggingDefinitions)
#endif
        {
            _tables = tables;
            _databaseType = databaseType;
        }

        protected override EntityTypeBuilder VisitTable(ModelBuilder modelBuilder, DatabaseTable table)
        {
            string name;
            if (_databaseType == DatabaseType.SQLServer || _databaseType == DatabaseType.SQLServerDacpac)
            {
                name = $"[{table.Schema}].[{table.Name}]";
            }
            else
            {
                name = string.IsNullOrEmpty(table.Schema)
                    ? table.Name
                    : $"{table.Schema}.{table.Name}";
            }

            var tableDefinition = _tables.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (tableDefinition?.ExcludedColumns != null)
            {
                foreach (var column in tableDefinition?.ExcludedColumns)
                {
                    var columnToRemove = table.Columns.FirstOrDefault(c => c.Name.Equals(column, StringComparison.OrdinalIgnoreCase));
                    if (columnToRemove != null)
                    {
                        table.Columns.Remove(columnToRemove);
                    }
                }
            }

            return base.VisitTable(modelBuilder, table);
        }
    }
}
