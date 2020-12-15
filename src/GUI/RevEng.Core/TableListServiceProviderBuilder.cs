using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
#if CORE50
#else
using Oracle.EntityFrameworkCore.Design.Internal;
#endif
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using RevEng.Core.Procedures;
using System;

namespace ReverseEngineer20.ReverseEngineer
{
    public static class TableListServiceProviderBuilder
    {
        public static ServiceProvider Build(DatabaseType databaseType)
        {
            // Add base services for scaffolding
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<IOperationReporter, OperationReporter>()
                .AddSingleton<IOperationReportHandler, OperationReportHandler>();

            // Add database provider services
            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    var provider = new SqlServerDesignTimeServices();
                    provider.ConfigureDesignTimeServices(serviceCollection);
                    serviceCollection.AddSqlServerStoredProcedureDesignTimeServices();
                    break;

                case DatabaseType.SQLServerDacpac:
                    var dacpacProvider = new SqlServerDesignTimeServices();
                    dacpacProvider.ConfigureDesignTimeServices(serviceCollection);
                    serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>();
                    serviceCollection.AddSqlServerDacpacStoredProcedureDesignTimeServices();
                    break;

                case DatabaseType.Npgsql:
                    var npgsqlProvider = new NpgsqlDesignTimeServices();
                    npgsqlProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;

                case DatabaseType.SQLite:
                    var sqliteProvider = new SqliteDesignTimeServices();
                    sqliteProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;

                case DatabaseType.Mysql:
                    var mysqlProvider = new MySqlDesignTimeServices();
                    mysqlProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
#if CORE50
#else
                case DatabaseType.Oracle:
                    var oracleProvider = new OracleDesignTimeServices();
                    oracleProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
#endif
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}