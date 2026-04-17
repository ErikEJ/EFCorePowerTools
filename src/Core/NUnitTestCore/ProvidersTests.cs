using System.Linq;
using RevEng.Common;
using Xunit;

namespace UnitTests
{
    public class ProvidersTests
    {
        [Fact]
        public void GetNeededPackagesIncludesSqliteNodaTimeForEfCore10()
        {
            var packages = Providers.GetNeededPackages(
                DatabaseType.SQLite,
                useSpatial: false,
                useNodaTime: true,
                useDateOnlyTimeOnly: false,
                useHierarchyId: false,
                discoverMultipleResultSets: false,
                hasProcedures: false,
                CodeGenerationMode.EFCore10);

            var nodaTimePackage = packages.Single(p => p.PackageId == "EntityFrameworkCore.Sqlite.NodaTime");

            Assert.Equal("10.0.0", nodaTimePackage.Version);
            Assert.Equal("NodaTime", nodaTimePackage.UseMethodName);
        }
    }
}
