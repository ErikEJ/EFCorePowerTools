using Microsoft.Extensions.DependencyInjection;
using RevEng.Core.Abstractions.Model;
using RevEng.Core.Routines.Functions;
using RevEng.Core.Routines.Procedures;

namespace RevEng.Core.Routines.Extensions
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
