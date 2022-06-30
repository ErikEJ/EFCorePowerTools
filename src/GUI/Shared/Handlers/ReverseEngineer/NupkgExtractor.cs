using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class NupkgExtractor
    {
        public async Task ExtractNupgkAsync(string packageId, string version, DirectoryInfo directory)
        {
            var logger = NullLogger.Instance;
            var cancellationToken = CancellationToken.None;

            var cache = new SourceCacheContext();
            var packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
            var sourceRepository = Repository.Factory.GetCoreV3(packageSource);
            var resource = await sourceRepository.GetResourceAsync<FindPackageByIdResource>();

            var packageVersion = new NuGetVersion(version);
            using (var packageStream = new MemoryStream())
            {
                await resource.CopyNupkgToStreamAsync(
                    packageId,
                    packageVersion,
                    packageStream,
                    cache,
                    logger,
                    cancellationToken);

                ExtractFiles(directory, logger, packageStream);
            }
        }

        private static void ExtractFiles(DirectoryInfo directory, ILogger logger, MemoryStream packageStream)
        {
            using (var reader = new PackageArchiveReader(packageStream))
            {
                var libs = reader.GetLibItems();
                foreach (var lib in libs)
                {
                    if (lib.TargetFramework == NuGet.Frameworks.NuGetFramework.Parse("netstandard2.0"))
                    {
                        foreach (var file in lib.Items)
                        {
                            var target = Path.Combine(directory.FullName, Path.GetFileName(file));
                            if (file.StartsWith("lib/netstandard2.0/Microsoft.", System.StringComparison.Ordinal)
                                && !File.Exists(target))
                            {
                                reader.ExtractFile(file, target, logger);
                            }
                        }
                    }
                }
            }
        }
    }
}
