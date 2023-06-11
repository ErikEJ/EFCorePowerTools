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

internal sealed class PackageService
{
    private readonly DisplayService displayService;

    public PackageService(DisplayService displayService)
    {
        this.displayService = displayService;
    }

    public static NuGetVersion CurrentPackageVersion()
    {
        return new NuGetVersion(Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion);
    }

    public async Task CheckForPackageUpdateAsync()
    {
        try
        {
            var logger = new NullLogger();
            var cancellationToken = CancellationToken.None;

            using var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<FindPackageByIdResource>().ConfigureAwait(false);

            var versions = await resource.GetAllVersionsAsync(
                "ErikEJ.EFCorePowerTools.Cli",
                cache,
                logger,
                cancellationToken).ConfigureAwait(false);

            var latestVersion = versions.Where(v => v.Major == Constants.Version).MaxBy(v => v);
            if (latestVersion > CurrentPackageVersion())
            {
                displayService.MarkupLine("Your are not using the latest version of the tool, please update to get the latest bug fixes, features and support", Color.Yellow);
                displayService.MarkupLine($"Latest version is '{latestVersion}'", Color.Yellow);
                displayService.MarkupLine($"Run 'dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version {Constants.Version}.0.*-*' to get the latest version.", Color.Yellow);
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
