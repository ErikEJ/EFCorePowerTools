using EFCore.SqlCe.Design.Internal;
using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using ReverseEngineer20.ReverseEngineer;
using System;

namespace ReverseEngineer20
{
    public static class ServiceProviderBuilder
    {
        public static ServiceProvider Build(ReverseEngineerOptions options)
        {
            // Add base services for scaffolding
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<IOperationReporter, OperationReporter>()
                .AddSingleton<IOperationReportHandler, OperationReportHandler>();

            if (options.UseHandleBars)
            {
                //TODO Consider being selective based on SelectedToBeGenerated
                var selected = Microsoft.EntityFrameworkCore.Design.ReverseEngineerOptions.DbContextAndEntities;
                var language = (LanguageOptions)options.SelectedHandlebarsLanguage;
                serviceCollection.AddHandlebarsScaffolding(selected, language);
                serviceCollection.AddSingleton<ITemplateFileService>(provider => new CustomTemplateFileService(options.ProjectPath));
            }

            if (options.CustomReplacers != null)
            {
                serviceCollection.AddSingleton<ICandidateNamingService>(provider => new ReplacingCandidateNamingService(options.CustomReplacers));
            }

            if (options.UseInflector)
            {
                if (options.UseLegacyPluralizer)
                {
                    serviceCollection.AddSingleton<IPluralizer, LegacyInflectorPluralizer>();
                }
                else
                {
                    serviceCollection.AddSingleton<IPluralizer, InflectorPluralizer>();
                }
            }

            // Add database provider services
            switch (options.DatabaseType)
            {
                case DatabaseType.SQLCE35:
                    throw new NotImplementedException();
                case DatabaseType.SQLCE40:
                    var sqlCeProvider = new SqlCeDesignTimeServices();
                    sqlCeProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
                case DatabaseType.SQLServer:
                    var provider = new SqlServerDesignTimeServices();
                    provider.ConfigureDesignTimeServices(serviceCollection);

                    var spatial = new SqlServerNetTopologySuiteDesignTimeServices();
                    spatial.ConfigureDesignTimeServices(serviceCollection);

                    if (!string.IsNullOrEmpty(options.Dacpac))
                    {
                        serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>();
                    }
                    break;
                case DatabaseType.Npgsql:
                    var npgsqlProvider = new NpgsqlDesignTimeServices();
                    npgsqlProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
                case DatabaseType.Mysql:
                    var mysqlProvider = new MySqlDesignTimeServices();
                    mysqlProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
                case DatabaseType.SQLite:
                    var sqliteProvider = new SqliteDesignTimeServices();
                    sqliteProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
