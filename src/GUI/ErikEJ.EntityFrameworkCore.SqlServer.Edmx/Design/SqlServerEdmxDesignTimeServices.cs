using ErikEJ.EntityFrameworkCore.SqlServer.Edmx.Scaffolding;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

[assembly: DesignTimeProviderServices("ErikEJ.EntityFrameworkCore.SqlServer.Design.SqlServerEdmxDesignTimeServices")]

namespace ErikEJ.EntityFrameworkCore.SqlServer.Design
{
    public class SqlServerEdmxDesignTimeServices : IDesignTimeServices
    {
        public virtual void ConfigureDesignTimeServices(IServiceCollection services)
            => services
                .AddSingleton<LoggingDefinitions, SqlServerLoggingDefinitions>()
                .AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>()
                .AddSingleton<IDatabaseModelFactory, SqlServerEdmxDatabaseModelFactory>()
                .AddSingleton<IProviderConfigurationCodeGenerator, SqlServerCodeGenerator>()
                .AddSingleton<IAnnotationCodeGenerator, SqlServerAnnotationCodeGenerator>();
    }
}
