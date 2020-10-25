using EntityFrameworkCore.Scaffolding.Handlebars;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using RevEng.Core.Procedures;
using System;

namespace ReverseEngineer20.ReverseEngineer
{
    public static class ServiceProviderBuilder
    {
        public static ServiceProvider Build(ReverseEngineerCommandOptions options)
        {
            // Add base services for scaffolding
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<ICSharpEntityTypeGenerator, CommentCSharpEntityTypeGenerator>()
                .AddSingleton<IOperationReporter, OperationReporter>()
                .AddSingleton<IOperationReportHandler, OperationReportHandler>();

            if (options.CustomReplacers != null)
            {
                serviceCollection.AddSingleton<ICandidateNamingService>(provider => new ReplacingCandidateNamingService(options.CustomReplacers));
            }

            if (options.UseHandleBars)
            {
                //TODO Consider being selective based on SelectedToBeGenerated
                var selected = ReverseEngineerOptions.DbContextAndEntities;
                var language = (LanguageOptions)options.SelectedHandlebarsLanguage;
                serviceCollection.AddHandlebarsScaffolding(selected, language);
                serviceCollection.AddSingleton<ITemplateFileService>(provider => new CustomTemplateFileService(options.ProjectPath));
            }

            if (options.UseLegacyPluralizer)
            {
                serviceCollection.AddSingleton<IPluralizer, LegacyPluralizer>();
            }

            // Add database provider services
            switch (options.DatabaseType)
            {
                case DatabaseType.SQLServer:
                    var provider = new SqlServerDesignTimeServices();
                    provider.ConfigureDesignTimeServices(serviceCollection);

                    if (!string.IsNullOrEmpty(options.Dacpac))
                    {
                        serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>();
                    }

                    if (options.UseSpatial)
                    {
                        var spatial = new SqlServerNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }

                    if (string.IsNullOrEmpty(options.Dacpac))
                    {
                        serviceCollection.AddSqlServerStoredProcedureDesignTimeServices();
                    }

                    var builder = new SqlConnectionStringBuilder(options.ConnectionString)
                    {
                        CommandTimeout = 300
                    };
                    options.ConnectionString = builder.ConnectionString;

                    break;

                case DatabaseType.Npgsql:
                    var npgsqlProvider = new NpgsqlDesignTimeServices();
                    npgsqlProvider.ConfigureDesignTimeServices(serviceCollection);

                    if (options.UseNodaTime)
                    {
                        var nodaTime = new NpgsqlNodaTimeDesignTimeServices();
                        nodaTime.ConfigureDesignTimeServices(serviceCollection);
                    }

                    if (options.UseSpatial)
                    {
                        var spatial = new NpgsqlNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }

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