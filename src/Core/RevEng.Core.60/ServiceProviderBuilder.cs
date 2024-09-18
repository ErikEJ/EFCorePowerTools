using System;
using System.Collections.Generic;
#if !CORE90
using EntityFrameworkCore.Scaffolding.Handlebars;
using FirebirdSql.EntityFrameworkCore.Firebird.Design.Internal;
#endif
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Humanizer.Inflections;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
#if !CORE90
using Oracle.EntityFrameworkCore.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
#endif
using RevEng.Common;
using RevEng.Core.Routines.Extensions;
#if !CORE90
using SimplerSoftware.EntityFrameworkCore.SqlServer.NodaTime.Design;
#endif
#if !CORE80
using Microsoft.EntityFrameworkCore.SqlServer.Design;
#endif

namespace RevEng.Core
{
    public static class ServiceProviderBuilder
    {
        public static IServiceCollection AddEfpt(this IServiceCollection serviceCollection, ReverseEngineerCommandOptions options, List<string> errors, List<string> warnings, List<string> info)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(serviceCollection);
            ArgumentNullException.ThrowIfNull(warnings);
            ArgumentNullException.ThrowIfNull(errors);
            ArgumentNullException.ThrowIfNull(info);

            var reporter = new OperationReporter(
                new OperationReportHandler(
                    errors.Add,
                    warnings.Add,
                    info.Add,
                    info.Add));

            serviceCollection
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<IOperationReporter, OperationReporter>(provider =>
                    reporter)
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

#if CORE80
            if (options.CustomReplacers != null || options.UsePrefixNavigationNaming)
            {
                serviceCollection.AddSingleton<ICandidateNamingService>(provider => new ReplacingCandidateNamingService(options.UsePrefixNavigationNaming, options.CustomReplacers, options.PreserveCasingWithRegex));
            }
#else
            if (options.CustomReplacers != null)
            {
                serviceCollection.AddSingleton<ICandidateNamingService>(provider => new ReplacingCandidateNamingService(options.CustomReplacers, options.PreserveCasingWithRegex));
            }
#endif

#if !CORE90
            if (options.UseHandleBars)
            {
                serviceCollection.AddHandlebarsScaffolding(hbOptions =>
                {
                    hbOptions.ReverseEngineerOptions = Microsoft.EntityFrameworkCore.Design.ReverseEngineerOptions.DbContextAndEntities;
                    hbOptions.LanguageOptions = (LanguageOptions)options.SelectedHandlebarsLanguage;
                });
                serviceCollection.AddSingleton<ITemplateFileService>(provider
                    => new CustomTemplateFileService(options.OptionsPath));
            }
#endif
            if (options.UseInflector || options.UseLegacyPluralizer)
            {
                if (options.UseLegacyPluralizer)
                {
                    serviceCollection.AddSingleton<IPluralizer, LegacyPluralizer>();
                }
                else
                {
                    if (options.UncountableWords != null && options.UncountableWords.Count > 0)
                    {
                        options.UncountableWords.ForEach(w => Vocabularies.Default.AddUncountable(w));
                    }

                    serviceCollection.AddSingleton<IPluralizer, HumanizerPluralizer>();
                }
            }

            serviceCollection.AddNotImplementedDesignTimeServices();

            // Add database provider services
            switch (options.DatabaseType)
            {
                case DatabaseType.SQLServer:
                case DatabaseType.SQLServerDacpac:
                    AddSqlServerProviderServices(serviceCollection, options); break;

                case DatabaseType.Npgsql:
                    AddPostgresProviderServices(serviceCollection, options);

                    break;
#if !CORE90
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
#if !CORE90
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

            return serviceCollection;
        }

        private static void AddPostgresProviderServices(IServiceCollection serviceCollection, ReverseEngineerCommandOptions options)
        {
            var npgsqlProvider = new NpgsqlDesignTimeServices();
            npgsqlProvider.ConfigureDesignTimeServices(serviceCollection);

            serviceCollection.AddPostgresStoredProcedureDesignTimeServices();

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
        }

        private static void AddSqlServerProviderServices(IServiceCollection serviceCollection, ReverseEngineerCommandOptions options)
        {
            var provider = new SqlServerDesignTimeServices();
            provider.ConfigureDesignTimeServices(serviceCollection);

            if (options.DatabaseType == DatabaseType.SQLServer)
            {
                serviceCollection.AddSingleton<IDatabaseModelFactory, PatchedSqlServerDatabaseModelFactory>();
                serviceCollection.AddSqlServerStoredProcedureDesignTimeServices();
                serviceCollection.AddSqlServerFunctionDesignTimeServices();
            }

            if (options.DatabaseType == DatabaseType.SQLServerDacpac)
            {
                serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>(
                   serviceProvider => new SqlServerDacpacDatabaseModelFactory(
                       new SqlServerDacpacDatabaseModelFactoryOptions
                   {
                       MergeDacpacs = options.MergeDacpacs,
#if CORE80
                   },
                       serviceProvider.GetService<IRelationalTypeMappingSource>()));
#else
                   }));
#endif

                serviceCollection.AddSqlServerDacpacStoredProcedureDesignTimeServices(new SqlServerDacpacDatabaseModelFactoryOptions
                {
                    MergeDacpacs = options.MergeDacpacs,
                });

                serviceCollection.AddSqlServerDacpacFunctionDesignTimeServices(new SqlServerDacpacDatabaseModelFactoryOptions
                {
                    MergeDacpacs = options.MergeDacpacs,
                });
            }

            if (options.UseSpatial)
            {
                var spatial = new SqlServerNetTopologySuiteDesignTimeServices();
                spatial.ConfigureDesignTimeServices(serviceCollection);
            }

            if (options.UseHierarchyId)
            {
                var hierachyId = new SqlServerHierarchyIdDesignTimeServices();
                hierachyId.ConfigureDesignTimeServices(serviceCollection);
            }
#if !CORE90
            if (options.UseNodaTime)
            {
                var nodaTime = new SqlServerNodaTimeDesignTimeServices();
                nodaTime.ConfigureDesignTimeServices(serviceCollection);
            }
#endif
#if CORE80
            serviceCollection.AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>(
                provider => new SqlServerTypeMappingSource(
                    provider.GetService<TypeMappingSourceDependencies>(),
                    provider.GetService<RelationalTypeMappingSourceDependencies>(),
                    options.UseDateOnlyTimeOnly));
#endif
#if !CORE80
            if (options.UseDateOnlyTimeOnly)
            {
                var dateOnlyTimeOnly = new SqlServerDateOnlyTimeOnlyDesignTimeServices();
                dateOnlyTimeOnly.ConfigureDesignTimeServices(serviceCollection);
            }
#endif
        }
    }
}
