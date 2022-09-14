#if !CORE70
using EntityFrameworkCore.Scaffolding.Handlebars;
using FirebirdSql.EntityFrameworkCore.Firebird.Design.Internal;
using Oracle.EntityFrameworkCore.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
#endif
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using RevEng.Common;
using RevEng.Core.Procedures;
#if (CORE50 || CORE60) && !CORE70
using SimplerSoftware.EntityFrameworkCore.SqlServer.NodaTime.Design;
#endif
using System;

namespace RevEng.Core
{
    public static class ServiceProviderBuilder
    {
        public static ServiceProvider Build(ReverseEngineerCommandOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

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
#elif CORE60
#else
                .AddSingleton<ICSharpEntityTypeGenerator>(provider =>
                 new CommentCSharpEntityTypeGenerator(
                    provider.GetService<ICSharpHelper>(),
                    options.UseNullableReferences,
                    options.UseNoConstructor))
#endif
                .AddSingleton<IOperationReporter, OperationReporter>()
                .AddSingleton<IOperationReportHandler, OperationReportHandler>()
#if CORE60

                .AddSingleton<IScaffoldingModelFactory>(provider =>
                new ColumnRemovingScaffoldingModelFactory(
                    provider.GetService<IOperationReporter>(),
                    provider.GetService<ICandidateNamingService>(),
                    provider.GetService<IPluralizer>(),
                    provider.GetService<ICSharpUtilities>(),
                    provider.GetService<IScaffoldingTypeMapper>(),
                    provider.GetService<LoggingDefinitions>(),
                    provider.GetService<IModelRuntimeInitializer>(),
                    options.Tables,
                    options.DatabaseType,
                    options.UseManyToManyEntity));
#else
                .AddSingleton<IScaffoldingModelFactory>(provider =>
                new ColumnRemovingScaffoldingModelFactory(
                    provider.GetService<IOperationReporter>(),
                    provider.GetService<ICandidateNamingService>(),
                    provider.GetService<IPluralizer>(),
                    provider.GetService<ICSharpUtilities>(),
                    provider.GetService<IScaffoldingTypeMapper>(),
                    provider.GetService<LoggingDefinitions>(),
                    options.Tables,
                    options.DatabaseType));
#endif
            if (options.CustomReplacers != null)
            {
                serviceCollection.AddSingleton<ICandidateNamingService>(provider => new ReplacingCandidateNamingService(options.CustomReplacers, options.PreserveCasingWithRegex));
            }

#if !CORE70
            if (options.UseHandleBars)
            {
                serviceCollection.AddHandlebarsScaffolding(hbOptions =>
                {
                    hbOptions.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;
                    hbOptions.LanguageOptions = (LanguageOptions)options.SelectedHandlebarsLanguage;
#if CORE60
#else
                    hbOptions.EnableNullableReferenceTypes = options.UseNullableReferences;
#endif
                });
                serviceCollection.AddSingleton<ITemplateFileService>(provider
                    => new CustomTemplateFileService(options.OptionsPath));
            }
#endif

#if CORE50 || CORE60
            if (options.UseLegacyPluralizer)
            {
                serviceCollection.AddSingleton<IPluralizer, LegacyPluralizer>();
            }
#else
            if (options.UseInflector || options.UseLegacyPluralizer)
            {
                if (options.UseLegacyPluralizer)
                {
                    serviceCollection.AddSingleton<IPluralizer, LegacyPluralizer>();
                }
                else
                {
                    serviceCollection.AddSingleton<IPluralizer, HumanizerPluralizer>();
                }
            }
#endif

            serviceCollection.AddNotImplementedDesignTimeServices();

            // Add database provider services
            switch (options.DatabaseType)
            {
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

#if (CORE31 || CORE60)  && !CORE70
                    var builder = new SqlConnectionStringBuilder(options.ConnectionString);
                    if (builder.DataSource.EndsWith(".dynamics.com", StringComparison.Ordinal))
                    {
                        serviceCollection.AddSingleton<IDatabaseModelFactory, CrmDatabaseModelFactory>();
                    }
#endif
#if !CORE70
                    if (options.UseHierarchyId)
                    {
                        var hierachyId = new SqlServerHierarchyIdDesignTimeServices();
                        hierachyId.ConfigureDesignTimeServices(serviceCollection);
                    }
#endif

#if (CORE50 || CORE60) && !CORE70
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

                    serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>(
                        provider => new SqlServerDacpacDatabaseModelFactory(new SqlServerDacpacDatabaseModelFactoryOptions
                        {
                            MergeDacpacs = options.MergeDacpacs,
                        }));

                    serviceCollection.AddSqlServerDacpacStoredProcedureDesignTimeServices(new SqlServerDacpacDatabaseModelFactoryOptions
                    {
                        MergeDacpacs = options.MergeDacpacs,
                    });

                    if (options.UseSpatial)
                    {
                        var spatial = new SqlServerNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }
#if !CORE70
                    if (options.UseHierarchyId)
                    {
                        var hierachyId = new SqlServerHierarchyIdDesignTimeServices();
                        hierachyId.ConfigureDesignTimeServices(serviceCollection);
                    }
#endif
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

#if !CORE70

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
#endif

                case DatabaseType.SQLite:
                    var sqliteProvider = new SqliteDesignTimeServices();
                    sqliteProvider.ConfigureDesignTimeServices(serviceCollection);

#if (CORE50 || CORE60) && !CORE70
                    if (options.UseNodaTime)
                    {
                        var nodaTime = new SqliteNodaTimeDesignTimeServices();
                        nodaTime.ConfigureDesignTimeServices(serviceCollection);
                    }
#endif

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(options), $"unsupported database type: {options.DatabaseType}");
            }

            serviceCollection.AddSingleton<IReverseEngineerScaffolder, ReverseEngineerScaffolder>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
