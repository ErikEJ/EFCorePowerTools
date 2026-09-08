using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Core;
using Xunit;

namespace UnitTests
{
    public class PostgresIndexExclusionTests
    {
        private const string FilteredIndex = "uq_child_active_parent";

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void PartialUniqueIndexIsExcludedBeforeRelationshipInference(bool composite, bool excludeIndex)
        {
            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = DatabaseType.Npgsql,
                UseInflector = true,
                Tables = new List<SerializationTableModel>
                {
                    new SerializationTableModel("public.child", ObjectType.Table, null, excludeIndex ? new[] { FilteredIndex } : null),
                },
            };
            using var services = new ServiceCollection()
                .AddLogging()
                .AddEfpt(options, new List<string>(), new List<string>(), new List<string>())
                .BuildServiceProvider();

            var model = services.GetRequiredService<IScaffoldingModelFactory>()
                .Create(CreateDatabase(composite), new ModelReverseEngineerOptions());
            var child = model.GetEntityTypes().Single(e => e.GetTableName() == "child");
            var foreignKey = Assert.Single(child.GetForeignKeys());

            // Without an explicit exclusion, preserve the existing upstream inference behavior.
            Assert.Equal(!excludeIndex, foreignKey.IsUnique);
            Assert.Equal(excludeIndex, foreignKey.PrincipalToDependent.IsCollection);
            Assert.Equal(composite ? 2 : 1, foreignKey.Properties.Count);
            Assert.Equal("fk_child_parent", foreignKey.GetConstraintName());
            Assert.Equal(composite ? 4 : 3, child.GetProperties().Count());
            Assert.Equal(!excludeIndex, child.GetIndexes().Any(i => i.GetDatabaseName() == FilteredIndex));
            Assert.Contains(child.GetIndexes(), i => i.GetDatabaseName() == "ix_child_parent");
        }

        private static DatabaseModel CreateDatabase(bool composite)
        {
            var database = new DatabaseModel { DatabaseName = "IndexExclusionTests", DefaultSchema = "public" };
            var parent = new DatabaseTable { Database = database, Name = "parent", Schema = "public" };
            var child = new DatabaseTable { Database = database, Name = "child", Schema = "public" };
            database.Tables.Add(parent);
            database.Tables.Add(child);

            var parentId = new DatabaseColumn { Table = parent, Name = "id", StoreType = "integer" };
            parent.Columns.Add(parentId);
            parent.PrimaryKey = new DatabasePrimaryKey { Table = parent, Name = "pk_parent" };
            parent.PrimaryKey.Columns.Add(parentId);

            var childId = new DatabaseColumn { Table = child, Name = "id", StoreType = "integer" };
            var childParentId = new DatabaseColumn { Table = child, Name = "parent_id", StoreType = "integer" };
            child.Columns.Add(childId);
            child.Columns.Add(childParentId);
            child.Columns.Add(new DatabaseColumn { Table = child, Name = "is_active", StoreType = "boolean" });
            child.PrimaryKey = new DatabasePrimaryKey { Table = child, Name = "pk_child" };
            child.PrimaryKey.Columns.Add(childId);

            var foreignKey = new DatabaseForeignKey { Table = child, PrincipalTable = parent, Name = "fk_child_parent" };
            foreignKey.Columns.Add(childParentId);
            foreignKey.PrincipalColumns.Add(parentId);
            child.ForeignKeys.Add(foreignKey);

            var index = new DatabaseIndex { Table = child, Name = FilteredIndex, IsUnique = true, Filter = "is_active = true" };
            index.Columns.Add(childParentId);
            child.Indexes.Add(index);
            var regularIndex = new DatabaseIndex { Table = child, Name = "ix_child_parent" };
            regularIndex.Columns.Add(childParentId);
            child.Indexes.Add(regularIndex);

            if (composite)
            {
                var parentTenant = new DatabaseColumn { Table = parent, Name = "tenant_id", StoreType = "integer" };
                var childTenant = new DatabaseColumn { Table = child, Name = "tenant_id", StoreType = "integer" };
                parent.Columns.Add(parentTenant);
                parent.PrimaryKey.Columns.Add(parentTenant);
                child.Columns.Add(childTenant);
                foreignKey.Columns.Add(childTenant);
                foreignKey.PrincipalColumns.Add(parentTenant);
                index.Columns.Add(childTenant);
                regularIndex.Columns.Add(childTenant);
            }

            return database;
        }
    }
}
