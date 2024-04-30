using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Routines.Procedures;

namespace RevEng.Core.Routines.Extensions
{
    public static class PostgresRoutineServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgresStoredProcedureDesignTimeServices(
            this IServiceCollection services,
            IOperationReporter reporter = null)
        {
            if (reporter == null)
            {
                reporter = new OperationReporter(handler: null);
            }

            return services
                .AddSingleton<IProcedureModelFactory, PostgresStoredProcedureModelFactory>()
                ////.AddSingleton<IProcedureScaffolder, SqlServerStoredProcedureScaffolder>()
                .AddLogging(b => b.SetMinimumLevel(LogLevel.Debug).AddProvider(new OperationLoggerProvider(reporter)));
        }
    }
}
