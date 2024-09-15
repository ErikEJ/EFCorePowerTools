using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Routines.Functions;
using RevEng.Core.Routines.Procedures;

namespace RevEng.Core.Routines.Extensions
{
    public static class SqlServerRoutineServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerStoredProcedureDesignTimeServices(
            this IServiceCollection services,
            IOperationReporter reporter = null)
        {
            if (reporter == null)
            {
                reporter = new OperationReporter(handler: null);
            }

            return services
                .AddSingleton<IClrTypeMapper, SqlServerClrTypeMapper>()
                .AddSingleton<IProcedureModelFactory, SqlServerStoredProcedureModelFactory>()
                .AddSingleton<IProcedureScaffolder, SqlServerStoredProcedureScaffolder>()
                .AddLogging(b => b.SetMinimumLevel(LogLevel.Debug).AddProvider(new OperationLoggerProvider(reporter)));
        }

        public static IServiceCollection AddSqlServerFunctionDesignTimeServices(
            this IServiceCollection services,
            IOperationReporter reporter = null)
        {
            if (reporter == null)
            {
                reporter = new OperationReporter(handler: null);
            }

            return services
                .AddSingleton<IFunctionModelFactory, SqlServerFunctionModelFactory>()
                .AddSingleton<IFunctionScaffolder, SqlServerFunctionScaffolder>()
                .AddLogging(b => b.SetMinimumLevel(LogLevel.Debug).AddProvider(new OperationLoggerProvider(reporter)));
        }

        public static IServiceCollection AddSqlServerDacpacStoredProcedureDesignTimeServices(
            this IServiceCollection services,
            SqlServerDacpacDatabaseModelFactoryOptions factoryOptions,
            IOperationReporter reporter = null)
        {
            if (reporter == null)
            {
                reporter = new OperationReporter(handler: null);
            }

            return services
                .AddSingleton<IClrTypeMapper, SqlServerClrTypeMapper>()
                .AddSingleton<IProcedureModelFactory, SqlServerDacpacStoredProcedureModelFactory>(
                    provider => new SqlServerDacpacStoredProcedureModelFactory(factoryOptions))
                .AddSingleton<IProcedureScaffolder, SqlServerStoredProcedureScaffolder>()
                .AddLogging(b => b.SetMinimumLevel(LogLevel.Debug).AddProvider(new OperationLoggerProvider(reporter)));
        }

        public static IServiceCollection AddSqlServerDacpacFunctionDesignTimeServices(
            this IServiceCollection services,
            SqlServerDacpacDatabaseModelFactoryOptions factoryOptions,
            IOperationReporter reporter = null)
        {
            if (reporter == null)
            {
                reporter = new OperationReporter(handler: null);
            }

            return services
                .AddSingleton<IFunctionModelFactory, SqlServerDacpacFunctionModelFactory>(
                    provider => new SqlServerDacpacFunctionModelFactory(factoryOptions))
                .AddSingleton<IFunctionScaffolder, SqlServerFunctionScaffolder>()
                .AddLogging(b => b.SetMinimumLevel(LogLevel.Debug).AddProvider(new OperationLoggerProvider(reporter)));
        }
    }
}
