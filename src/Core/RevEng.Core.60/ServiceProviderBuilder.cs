using System;
using System.Collections.Generic;
using EntityFrameworkCore.Scaffolding.Handlebars;
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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using Oracle.EntityFrameworkCore.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using RevEng.Common;
using RevEng.Core.Procedures;
using SimplerSoftware.EntityFrameworkCore.SqlServer.NodaTime.Design;

#if !CORE80
using FirebirdSql.EntityFrameworkCore.Firebird.Design.Internal;
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

            var reporter = new OperationReporter(
                new OperationReportHandler(
                    m => errors.Add(m),
                    m => warnings.Add(m),
                    m => info.Add(m),
                    m => info.Add(m)));

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
            if (options.UseHandleBars)
            {
                serviceCollection.AddHandlebarsScaffolding(hbOptions =>
                {
                    hbOptions.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;
                    hbOptions.LanguageOptions = (LanguageOptions)options.SelectedHandlebarsLanguage;
                });
                serviceCollection.AddSingleton<ITemplateFileService>(provider
                    => new CustomTemplateFileService(options.OptionsPath));
            }

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
                    var provider = new SqlServerDesignTimeServices();
                    provider.ConfigureDesignTimeServices(serviceCollection);

                    if (options.DatabaseType == DatabaseType.SQLServer)
                    {
#if !CORE80
                        serviceCollection.AddSingleton<IDatabaseModelFactory, PatchedSqlServerDatabaseModelFactory>();
#endif
                        serviceCollection.AddSqlServerStoredProcedureDesignTimeServices();
                        serviceCollection.AddSqlServerFunctionDesignTimeServices();
                    }

                    if (options.DatabaseType == DatabaseType.SQLServerDacpac)
                    {
                        serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>(
                           provider => new SqlServerDacpacDatabaseModelFactory(new SqlServerDacpacDatabaseModelFactoryOptions
                           {
                               MergeDacpacs = options.MergeDacpacs,
                           }));

                        serviceCollection.AddSqlServerDacpacStoredProcedureDesignTimeServices(new SqlServerDacpacDatabaseModelFactoryOptions
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

                    if (options.UseNodaTime)
                    {
                        var nodaTime = new SqlServerNodaTimeDesignTimeServices();
                        nodaTime.ConfigureDesignTimeServices(serviceCollection);
                    }
#if CORE80
                    serviceCollection.AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>(
                        provider => new RevEng.Core.SqlServerTypeMappingSource(
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
#if !CORE80
                case DatabaseType.Firebird:
                    var firebirdProvider = new FbDesignTimeServices();
                    firebirdProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
#endif
                case DatabaseType.SQLite:
                    var sqliteProvider = new SqliteDesignTimeServices();
                    sqliteProvider.ConfigureDesignTimeServices(serviceCollection);

                    if (options.UseNodaTime)
                    {
                        var nodaTime = new SqliteNodaTimeDesignTimeServices();
                        nodaTime.ConfigureDesignTimeServices(serviceCollection);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(options), $"unsupported database type: {options.DatabaseType}");
            }

            serviceCollection.AddSingleton<IReverseEngineerScaffolder, ReverseEngineerScaffolder>();

            return serviceCollection;
        }
    }
}
