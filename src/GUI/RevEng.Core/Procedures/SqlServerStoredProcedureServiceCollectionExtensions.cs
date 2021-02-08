using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Functions;

namespace RevEng.Core.Procedures
{
    public static class SqlServerStoredProcedureServiceCollectionExtensions
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
            IOperationReporter reporter = null)
        {
            if (reporter == null)
            {
                reporter = new OperationReporter(handler: null);
            }

            return services
                .AddSingleton<IProcedureModelFactory, SqlServerDacpacStoredProcedureModelFactory>()
                .AddSingleton<IProcedureScaffolder, SqlServerStoredProcedureScaffolder>()
                .AddLogging(b => b.SetMinimumLevel(LogLevel.Debug).AddProvider(new OperationLoggerProvider(reporter)));
        }
    }
}
