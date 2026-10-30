using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using NUnitTestCore;
using Testcontainers.MsSql;
using Xunit;

namespace SqlServerIntegrationTests;

// Feasibility test only: this does not enable system-view scaffolding in the CLI.
public sealed class MasterDacpacObjectFallbackTest : IAsyncLifetime
{
    private readonly MsSqlContainer server = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-24.04")
        .WithPassword($"Efpt!{Guid.NewGuid():N}")
        .Build();

    public async ValueTask InitializeAsync()
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        timeout.CancelAfter(TimeSpan.FromMinutes(5));
        try
        {
            await server.StartAsync(timeout.Token);
        }
        catch
        {
            await server.DisposeAsync();
            throw;
        }
    }

    public ValueTask DisposeAsync() => server.DisposeAsync();

    [Fact]
    public async Task ObjectFallbackCanQueryActualSystemViewValues()
    {
        var database = SystemViewObjectFallback.Create("databases");
        var options = new DbContextOptionsBuilder<ObjectFallbackContext>()
            .UseSqlServer(server.GetConnectionString())
            .Options;
        await using var context = new ObjectFallbackContext(options, database.Tables.Single());
        var databaseName = context.Database.GetDbConnection().Database;
        var query = context.Set<Dictionary<string, object>>("SystemView")
            .Where(row => (string)EF.Property<object>(row, "name") == databaseName);

        var result = await query.SingleAsync(TestContext.Current.CancellationToken);
        Assert.Equal(databaseName, Assert.IsType<string>(result["name"]));
        Assert.IsType<int>(result["database_id"]);
        Assert.IsType<DateTime>(result["create_date"]);
        Assert.IsType<bool>(result["is_read_only"]);
        Assert.Equal(database.Tables.Single().Columns.Count, result.Count);
        var id = await query.Select(row => (int)EF.Property<object>(row, "database_id"))
            .SingleAsync(TestContext.Current.CancellationToken);
        Assert.Equal(result["database_id"], id);
        Assert.Empty(context.ChangeTracker.Entries());
        Assert.Empty(context.Model.GetRelationalModel().Tables);
        Assert.Single(context.Model.GetRelationalModel().Views);
    }

    private sealed class ObjectFallbackContext : DbContext
    {
        private readonly DatabaseTable view;

        public ObjectFallbackContext(DbContextOptions<ObjectFallbackContext> options, DatabaseTable view)
            : base(options)
        {
            this.view = view;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.SharedTypeEntity<Dictionary<string, object>>("SystemView");
            entity.HasNoKey().ToView(view.Name, view.Schema);
            foreach (var column in view.Columns)
            {
                entity.IndexerProperty<object>(column.Name)
                    .HasColumnName(column.Name)
                    .HasColumnType(column.StoreType)
                    .IsRequired(false);
            }
        }
    }
}
