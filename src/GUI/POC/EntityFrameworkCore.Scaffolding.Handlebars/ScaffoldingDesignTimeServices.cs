using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Scaffolding.Handlebars
{
    /// <summary>
    /// Used by EF Core toolchain to register design time services.
    /// </summary>
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        /// <summary>
        /// Register Handlebars scaffolding with design time dependency injection system.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            var options = ReverseEngineerOptions.DbContextAndEntities;
            services.AddHandlebarsScaffolding(null, options);
        }
    }
}