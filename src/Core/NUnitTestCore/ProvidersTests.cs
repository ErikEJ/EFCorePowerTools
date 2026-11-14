using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using RevEng.Common;
using Xunit;

namespace UnitTests
{
    public class ProvidersTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public void LegacyOptionsMigrateBeforePackageSelection(int savedMode)
        {
            var serializer = new DataContractJsonSerializer(typeof(ReverseEngineerOptions));
            using var input = new MemoryStream(Encoding.UTF8.GetBytes($"{{\"CodeGenerationMode\":{savedMode}}}"));
            var options = (ReverseEngineerOptions)serializer.ReadObject(input);

            Assert.Equal(CodeGenerationMode.EFCore10, options.CodeGenerationMode);

            var databaseTypes = new[]
            {
                DatabaseType.SQLServer,
                DatabaseType.SQLServerDacpac,
                DatabaseType.SQLite,
                DatabaseType.Npgsql,
                DatabaseType.Mysql,
                DatabaseType.Oracle,
                DatabaseType.Firebird,
            };

            foreach (var databaseType in databaseTypes)
            {
                var expected = Providers.GetNeededPackages(databaseType, true, true, true, true, true, CodeGenerationMode.EFCore10);
                var actual = Providers.GetNeededPackages(databaseType, true, true, true, true, true, options.CodeGenerationMode);

                Assert.NotEmpty(actual);
                Assert.Equal(
                    expected.Select(p => (p.PackageId, p.Version, p.UseMethodName, p.IsMainProviderPackage)),
                    actual.Select(p => (p.PackageId, p.Version, p.UseMethodName, p.IsMainProviderPackage)));
            }

            using var output = new MemoryStream();
            serializer.WriteObject(output, options);
            using var document = System.Text.Json.JsonDocument.Parse(output.ToArray());
            Assert.Equal(6, document.RootElement.GetProperty("CodeGenerationMode").GetInt32());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public void NonLegacyOptionsRetainSavedMode(int savedMode)
        {
            var serializer = new DataContractJsonSerializer(typeof(ReverseEngineerOptions));
            using var input = new MemoryStream(Encoding.UTF8.GetBytes($"{{\"CodeGenerationMode\":{savedMode}}}"));
            var options = (ReverseEngineerOptions)serializer.ReadObject(input);

            Assert.Equal(savedMode, (int)options.CodeGenerationMode);
        }

        [Fact]
        public void GetNeededPackagesIncludesSqliteNodaTimeForEfCore10()
        {
            var packages = Providers.GetNeededPackages(
                DatabaseType.SQLite,
                useSpatial: false,
                useNodaTime: true,
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