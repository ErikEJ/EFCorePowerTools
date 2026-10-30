using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ErikEJ.EFCorePowerTools;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using GOEddie.Dacpac.References;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.SqlServer.Dac.Model;
using Xunit;

namespace NUnitTestCore;

// Characterization tests for the actual Microsoft system package, not synthetic reference metadata.
// These expose the limitations a future system-view scaffolding feature must address.
[Collection(nameof(CliExitCodeTests))]
public sealed class MasterDacpacIntegrationTest
{
    private static readonly string RootPath = Path.Combine(AppContext.BaseDirectory, "MasterReference.dacpac");
    private static readonly string MasterPath = Path.Combine(AppContext.BaseDirectory, "master.dacpac");

    [Fact]
    public void SqlProjectBuildProducesARealMasterReference()
    {
        var reference = Assert.Single(new HeaderParser(RootPath).GetCustomData(),
            data => data.Category == "Reference" && data.Type == "SqlSchema");

        Assert.Contains(reference.Items, item => item.Name == "LogicalName" && item.Value == "master.dacpac");
        Assert.Contains(reference.Items, item => item.Name == "ExternalParts" && item.Value == "[master]");
        Assert.True(File.Exists(MasterPath));
        using var model = new TSqlModel(RootPath);
        Assert.Equal(2, model.GetObjects(DacQueryScopes.UserDefined, Procedure.TypeClass).Count());
        Assert.Equal(RootPath, DacpacConsolidator.Consolidate(RootPath));
    }

    [Theory]
    [InlineData("tables")]
    [InlineData("databases")]
    [InlineData("master_files")]
    public void MasterViewDefinitionsHaveColumnNamesButNoUsableColumnTypes(string viewName)
    {
        using var model = new TSqlModel(MasterPath);
        var view = model.GetObjects(DacQueryScopes.UserDefined, View.TypeClass)
            .Single(item => item.Name.ToString() == $"[sys].[{viewName}]");
        var columns = view.GetReferenced(View.Columns, DacQueryScopes.All).ToList();

        Assert.NotEmpty(columns);
        Assert.Contains(columns, column => column.Name.Parts.Last() == "name");
        Assert.All(columns, column =>
        {
            Assert.Empty(column.GetReferencedRelationshipInstances(Column.DataType, DacExternalQueryScopes.All));
            Assert.Empty(column.GetReferencedRelationshipInstances(Column.ExpressionDependencies, DacExternalQueryScopes.All));
            Assert.Null(column.GetProperty(Column.Expression));
        });

        var database = new SqlServerDacpacDatabaseModelFactory().Create(MasterPath,
            new DatabaseModelFactoryOptions(tables: new[] { $"[sys].[{viewName}]" }));
        var scaffoldedView = Assert.Single(database.Tables);
        Assert.Equal(viewName, scaffoldedView.Name);
        Assert.Empty(scaffoldedView.Columns);
    }

    [Fact]
    public void ExplicitSelectionDoesNotIncludeViewsFromTheSkippedMasterReference()
    {
        var factory = new SqlServerDacpacDatabaseModelFactory(new SqlServerDacpacDatabaseModelFactoryOptions { MergeDacpacs = true });
        var database = factory.Create(RootPath,
            new DatabaseModelFactoryOptions(tables: new[] { "[sys].[tables]" }));

        Assert.Empty(database.Tables);
    }

    [Fact]
    public void SelectedSystemViewCannotBeReaddedByTheScriptBasedMerger()
    {
        using var master = new TSqlModel(MasterPath);
        var view = master.GetObjects(DacQueryScopes.UserDefined, View.TypeClass)
            .Single(item => item.Name.ToString() == "[sys].[tables]");
        Assert.True(view.TryGetAst(out var ast));

        using var target = new TSqlModel(master.Version, master.CopyModelOptions());
        // The definition scripts as CREATE VIEW [sys].[tables] AS ; without a SELECT body.
        // This is the same AddObjects operation used by DacpacMerger.
        Assert.Throws<DacModelException>(() => target.AddObjects(ast));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CliExplicitSystemViewSelectionCannotGenerateTypedProperties(bool useMasterDirectly)
    {
        var directory = Path.Combine(Path.GetTempPath(), "EFCorePowerTools.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            File.Copy(RootPath, Path.Combine(directory, "MasterReference.dacpac"));
            File.Copy(MasterPath, Path.Combine(directory, "master.dacpac"));
            var configPath = Path.Combine(directory, "efcpt-config.json");
            const string config = """
                {
                  "code-generation": { "merge-dacpacs": true, "refresh-object-lists": false },
                  "names": { "dbcontext-name": "SystemViewsContext", "root-namespace": "SystemViews" },
                  "tables": [],
                  "views": [{ "name": "[sys].[tables]" }],
                  "stored-procedures": [],
                  "functions": []
                }
                """;
            await File.WriteAllTextAsync(configPath, config, TestContext.Current.CancellationToken);
            var source = Path.Combine(directory, useMasterDirectly ? "master.dacpac" : "MasterReference.dacpac");
            Environment.ExitCode = 0;

            var exitCode = await Program.MainAsync(new[] { source, "mssql", "-i", configPath, "-o", directory })
                .WaitAsync(TimeSpan.FromSeconds(60), TestContext.Current.CancellationToken);

            Assert.Equal(0, exitCode);
            var context = await File.ReadAllTextAsync(Path.Combine(directory, "Models", "SystemViewsContext.cs"), TestContext.Current.CancellationToken);
            Assert.Equal(useMasterDirectly, context.Contains("ToView(\"tables\", \"sys\")", StringComparison.Ordinal));
            if (useMasterDirectly)
            {
                Assert.Contains("HasNoKey()", context, StringComparison.Ordinal);
                var entity = await File.ReadAllTextAsync(Path.Combine(directory, "Models", "Table.cs"), TestContext.Current.CancellationToken);
                Assert.Contains("public partial class Table", entity, StringComparison.Ordinal);
                Assert.DoesNotContain("{ get;", entity, StringComparison.Ordinal);
            }
            else
            {
                Assert.False(File.Exists(Path.Combine(directory, "Models", "Table.cs")));
            }

            Assert.Equal(config, await File.ReadAllTextAsync(configPath, TestContext.Current.CancellationToken));
            Assert.Empty(Directory.GetFiles(directory, "efpt-*.dacpac"));
        }
        finally
        {
            Environment.ExitCode = 0;
            Directory.Delete(directory, true);
        }
    }
}
