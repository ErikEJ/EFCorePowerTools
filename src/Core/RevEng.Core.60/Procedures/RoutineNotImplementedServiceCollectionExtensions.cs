using Microsoft.Extensions.DependencyInjection;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Functions;

namespace RevEng.Core.Procedures
{
    public static class RoutineNotImplementedServiceCollectionExtensions
    {
        public static IServiceCollection AddNotImplementedDesignTimeServices(
                    this IServiceCollection services)
        {
            return services
                .AddSingleton<IProcedureModelFactory, NotImplementedProcedureModelFactory>()
                .AddSingleton<IProcedureScaffolder, NotImplementedProcedureScaffolder>()
                .AddSingleton<IFunctionModelFactory, NotImplementedFunctionModelFactory>()
                .AddSingleton<IFunctionScaffolder, NotImplementedFunctionScaffolder>();
        }
    }
}
