using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Spectre.Console;

namespace ErikEJ.EfCorePowerTools;

internal static class UpdateChecker
{
    public static async Task CheckForPackageUpdateAsync(int efCoreVersion)
    {
        try
        {
            var logger = new NuGet.Common.NullLogger();
            var cancellationToken = CancellationToken.None;

            using var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            var versions = await resource.GetAllVersionsAsync(
                "ErikEJ.EFCorePowerTools.CLI",
                cache,
                logger,
                cancellationToken);

            var latestVersion = versions.Where(v => v.Major == efCoreVersion).OrderByDescending(v => v).FirstOrDefault();

            if (latestVersion > CurrentPackageVersion())
            {
                AnsiConsole.Markup("[yellow]Your are not using the latest version of the tool, please update to get the latest bug fixes, features and support.[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.Markup($"[yellow]Latest version is '{latestVersion}'.[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.Markup($"[yellow]Run 'dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version {efCoreVersion}.0.*-*' to get the latest version.[/]");
            }
        }
        catch (Exception)
        {
            // Ignore
        }
    }

    private static NuGetVersion CurrentPackageVersion()
    {
        return new NuGetVersion(Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
    }
}
