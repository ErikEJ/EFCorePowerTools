using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevEng.Core.Procedures.Model;
using RevEng.Core.Procedures.Scaffolding;

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
    }
}
