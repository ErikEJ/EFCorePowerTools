using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;

[assembly: DesignTimeProviderServices("ErikEJ.EntityFrameworkCore.SqlServer.Design.SqlServerDacpacDesignTimeServices")]

namespace ErikEJ.EntityFrameworkCore.SqlServer.Design
{
    public class SqlServerDacpacDesignTimeServices : IDesignTimeServices
    {
        public virtual void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddEntityFrameworkSqlServer();
#pragma warning disable EF1001 // Internal EF Core API usage.
            new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection)
                .TryAdd<IAnnotationCodeGenerator, SqlServerAnnotationCodeGenerator>()
                .TryAdd<ICSharpRuntimeAnnotationCodeGenerator, SqlServerCSharpRuntimeAnnotationCodeGenerator>()
#pragma warning restore EF1001 // Internal EF Core API usage.
                .TryAdd<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>()
                .TryAdd<IProviderConfigurationCodeGenerator, SqlServerCodeGenerator>()
                .TryAddCoreServices();
        }
    }
}
