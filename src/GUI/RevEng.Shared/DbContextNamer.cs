using System;
using System.Data.Common;
using System.IO;

namespace RevEng.Common
{
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

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;

            if (builder.TryGetValue("Initial Catalog", out object catalog))
            {
                return catalog.ToString();
            }

            if (builder.TryGetValue("Database", out object database))
            {
                return database.ToString();
            }

            if (builder.TryGetValue("Data Source", out object dataSource))
            {
                return dataSource.ToString();
            }

            if (builder.TryGetValue("DataSource", out object dataSource2))
            {
                return dataSource2.ToString();
            }

            return dbType.ToString();
        }
    }
}