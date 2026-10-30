using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.SqlServer.Dac.Model;

namespace NUnitTestCore;

// Test-only prototype shared by offline scaffolding and live query tests.
// sql_variant selects EF's object mapping; it is not the actual SQL column type.
internal static class SystemViewObjectFallback
{
    private static readonly string MasterPath = Path.Combine(AppContext.BaseDirectory, "master.dacpac");

    public static DatabaseModel Create(string viewName)
    {
        using var master = new TSqlModel(MasterPath);
        var source = master.GetObjects(DacQueryScopes.UserDefined, View.TypeClass)
            .Single(view => view.Name.ToString() == $"[sys].[{viewName}]");
        var database = new DatabaseModel { DatabaseName = "SystemViews", DefaultSchema = "dbo" };
        var view = new DatabaseView { Name = viewName, Schema = "sys", Database = database };
        foreach (var column in source.GetReferenced(View.Columns, DacQueryScopes.All))
        {
            view.Columns.Add(new DatabaseColumn
            {
                Name = column.Name.Parts.Last(),
                Table = view,
                StoreType = "sql_variant",
                IsNullable = true,
            });
        }

        database.Tables.Add(view);
        return database;
    }

}