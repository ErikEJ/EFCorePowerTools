using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Spectre.Console;

namespace ErikEJ.EFCorePowerTools.Services;

internal static class PackageService
{
    public static NuGetVersion CurrentPackageVersion()
    {
        return new NuGetVersion(Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion);
    }

    public static async Task CheckForPackageUpdateAsync()
    {
        try
        {
            var logger = new NullLogger();
            var cancellationToken = CancellationToken.None;

            using var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<PackageMetadataResource>().ConfigureAwait(false);

            var packages = await resource.GetMetadataAsync(
                "ErikEJ.EFCorePowerTools.Cli",
                includePrerelease: false,
                includeUnlisted: false,
                cache,
                logger,
                cancellationToken).ConfigureAwait(false);

            var latestVersion = packages.Where(v => v.Identity.Version.Major == Constants.Version).Select(v => v.Identity.Version).MaxBy(v => v);
            if (latestVersion > CurrentPackageVersion())
            {
                DisplayService.MarkupLine("Your are not using the latest version of the tool, please update to get the latest bug fixes, features and support", Color.Yellow);
                DisplayService.MarkupLine($"Latest version is '{latestVersion}'", Color.Yellow);
                DisplayService.MarkupLine($"Run 'dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version {Constants.Version}.*' to get the latest version.", Color.Yellow);
            }
        }
#pragma warning disable CA1031
        catch (Exception)
        {
            // Ignore
        }
#pragma warning restore CA1031
    }
}
