using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Common.Cli;
using RevEng.Common.Cli.Configuration;
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
        public void ExcludingPartialUniqueIndexPreservesForeignKeyAndGeneratesCollection(bool composite, bool useManyToManyEntity)
        {
            var database = CreateDatabase(composite);
            var model = Scaffold(database, new[] { FilteredIndex }, useManyToManyEntity: useManyToManyEntity);
            var child = FindChild(model);
            var foreignKey = Assert.Single(child.GetForeignKeys());

            Assert.False(foreignKey.IsUnique);
            Assert.True(foreignKey.PrincipalToDependent.IsCollection);
            Assert.Equal(composite ? 2 : 1, foreignKey.Properties.Count);
            Assert.Equal("fk_child_parent", foreignKey.GetConstraintName());
            Assert.Equal(composite ? 4 : 3, child.GetProperties().Count());
            Assert.DoesNotContain(child.GetIndexes(), i => i.GetDatabaseName() == FilteredIndex);
            Assert.Contains(child.GetIndexes(), i => i.GetDatabaseName() == "ix_child_parent");
            Assert.Single(database.Tables[1].ForeignKeys);
            Assert.NotNull(database.Tables[1].PrimaryKey);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("missing_index")]
        [InlineData("UQ_CHILD_ACTIVE_PARENT")]
        public void IndexIsNotExcludedWithoutAnExactName(string excludedIndex)
        {
            var exclusions = excludedIndex == null ? null : excludedIndex.Length == 0 ? new string[0] : new[] { excludedIndex };
            var model = Scaffold(CreateDatabase(), exclusions);
            var child = FindChild(model);

            // Documents the upstream inference bug; an explicit exclusion is required.
            Assert.True(Assert.Single(child.GetForeignKeys()).IsUnique);
            var index = Assert.Single(child.GetIndexes(), i => i.GetDatabaseName() == FilteredIndex);
            Assert.True(index.IsUnique);
            Assert.Equal("is_active = true", index.GetFilter());
        }

        [Fact]
        public void ExclusionIsScopedToTheConfiguredSchemaAndTable()
        {
            var database = CreateDatabase();
            var other = CreateDatabase(schema: "archive");
            foreach (var table in other.Tables)
            {
                table.Database = database;
                database.Tables.Add(table);
            }

            var model = Scaffold(database, new[] { FilteredIndex });

            Assert.False(Assert.Single(FindChild(model).GetForeignKeys()).IsUnique);
            Assert.True(Assert.Single(FindChild(model, "archive").GetForeignKeys()).IsUnique);
            Assert.Contains(FindChild(model, "archive").GetIndexes(), i => i.GetDatabaseName() == FilteredIndex);
        }

        [Fact]
        public void ExclusionDoesNotMatchDifferentlyCasedSchema()
        {
            var model = Scaffold(CreateDatabase(schema: "Public"), new[] { FilteredIndex });

            Assert.True(Assert.Single(FindChild(model, "Public").GetForeignKeys()).IsUnique);
            Assert.Contains(FindChild(model, "Public").GetIndexes(), i => i.GetDatabaseName() == FilteredIndex);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void VisualStudioAndCliConfigurationReachTheScaffoldingFactory(bool cli)
        {
            List<SerializationTableModel> tables;
            if (cli)
            {
                var config = JsonSerializer.Deserialize<CliConfig>("""
                    { "tables": [{ "name": "public.child", "excludedIndexes": ["uq_child_active_parent"] }] }
                    """);
                tables = CliConfigMapper.BuildObjectList(config);
            }
            else
            {
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes("""
                    [{ "Name": "public.child", "ObjectType": 0, "ExcludedIndexes": ["uq_child_active_parent"] }]
                    """));
                tables = (List<SerializationTableModel>)new DataContractJsonSerializer(typeof(List<SerializationTableModel>)).ReadObject(stream);
            }

            var model = Scaffold(CreateDatabase(), new ReverseEngineerCommandOptions
            {
                DatabaseType = DatabaseType.Npgsql,
                Tables = tables,
            });

            Assert.False(Assert.Single(FindChild(model).GetForeignKeys()).IsUnique);
            Assert.DoesNotContain(FindChild(model).GetIndexes(), i => i.GetDatabaseName() == FilteredIndex);
        }

        [Fact]
        public void RepeatedAndMissingExclusionsAreHarmless()
        {
            var model = Scaffold(CreateDatabase(), new[] { "missing_index", FilteredIndex, FilteredIndex });

            Assert.False(Assert.Single(FindChild(model).GetForeignKeys()).IsUnique);
        }

        [Fact]
        public void UnexcludedUniqueIndexStillImpliesOneToOne()
        {
            var database = CreateDatabase();
            var child = database.Tables[1];
            var unique = new DatabaseIndex { Table = child, Name = "uq_child_parent", IsUnique = true };
            unique.Columns.Add(child.Columns[1]);
            child.Indexes.Add(unique);

            var model = Scaffold(database, new[] { FilteredIndex });

            Assert.True(Assert.Single(FindChild(model).GetForeignKeys()).IsUnique);
            Assert.Contains(FindChild(model).GetIndexes(), i => i.GetDatabaseName() == unique.Name && i.IsUnique);
        }

        [Fact]
        public void ExcludingNonUniqueIndexDoesNotRemoveUnlistedUniqueIndex()
        {
            var model = Scaffold(CreateDatabase(), new[] { "ix_child_parent" });
            var child = FindChild(model);

            Assert.True(Assert.Single(child.GetForeignKeys()).IsUnique);
            Assert.DoesNotContain(child.GetIndexes(), i => i.GetDatabaseName() == "ix_child_parent");
            Assert.Contains(child.GetIndexes(), i => i.GetDatabaseName() == FilteredIndex && i.IsUnique);
        }

        [Fact]
        public void IndexExclusionDoesNotRemoveUniqueConstraints()
        {
            var database = CreateDatabase();
            var child = database.Tables[1];
            child.Indexes.RemoveAt(0);
            var constraint = new DatabaseUniqueConstraint { Table = child, Name = FilteredIndex };
            constraint.Columns.Add(child.Columns[1]);
            child.UniqueConstraints.Add(constraint);

            var model = Scaffold(database, new[] { FilteredIndex });

            Assert.Same(constraint, Assert.Single(child.UniqueConstraints));
            Assert.True(Assert.Single(FindChild(model).GetForeignKeys()).IsUnique);
        }

        [Fact]
        public void NullOnlyFilterIsPreservedWithoutExplicitExclusion()
        {
            var database = CreateDatabase();
            database.Tables[1].Columns[1].IsNullable = true;
            database.Tables[1].Indexes[0].Filter = "parent_id IS NOT NULL";

            var model = Scaffold(database, excludedIndexes: null);

            Assert.True(Assert.Single(FindChild(model).GetForeignKeys()).IsUnique);
            Assert.Equal("parent_id IS NOT NULL", Assert.Single(FindChild(model).GetIndexes(), i => i.IsUnique).GetFilter());
        }

        [Fact]
        public void IndexExclusionCanBeCombinedWithColumnExclusion()
        {
            var database = CreateDatabase();
            var child = database.Tables[1];
            var ignored = new DatabaseColumn { Table = child, Name = "ignored", StoreType = "integer" };
            child.Columns.Add(ignored);
            var index = new DatabaseIndex { Table = child, Name = "ix_child_ignored" };
            index.Columns.Add(ignored);
            child.Indexes.Add(index);

            var model = Scaffold(database, new[] { FilteredIndex }, excludedColumns: new[] { "ignored" });
            var entity = FindChild(model);

            Assert.False(Assert.Single(entity.GetForeignKeys()).IsUnique);
            Assert.DoesNotContain(entity.GetProperties(), p => p.GetColumnName() == "ignored");
            Assert.DoesNotContain(entity.GetIndexes(), i => i.GetDatabaseName() == index.Name);
        }

        private static IEntityType FindChild(IModel model, string schema = "public")
            => model.GetEntityTypes().Single(e => e.GetTableName() == "child" && e.GetSchema() == schema);

        private static IModel Scaffold(DatabaseModel database, string[] excludedIndexes, bool useManyToManyEntity = false, string[] excludedColumns = null)
        {
            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = DatabaseType.Npgsql,
                UseInflector = true,
                UseManyToManyEntity = useManyToManyEntity,
                Tables = new List<SerializationTableModel>
                {
                    new SerializationTableModel("public.child", ObjectType.Table, excludedColumns, excludedIndexes),
                },
            };
            return Scaffold(database, options);
        }

        private static IModel Scaffold(DatabaseModel database, ReverseEngineerCommandOptions options)
        {
            using var services = new ServiceCollection()
                .AddLogging()
                .AddEfpt(options, new List<string>(), new List<string>(), new List<string>())
                .BuildServiceProvider();

            return services.GetRequiredService<IScaffoldingModelFactory>().Create(database, new ModelReverseEngineerOptions());
        }

        private static DatabaseModel CreateDatabase(bool composite = false, string schema = "public")
        {
            var database = new DatabaseModel { DatabaseName = "IndexExclusionTests", DefaultSchema = "public" };
            var parent = new DatabaseTable { Database = database, Name = "parent", Schema = schema };
            var child = new DatabaseTable { Database = database, Name = "child", Schema = schema };
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
