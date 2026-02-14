using System;
using System.Collections.Generic;
using System.Linq;
#if !CORE100
using EFCore.Snowflake.Design.Internal;
using EntityFrameworkCore.Scaffolding.Handlebars;
#endif
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
#if CORE100
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
#endif
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using Oracle.EntityFrameworkCore.Design.Internal;
#if !CORE100
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
#endif
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

#if !CORE100
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
                    if (options.IrregularWords != null && options.IrregularWords.Count > 0)
                    {
                        options.IrregularWords.ForEach(w => Vocabularies.Default.AddIrregular(w.Singular, w.Plural, w.MatchEnding));
                    }

                    if (options.UncountableWords != null && options.UncountableWords.Count > 0)
                    {
                        options.UncountableWords.ForEach(w => Vocabularies.Default.AddUncountable(w));
                    }

                    if (options.PluralRules != null && options.PluralRules.Count > 0)
                    {
                        options.PluralRules.ForEach(w => Vocabularies.Default.AddPlural(w.Rule, w.Replacement));
                    }

                    if (options.SingularRules != null && options.SingularRules.Count > 0)
                    {
                        options.SingularRules.ForEach(w => Vocabularies.Default.AddSingular(w.Rule, w.Replacement));
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
                    AddSqlServerProviderServices(serviceCollection, options);
                    break;

                case DatabaseType.Npgsql:
                    AddPostgresProviderServices(serviceCollection, options);
                    break;

                case DatabaseType.Oracle:
                    var oracleProvider = new OracleDesignTimeServices();
                    oracleProvider.ConfigureDesignTimeServices(serviceCollection);

#if CORE100
                    if (options.UseSpatial)
                    {
                        var spatial = new OracleNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }
#endif
                    break;

#if !CORE100
                case DatabaseType.Mysql:
                    var mysqlProvider = new MySqlDesignTimeServices();
                    mysqlProvider.ConfigureDesignTimeServices(serviceCollection);

                    if (options.UseSpatial)
                    {
                        var spatial = new MySqlNetTopologySuiteDesignTimeServices();
                        spatial.ConfigureDesignTimeServices(serviceCollection);
                    }

                    break;

                case DatabaseType.Firebird:
                    var firebirdProvider = new FbDesignTimeServices();
                    firebirdProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;

                case DatabaseType.Snowflake:
                    var snowflakeProvider = new SnowflakeDesignTimeServices();
                    snowflakeProvider.ConfigureDesignTimeServices(serviceCollection);
                    break;
#endif

                case DatabaseType.SQLite:
                    var sqliteProvider = new SqliteDesignTimeServices();
                    sqliteProvider.ConfigureDesignTimeServices(serviceCollection);

#if !CORE100
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

#if CORE100
            serviceCollection.AddSingleton(new SqlServerSingletonOptions());
#endif

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

#if !CORE100
            serviceCollection.AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>(
                provider => new SqlServerTypeMappingSource(
                    provider.GetService<TypeMappingSourceDependencies>(),
                    provider.GetService<RelationalTypeMappingSourceDependencies>(),
                    options.UseDateOnlyTimeOnly));
#else
            serviceCollection.AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>(
                provider => new SqlServerTypeMappingSource(
                    provider.GetService<TypeMappingSourceDependencies>(),
                    provider.GetService<RelationalTypeMappingSourceDependencies>(),
                    provider.GetService<SqlServerSingletonOptions>(),
                    options.UseDateOnlyTimeOnly));
#endif
        }
    }
}