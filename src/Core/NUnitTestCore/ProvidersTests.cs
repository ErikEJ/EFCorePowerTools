using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        [Fact]
        public void GetNeededPackagesDoesNotIncludeDapperUntilGeneratedCodeUsesIt()
        {
            var packages = Providers.GetNeededPackages(
                DatabaseType.SQLServer,
                useSpatial: false,
                useNodaTime: false,
                useDateOnlyTimeOnly: false,
                useHierarchyId: false,
                discoverMultipleResultSets: true,
                hasProcedures: true,
                CodeGenerationMode.EFCore8);

            Assert.DoesNotContain(packages, p => p.PackageId == "Dapper");
        }

        [Fact]
        public void AddGeneratedCodePackagesIncludesDapperWhenGeneratedCodeUsesIt()
        {
            var packages = new List<NuGetPackage>();
            var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.cs");

            try
            {
                File.WriteAllText(tempFile, "using Dapper;" + Environment.NewLine, Encoding.UTF8);

                Providers.AddGeneratedCodePackages(packages, DatabaseType.SQLServer, new[] { tempFile });

                var dapperPackage = Assert.Single(packages, p => p.PackageId == "Dapper");
                Assert.Equal("2.1.66", dapperPackage.Version);
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}
