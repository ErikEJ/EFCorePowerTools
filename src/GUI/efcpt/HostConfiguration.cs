using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ErikEJ.EfCorePowerTools;

internal static class HostConfiguration
{
    public static IHostBuilder Configure(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureHostConfiguration(
                builder => builder.AddEnvironmentVariables())
            .UseConsoleLifetime();
    }
}
