﻿using System;
using System.Collections.Generic;
using System.Linq;
using EFCore.Snowflake.Design.Internal;
using EntityFrameworkCore.Scaffolding.Handlebars;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using FirebirdSql.EntityFrameworkCore.Firebird.Design.Internal;
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
using Oracle.EntityFrameworkCore.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using RevEng.Common;
using RevEng.Core.Routines.Extensions;
using SimplerSoftware.EntityFrameworkCore.SqlServer.NodaTime.Design;

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

            if (options.CustomReplacers != null || options.UsePrefixNavigationNaming)
            {
                serviceCollection.AddSingleton<ICandidateNamingService>(provider => new ReplacingCandidateNamingService(options.UsePrefixNavigationNaming, options.CustomReplacers, options.PreserveCasingWithRegex));
            }

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

                case DatabaseType.Snowflake:
                    var snowflakeProvider = new SnowflakeDesignTimeServices();
                    snowflakeProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;

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
                serviceCollection.AddSingleton(options);
                serviceCollection.AddSingleton<IDatabaseModelFactory, PatchedSqlServerDatabaseModelFactory>();
                serviceCollection.AddSqlServerStoredProcedureDesignTimeServices();
                serviceCollection.AddSqlServerFunctionDesignTimeServices();
            }

            if (options.DatabaseType == DatabaseType.SQLServerDacpac)
            {
                var excludedIndexes = options.Tables?.Select(t => new { t.Name, t.ExcludedIndexes });

                serviceCollection.AddSingleton<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>(
                   serviceProvider => new SqlServerDacpacDatabaseModelFactory(
                       new SqlServerDacpacDatabaseModelFactoryOptions
                       {
                           MergeDacpacs = options.MergeDacpacs,
                           ExcludedIndexes = excludedIndexes?.ToDictionary(t => t.Name, t => t.ExcludedIndexes),
                       },
                       serviceProvider.GetService<IRelationalTypeMappingSource>()));

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

            if (options.UseNodaTime)
            {
                var nodaTime = new SqlServerNodaTimeDesignTimeServices();
                nodaTime.ConfigureDesignTimeServices(serviceCollection);
            }

            serviceCollection.AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>(
                provider => new SqlServerTypeMappingSource(
                    provider.GetService<TypeMappingSourceDependencies>(),
                    provider.GetService<RelationalTypeMappingSourceDependencies>(),
                    options.UseDateOnlyTimeOnly));
        }
    }
}