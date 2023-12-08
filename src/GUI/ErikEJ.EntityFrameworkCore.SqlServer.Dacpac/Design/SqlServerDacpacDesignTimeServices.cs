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
            new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection)
                .TryAdd<IAnnotationCodeGenerator, SqlServerAnnotationCodeGenerator>()
                .TryAdd<ICSharpRuntimeAnnotationCodeGenerator, SqlServerCSharpRuntimeAnnotationCodeGenerator>()
                .TryAdd<IDatabaseModelFactory, SqlServerDacpacDatabaseModelFactory>()
                .TryAdd<IProviderConfigurationCodeGenerator, SqlServerCodeGenerator>()
                .TryAddCoreServices();
        }
    }
}
