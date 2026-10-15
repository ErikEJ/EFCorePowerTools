using System;
using System.Data.Common;
using System.IO;

namespace RevEng.Common;

public static class DbContextNamer
{
    public static string GetDatabaseName(string connectionString, DatabaseType dbType)
    {
        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        if (connectionString.EndsWith(".dacpac", System.StringComparison.OrdinalIgnoreCase))
        {
            return Path.GetFileNameWithoutExtension(connectionString);
        }

        var builder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString,
        };

        return builder.TryGetValue("Initial Catalog", out var catalog)
            ? catalog.ToString()
            : builder.TryGetValue("Database", out var database)
            ? database.ToString()
            : builder.TryGetValue("Data Source", out var dataSource)
            ? dataSource.ToString()
            : builder.TryGetValue("DataSource", out var dataSource2) ? dataSource2.ToString() : dbType.ToString();
    }
}