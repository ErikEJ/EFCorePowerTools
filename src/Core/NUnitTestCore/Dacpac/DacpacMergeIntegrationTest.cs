using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using ErikEJ.EFCorePowerTools;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using GOEddie.Dacpac.References;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using RevEng.Common;
using RevEng.Core;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Model;
using Xunit;

namespace NUnitTestCore;

[Collection(nameof(CliExitCodeTests))]
public sealed class DacpacMergeIntegrationTest : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "EFCorePowerTools.Tests", Guid.NewGuid().ToString("N"));

    public DacpacMergeIntegrationTest()
    {
        Directory.CreateDirectory(directory);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MergeControlsReferencedTablesViewsAndRoutines(bool merge)
    {
        var root = BuildGraph();
        var options = new SqlServerDacpacDatabaseModelFactoryOptions { MergeDacpacs = merge };
        var database = new SqlServerDacpacDatabaseModelFactory(options).Create(root, new DatabaseModelFactoryOptions());
        var modules = new ModuleModelFactoryOptions { Modules = Array.Empty<string>(), FullModel = true };
        var procedures = new SqlServerDacpacStoredProcedureModelFactory(options).Create(root, modules);
        var functions = new SqlServerDacpacFunctionModelFactory(options).Create(root, modules);

        Assert.Equal("Root", database.DatabaseName);
        Assert.Equal(merge ? new[] { "Child", "ChildView", "Leaf", "Root" } : new[] { "Root" }, database.Tables.Select(t => t.Name).OrderBy(n => n));
        Assert.Equal(merge ? new[] { "GetChild" } : Array.Empty<string>(), procedures.Routines.Select(r => r.Name));
        Assert.Equal(merge ? new[] { "GetLeaf", "GetValue" } : Array.Empty<string>(), functions.Routines.Select(r => r.Name).OrderBy(n => n));
        Assert.Empty(procedures.Errors);
        Assert.Empty(functions.Errors);
        if (merge)
        {
            var child = database.Tables.Single(t => t.Name == "Child");
            Assert.Equal("Id", Assert.Single(child.PrimaryKey.Columns).Name);
            Assert.Equal("int", Assert.Single(child.Columns).StoreType);
            Assert.Equal("int", Assert.Single(procedures.Routines).Results.Single().Single().StoreType);
            Assert.Equal("Id", functions.Routines.Single(r => r.Name == "GetLeaf").Results.Single().Single().Name);
        }

        AssertNoTemporaryDacpacs();
        Assert.True(File.Exists(root));
    }

    [Fact]
    public void ConsolidatedPackageContainsTransitiveObjectsOnlyOnce()
    {
        var root = BuildGraph();
        var merged = DacpacConsolidator.Consolidate(root);
        try
        {
            Assert.NotEqual(root, merged);
            using var model = new TSqlModel(merged);
            Assert.Equal(new[] { "[dbo].[Child]", "[dbo].[Leaf]", "[dbo].[Root]" }, model.GetObjects(DacQueryScopes.UserDefined, Table.TypeClass).Select(t => t.Name.ToString()).OrderBy(n => n));
            Assert.Single(model.GetObjects(DacQueryScopes.UserDefined, View.TypeClass));
            Assert.Single(model.GetObjects(DacQueryScopes.UserDefined, Procedure.TypeClass));
            Assert.Single(model.GetObjects(DacQueryScopes.UserDefined, ScalarFunction.TypeClass));
            Assert.Single(model.GetObjects(DacQueryScopes.UserDefined, TableValuedFunction.TypeClass));
        }
        finally
        {
            File.Delete(merged);
        }
    }

    [Fact]
    public void MissingDirectAndTransitiveReferencesWarnButMasterDoesNot()
    {
        var root = BuildGraph();
        AddReferences(root, "Missing.dacpac", "master.dacpac");
        AddReferences(Path.Combine(directory, "nested", "Child.dacpac"), "MissingLeaf.dacpac");
        var warnings = new List<string>();
        var database = new SqlServerDacpacDatabaseModelFactory(new SqlServerDacpacDatabaseModelFactoryOptions
        {
            MergeDacpacs = true,
            ReportWarning = warnings.Add,
        }).Create(root, new DatabaseModelFactoryOptions());

        Assert.Equal(4, database.Tables.Count);
        Assert.Equal(2, warnings.Count);
        Assert.Contains(warnings, w => w.Contains(Path.Combine(directory, "Missing.dacpac"), StringComparison.Ordinal));
        Assert.Contains(warnings, w => w.Contains(Path.Combine(directory, "nested", "MissingLeaf.dacpac"), StringComparison.Ordinal));
        AssertNoTemporaryDacpacs();
    }

    [Fact]
    public void WarningIsReportedOnceAcrossAllRegisteredFactories()
    {
        var root = BuildGraph();
        AddReferences(root, "Missing.dacpac");
        var warnings = new List<string>();
        using var services = new ServiceCollection().AddEfpt(new ReverseEngineerCommandOptions
        {
            DatabaseType = DatabaseType.SQLServerDacpac,
            MergeDacpacs = true,
        }, new List<string>(), warnings, new List<string>()).BuildServiceProvider();
        var modules = new ModuleModelFactoryOptions { Modules = Array.Empty<string>() };

        services.GetRequiredService<IDatabaseModelFactory>().Create(root, new DatabaseModelFactoryOptions());
        services.GetRequiredService<IProcedureModelFactory>().Create(root, modules);
        services.GetRequiredService<IFunctionModelFactory>().Create(root, modules);

        Assert.Contains("Missing.dacpac", Assert.Single(warnings), StringComparison.Ordinal);
        AssertNoTemporaryDacpacs();
    }

    [Fact]
    public void MissingReferencesDoNotCauseTheSourcePackageToBeDeleted()
    {
        var root = Build("Root.dacpac", "CREATE TABLE dbo.Root (Id int NOT NULL PRIMARY KEY);");
        AddReferences(root, "Missing.dacpac", "master.dacpac");
        var original = File.ReadAllBytes(root);
        var warnings = new List<string>();
        var options = new SqlServerDacpacDatabaseModelFactoryOptions { MergeDacpacs = true, ReportWarning = warnings.Add };
        var database = new SqlServerDacpacDatabaseModelFactory(options).Create(root, new DatabaseModelFactoryOptions());

        Assert.Equal("Root", Assert.Single(database.Tables).Name);
        Assert.Single(warnings);
        Assert.Equal(original, File.ReadAllBytes(root));
        AssertNoTemporaryDacpacs();
    }

    [Fact]
    public void CyclicReferencesAreMergedOnce()
    {
        var root = BuildGraph();
        AddReferences(Path.Combine(directory, "Leaf.dacpac"), "Root.dacpac");
        var database = new SqlServerDacpacDatabaseModelFactory(new SqlServerDacpacDatabaseModelFactoryOptions { MergeDacpacs = true })
            .Create(root, new DatabaseModelFactoryOptions());

        Assert.Equal(4, database.Tables.Count);
        AssertNoTemporaryDacpacs();
    }

    [Fact]
    public void TemporaryPackageIsDeletedWhenModelReadingFails()
    {
        var root = BuildGraph();
        var options = new SqlServerDacpacDatabaseModelFactoryOptions { MergeDacpacs = true };
        // A null module list fails after consolidation and model loading.
        Assert.Throws<ArgumentNullException>(() => new SqlServerDacpacStoredProcedureModelFactory(options).Create(root, new ModuleModelFactoryOptions()));
        AssertNoTemporaryDacpacs();
        Assert.Throws<ArgumentNullException>(() => new SqlServerDacpacFunctionModelFactory(options).Create(root, new ModuleModelFactoryOptions()));
        AssertNoTemporaryDacpacs();
        Assert.True(File.Exists(root));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CliGeneratesReferencedEntitiesOnlyWhenEnabled(bool merge)
    {
        var root = BuildGraph();
        var config = Path.Combine(directory, "efcpt-config.json");
        await File.WriteAllTextAsync(config, $$"""
            {
              "code-generation": { "merge-dacpacs": {{merge.ToString().ToLowerInvariant()}}, "refresh-object-lists": true },
              "names": { "dbcontext-name": "MergeContext", "root-namespace": "MergeTests" }
            }
            """, TestContext.Current.CancellationToken);
        Environment.ExitCode = 0;
        var exitCode = await Program.MainAsync(new[] { root, "mssql", "-i", config, "-o", directory });

        Assert.Equal(0, exitCode);
        var generated = string.Join(Environment.NewLine, Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories).Select(File.ReadAllText));
        Assert.Contains("ToTable(\"Root\")", generated, StringComparison.Ordinal);
        Assert.Equal(merge, generated.Contains("ToTable(\"Child\")", StringComparison.Ordinal));
        Assert.Equal(merge, generated.Contains("ToTable(\"Leaf\")", StringComparison.Ordinal));
        Assert.Equal(merge, generated.Contains("ToView(\"ChildView\")", StringComparison.Ordinal));
        AssertNoTemporaryDacpacs();
    }

    public void Dispose()
    {
        Environment.ExitCode = 0;
        Directory.Delete(directory, true);
    }

    private string BuildGraph()
    {
        Build("Leaf.dacpac", "CREATE TABLE dbo.Leaf (Id int NOT NULL PRIMARY KEY);", "CREATE FUNCTION dbo.GetLeaf() RETURNS TABLE AS RETURN (SELECT Id FROM dbo.Leaf);");
        var child = Build("nested/Child.dacpac", "CREATE TABLE dbo.Child (Id int NOT NULL PRIMARY KEY);", "CREATE VIEW dbo.ChildView AS SELECT Id FROM dbo.Child;", "CREATE PROCEDURE dbo.GetChild AS SELECT Id FROM dbo.Child;", "CREATE FUNCTION dbo.GetValue() RETURNS int AS BEGIN RETURN 1; END;");
        AddReferences(child, "../Leaf.dacpac");
        var root = Build("Root.dacpac", "CREATE TABLE dbo.Root (Id int NOT NULL PRIMARY KEY);");
        // The diamond verifies that a shared transitive dependency is merged only once.
        AddReferences(root, "nested/Child.dacpac", "Leaf.dacpac");
        return root;
    }

    private string Build(string name, params string[] scripts)
    {
        var path = Path.Combine(directory, name);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using var model = new TSqlModel(SqlServerVersion.Sql160, new TSqlModelOptions());
        foreach (var script in scripts)
        {
            model.AddObjects(script);
        }

        DacPackageExtensions.BuildPackage(path, model, new PackageMetadata { Name = Path.GetFileNameWithoutExtension(path) });
        return path;
    }

    private static void AddReferences(string path, params string[] references)
    {
        // BuildPackage creates real DacFx models; add the reference metadata emitted by SQL project builds.
        using var package = Package.Open(path, FileMode.Open, FileAccess.ReadWrite);
        var part = package.GetPart(new Uri("/model.xml", UriKind.Relative));
        XDocument document;
        using (var stream = part.GetStream(FileMode.Open, FileAccess.Read))
        {
            document = XDocument.Load(stream);
        }

        var ns = document.Root.Name.Namespace;
        var header = document.Root.Element(ns + "Header");
        if (header == null)
        {
            header = new XElement(ns + "Header");
            document.Root.AddFirst(header);
        }

        foreach (var reference in references)
        {
            header.Add(new XElement(ns + "CustomData", new XAttribute("Category", "Reference"), new XAttribute("Type", "SqlSchema"),
                new XElement(ns + "Metadata", new XAttribute("Name", "FileName"), new XAttribute("Value", reference)),
                new XElement(ns + "Metadata", new XAttribute("Name", "LogicalName"), new XAttribute("Value", reference)),
                new XElement(ns + "Metadata", new XAttribute("Name", "SuppressMissingDependenciesErrors"), new XAttribute("Value", "True"))));
        }

        using (var output = part.GetStream(FileMode.Create, FileAccess.Write))
        {
            document.Save(output);
        }

        var originPart = package.GetPart(new Uri("/Origin.xml", UriKind.Relative));
        XDocument origin;
        using (var stream = originPart.GetStream(FileMode.Open, FileAccess.Read))
        {
            origin = XDocument.Load(stream);
        }

        using (var stream = part.GetStream(FileMode.Open, FileAccess.Read))
        {
            origin.Descendants(ns + "Checksum").Single(c => (string)c.Attribute("Uri") == "/model.xml").Value = Convert.ToHexString(SHA256.HashData(stream));
        }

        using var originOutput = originPart.GetStream(FileMode.Create, FileAccess.Write);
        origin.Save(originOutput);
    }

    private void AssertNoTemporaryDacpacs()
        => Assert.Empty(Directory.GetFiles(directory, "efpt-*.dacpac", SearchOption.AllDirectories));
}
