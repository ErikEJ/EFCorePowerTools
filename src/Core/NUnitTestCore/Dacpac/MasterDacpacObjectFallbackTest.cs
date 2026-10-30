using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Dac.Model;
using RevEng.Common;
using RevEng.Core;
using Xunit;

namespace NUnitTestCore;

// Feasibility prototype only: sql_variant selects EF's object mapping, not the real SQL column type.
// No column types are guessed, and no server metadata is used to construct the model.
[Collection(nameof(CliExitCodeTests))]
public sealed class MasterDacpacObjectFallbackTest
{
    private static readonly string MasterPath = Path.Combine(AppContext.BaseDirectory, "master.dacpac");

    [Fact]
    public void TypedSystemTableIsAPositiveControlForDataTypeLookup()
    {
        using var master = new TSqlModel(MasterPath);
        var table = master.GetObjects(DacQueryScopes.UserDefined, Table.TypeClass)
            .Single(item => item.Name.ToString() == "[sys].[trace_xe_action_map]");
        var column = table.GetReferenced(Table.Columns, DacQueryScopes.All)
            .Single(item => item.Name.Parts.Last() == "trace_column_id");

        var type = Assert.Single(column.GetReferencedRelationshipInstances(Column.DataType, DacExternalQueryScopes.All));
        Assert.Equal("[smallint]", type.ObjectName.ToString());
        Assert.False(column.GetProperty<bool>(Column.Nullable));
    }

    [Fact]
    public void SystemViewNamesCanGenerateQueryOnlyObjectProperties()
    {
        var database = SystemViewObjectFallback.Create("tables");
        using var services = new ServiceCollection().AddEfpt(new ReverseEngineerCommandOptions
        {
            DatabaseType = DatabaseType.SQLServerDacpac,
            Tables = new List<SerializationTableModel>(),
        }, new List<string>(), new List<string>(), new List<string>()).BuildServiceProvider();
        var model = services.GetRequiredService<IScaffoldingModelFactory>().Create(database, new ModelReverseEngineerOptions());
        var entity = Assert.Single(model.GetEntityTypes());

        Assert.Null(entity.FindPrimaryKey());
        Assert.Equal("tables", entity.GetViewName());
        Assert.Equal("sys", entity.GetViewSchema());
        Assert.Equal(database.Tables.Single().Columns.Count, entity.GetProperties().Count());
        Assert.All(entity.GetProperties(), property => Assert.Equal(typeof(object), property.ClrType));

        var options = new ModelCodeGenerationOptions
        {
            ContextName = "SystemViewsContext",
            ModelNamespace = "SystemViews",
            ContextNamespace = "SystemViews",
            Language = "C#",
            SuppressOnConfiguring = true,
            UseNullableReferenceTypes = true,
            ConnectionString = "unused",
        };
        var generated = services.GetRequiredService<IModelCodeGeneratorSelector>().Select(options).GenerateModel(model, options);
        var source = Assert.Single(generated.AdditionalFiles).Code;
        Assert.Contains("public object? Name", source, StringComparison.Ordinal);
        Assert.Contains("public object? ObjectId", source, StringComparison.Ordinal);
        Assert.Contains("HasNoKey()", generated.ContextFile.Code, StringComparison.Ordinal);
        Assert.Contains("ToView(\"tables\", \"sys\")", generated.ContextFile.Code, StringComparison.Ordinal);
        Assert.Contains("HasColumnType(\"sql_variant\")", generated.ContextFile.Code, StringComparison.Ordinal);
    }

}
