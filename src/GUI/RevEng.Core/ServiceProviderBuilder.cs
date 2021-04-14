using EntityFrameworkCore.Scaffolding.Handlebars;
using ErikEJ.EntityFrameworkCore.SqlServer.Edmx.Scaffolding;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using FirebirdSql.EntityFrameworkCore.Firebird.Design.Internal;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using Oracle.EntityFrameworkCore.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using RevEng.Core.Procedures;
using RevEng.Shared;
#if CORE50
using SimplerSoftware.EntityFrameworkCore.SqlServer.NodaTime.Design;
#endif
using System;

namespace RevEng.Core
{
    public static class ServiceProviderBuilder
    {
        public static ServiceProvider Build(ReverseEngineerCommandOptions options)
        {
            // Add base services for scaffolding
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddEntityFrameworkDesignTimeServices()
#if CORE50
                .AddSingleton<ICSharpEntityTypeGenerator>(provider =>
                 new CommentCSharpEntityTypeGenerator(                    
                    provider.GetService<IAnnotationCodeGenerator>(),
                    provider.GetService<ICSharpHelper>(),
                    options.UseNullableReferences,
                    options.UseNoConstructor))
#else
                .AddSingleton<ICSharpEntityTypeGenerator>(provider =>
                 new CommentCSharpEntityTypeGenerator(
                    provider.GetService<ICSharpHelper>(),
                    options.UseNullableReferences,
                    options.UseNoConstructor))
#endif
                .AddSingleton<IOperationReporter, OperationReporter>()
                .AddSingleton<IOperationReportHandler, OperationReportHandler>()
                .AddSingleton<IScaffoldingModelFactory>(provider =>
                new ColumnRemovingScaffoldingModelFactory(
                    provider.GetService<IOperationReporter>(),
                    provider.GetService<ICandidateNamingService>(),
                    provider.GetService<IPluralizer>(),
                    provider.GetService<ICSharpUtilities>(),
                    provider.GetService<IScaffoldingTypeMapper>(),
                    provider.GetService<LoggingDefinitions>(),
                    options.Tables,
                    options.DatabaseType
                ));

            if (options.CustomReplacers != null)
            {
                serviceCollection.AddSingleton<ICandidateNamingService>(provider => new ReplacingCandidateNamingService(options.CustomReplacers));
            }

            if (options.UseHandleBars)
            {
                serviceCollection.AddHandlebarsScaffolding(hbOptions =>
                {
                    hbOptions.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;
                    hbOptions.LanguageOptions = (LanguageOptions)options.SelectedHandlebarsLanguage;
                });
                serviceCollection.AddSingleton<ITemplateFileService>(provider => new CustomTemplateFileService(options.ProjectPath));
            }

            if (options.UseInflector || options.UseLegacyPluralizer)
            {
                if (options.UseLegacyPluralizer)
                {
                    serviceCollection.AddSingleton<IPluralizer, LegacyPluralizer>();
                }
#if CORE50
#else
                else
                {
                    serviceCollection.AddSingleton<IPluralizer, HumanizerPluralizer>();
                }
#endif
            }

            // Add database provider services
            switch (options.DatabaseType)
            {
                case DatabaseType.Edmx:
                    var edmxProvider = new SqlServerDesignTimeServices();
                    edmxProvider.ConfigureDesignTimeServices(serviceCollection);

                    serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerEdmxDatabaseModelFactory>();

                    break;
                case DatabaseType.SQLServer:
                    var provider = new SqlServerDesignTimeServices();
                    provider.ConfigureDesignTimeServices(serviceCollection);

                    serviceCollection.AddSqlServerStoredProcedureDesignTimeServices();
                    serviceCollection.AddSqlServerFunctionDesignTimeServices();

                    if (options.UseSpatial)
                    {
                        var spatial = new SqlServerNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }
#if CORE50
                    if (options.UseNodaTime)
                    {
                        var nodaTime = new SqlServerNodaTimeDesignTimeServices();
                        nodaTime.ConfigureDesignTimeServices(serviceCollection);
                    }
#endif
                    break;

                case DatabaseType.SQLServerDacpac:
                    var dacProvider = new SqlServerDesignTimeServices();
                    dacProvider.ConfigureDesignTimeServices(serviceCollection);

                    serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>();
                    serviceCollection.AddSqlServerDacpacStoredProcedureDesignTimeServices();

                    if (options.UseSpatial)
                    {
                        var spatial = new SqlServerNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }

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

                case DatabaseType.Mysql:
                    var mysqlProvider = new MySqlDesignTimeServices();
                    mysqlProvider.ConfigureDesignTimeServices(serviceCollection);

                    if (options.UseSpatial)
                    {
                        var spatial = new MySqlNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }

                    break;

                case DatabaseType.Oracle:
                    var oracleProvider = new OracleDesignTimeServices();
                    oracleProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;

                case DatabaseType.Firebird:
                    var firebirdProvider = new FbDesignTimeServices();
                    firebirdProvider.ConfigureDesignTimeServices(serviceCollection);
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