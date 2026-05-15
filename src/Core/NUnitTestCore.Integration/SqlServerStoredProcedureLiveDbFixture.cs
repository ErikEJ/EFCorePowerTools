using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Xunit;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Procedures;
using Testcontainers.MsSql;

namespace IntegrationTests
{
    public sealed class SqlServerStoredProcedureLiveDbFixture : IAsyncLifetime
    {
        private const string DatabaseName = "DockerPlayground";
        private const string SaPassword = "Password!123";

        private MsSqlContainer container;

        public string DatabaseConnectionString { get; private set; }

        public async ValueTask InitializeAsync()
        {
            container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword(SaPassword)
                .Build();

            await container.StartAsync();

            var dacpacPath = SqlServerStoredProcedureLiveDbIntegrationTests.GetDockerPlaygroundDacpacPath();
            DeployDacpac(dacpacPath);

            var connectionStringBuilder = new SqlConnectionStringBuilder(container.GetConnectionString())
            {
                InitialCatalog = DatabaseName,
                TrustServerCertificate = true,
                Encrypt = false,
            };

            DatabaseConnectionString = connectionStringBuilder.ConnectionString;
        }

        public async ValueTask DisposeAsync()
        {
            if (container != null)
            {
                await container.DisposeAsync();
            }
        }

        private void DeployDacpac(string dacpacPath)
        {
            var adminConnectionString = new SqlConnectionStringBuilder(container.GetConnectionString())
            {
                InitialCatalog = "master",
                TrustServerCertificate = true,
                Encrypt = false,
            }.ConnectionString;

            using var package = DacPackage.Load(dacpacPath);
            var services = new DacServices(adminConnectionString);
            services.Deploy(
                package,
                DatabaseName,
                upgradeExisting: true,
                options: new DacDeployOptions
                {
                    BlockOnPossibleDataLoss = false,
                    CreateNewDatabase = true,
                    CommandTimeout = 300,
                });
        }
    }

    public class SqlServerStoredProcedureLiveDbIntegrationTests : IClassFixture<SqlServerStoredProcedureLiveDbFixture>
    {
        private readonly SqlServerStoredProcedureLiveDbFixture fixture;

        public SqlServerStoredProcedureLiveDbIntegrationTests(SqlServerStoredProcedureLiveDbFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void LiveDbDiscoveryWithDefinitionFallbackRecoversObservedTempTableProcedures()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: true, useLegacyForLegacyProcedure: true);

            Assert.Empty(model.Errors);

            AssertRoutineColumns(model, "StoGetSomeData", "SomeName", "SomeValue");
            AssertRoutineColumns(model, "StoGetSomeDataLegacyDiscovery", "OrderName", "OrderValue");
            AssertRoutineColumns(model, "StoGetSomeDataWithParameters", "CategoryId", "SearchTerm", "Amount");
        }

        [Fact]
        public void LiveDbDiscoveryWithDefinitionFallbackRecoversMultipleTempTableResultSets()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: true, useLegacyForLegacyProcedure: true);
            var routine = model.Routines.Single(r => r.Name == "StoGetSomeDataMultipleResults");

            Assert.True(routine.SupportsMultipleResultSet);
            Assert.Equal(2, routine.Results.Count);
            Assert.Equal(new[] { "CategoryId", "TotalCount" }, routine.Results[0].Select(c => c.Name));
            Assert.Equal(new[] { "CategoryId", "ItemName" }, routine.Results[1].Select(c => c.Name));
        }

        [Fact]
        public void LiveDbDiscoveryReturnsOnlyFirstResultSetWhenMultipleResultDiscoveryIsDisabled()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: false, useLegacyForLegacyProcedure: true);
            var routine = model.Routines.Single(r => r.Name == "StoGetSomeDataMultipleResults");

            Assert.False(routine.SupportsMultipleResultSet);
            Assert.Single(routine.Results);
            Assert.Equal(new[] { "CategoryId", "TotalCount" }, routine.Results[0].Select(c => c.Name));
        }

        [Fact]
        public void LiveDbDiscoveryNormalizesUnicodeParameterLengthFromBytesToCharacters()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: true, useLegacyForLegacyProcedure: true);
            var routine = model.Routines.Single(r => r.Name == "StoGetSomeDataWithParameters");
            var searchTerm = routine.Parameters.Single(p => p.Name == "SearchTerm");

            Assert.Equal(50, searchTerm.Length);
        }

        [Fact]
        public void LiveDbDiscoveryStillSucceedsForNonTempTableProcedure()
        {
            var model = CreateRoutineModel(
                discoverMultipleResultSets: false,
                useLegacyForLegacyProcedure: false,
                useGlobalLegacyDiscovery: false,
                modules: new[] { "[dbo].[StoGetSomeDataDirect]" });

            Assert.Empty(model.Errors);
            AssertRoutineColumns(model, "StoGetSomeDataDirect", "SomeName", "SomeValue");
        }

        [Fact]
        public void LiveDbDiscoveryWithGlobalLegacySettingRecoversTempTableProcedure()
        {
            var model = CreateRoutineModel(
                discoverMultipleResultSets: false,
                useLegacyForLegacyProcedure: false,
                useGlobalLegacyDiscovery: true,
                modules: new[] { "[dbo].[StoGetSomeDataLegacyDiscovery]" });

            Assert.Empty(model.Errors);
            AssertRoutineColumns(model, "StoGetSomeDataLegacyDiscovery", "OrderName", "OrderValue");
        }

        [Fact]
        public void LiveDbDiscoveryWithoutStoredProcedureResultSetFallbackFailsForObservedTempTableProcedures()
        {
            var model = CreateRoutineModel(
                discoverMultipleResultSets: true,
                useLegacyForLegacyProcedure: true,
                useGlobalLegacyDiscovery: false,
                useStoredProcedureResultSetFallback: false);

            Assert.Equal(
                new[]
                {
                    "Unable to get result set shape for procedure 'dbo.StoGetSomeData'. Invalid object name '#OrderTable'..",
                    "Unable to get result set shape for procedure 'dbo.StoGetSomeDataLegacyDiscovery'. The metadata could not be determined because statement 'SELECT\n        t.OrderName,\n        t.OrderValue\n    FROM #OrderLegacyTable t' in procedure 'StoGetSomeDataLegacyDiscovery' uses a temp table..",
                    "Unable to get result set shape for procedure 'dbo.StoGetSomeDataMultipleResults'. Invalid object name '#OrderSummaryTable'..",
                    "Unable to get result set shape for procedure 'dbo.StoGetSomeDataWithParameters'. Invalid object name '#OrderSearchTable'..",
                },
                model.Errors);

            foreach (var routineName in new[]
                     {
                         "StoGetSomeData",
                         "StoGetSomeDataLegacyDiscovery",
                         "StoGetSomeDataMultipleResults",
                         "StoGetSomeDataWithParameters",
                     })
            {
                var routine = model.Routines.Single(r => r.Name == routineName);
                Assert.False(routine.HasValidResultSet);
            }

            Assert.True(model.Routines.Single(r => r.Name == "StoGetSomeDataDirect").HasValidResultSet);
        }

        [Fact]
        public async Task EfcptCliNet8LiveDbSmokeTestCompletes()
        {
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt8CliPath(), fixture.DatabaseConnectionString, workingDirectory);

                Assert.Contains("Getting database objects...", stdout);
                Assert.Contains("database objects discovered", stdout);
                Assert.Contains("files generated", stdout);
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Fact]
        public async Task EfcptCliLiveDbWithStoredProcedureResultSetFallbackDisabledShowsDiscoveryWarning()
        {
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                SetStoredProcedureResultSetFallback(workingDirectory, enabled: false);

                var stdout = await RunEfcptCliAsync(GetEfcpt10CliPath(), fixture.DatabaseConnectionString, workingDirectory);
                const string expectedWarning = "warning: Unable to get result set shape for procedure 'dbo.StoGetSomeData'. Invalid object name '#OrderTable'..";

                Assert.Contains(NormalizeConsoleOutput(expectedWarning), NormalizeConsoleOutput(stdout));
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Fact]
        public async Task EfcptCliNet10LiveDbSmokeTestGeneratesExpectedStoredProcedureOutput()
        {
            // This is a narrow end-to-end smoke test of the actual efcpt live-db path.
            // It complements the model-factory assertions above by checking the user-facing generated code.
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt10CliPath(), fixture.DatabaseConnectionString, workingDirectory);
                Assert.DoesNotContain("warning: Unable to get result set shape", stdout);

                var modelsPath = Path.Combine(workingDirectory, "Models");
                var proceduresPath = Path.Combine(modelsPath, "DockerPlaygroundContextProcedures.cs");
                var resultPath = Path.Combine(modelsPath, "StoGetSomeDataResult.cs");
                var legacyResultPath = Path.Combine(modelsPath, "StoGetSomeDataLegacyDiscoveryResult.cs");

                Assert.True(File.Exists(proceduresPath), proceduresPath);
                Assert.True(File.Exists(resultPath), resultPath);
                Assert.True(File.Exists(legacyResultPath), legacyResultPath);

                var procedures = await File.ReadAllTextAsync(proceduresPath, TestContext.Current.CancellationToken);
                var result = await File.ReadAllTextAsync(resultPath, TestContext.Current.CancellationToken);
                var legacyResult = await File.ReadAllTextAsync(legacyResultPath, TestContext.Current.CancellationToken);

                Assert.Contains("Size = 50", procedures);
                Assert.Contains("SomeName", result);
                Assert.Contains("SomeValue", result);
                Assert.Contains("OrderName", legacyResult);
                Assert.Contains("OrderValue", legacyResult);
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Fact]
        public async Task EfcptCliLiveDbGeneratedResultModelsMatchGoldenFiles()
        {
            // Keep this focused on the generated result models that were affected by the temp-table
            // discovery and Unicode-length fixes. If a future change alters the emitted code, this
            // test forces that change to be intentional and reviewable.
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt10CliPath(), fixture.DatabaseConnectionString, workingDirectory);
                Assert.DoesNotContain("warning: Unable to get result set shape", stdout);

                var modelsPath = Path.Combine(workingDirectory, "Models");
                var goldenFilesPath = Path.Combine(GetRepositoryRoot(), "src", "Core", "NUnitTestCore.Integration", "GoldenFiles");

                AssertGoldenFileMatch(
                    Path.Combine(goldenFilesPath, "StoGetSomeDataResult.cs"),
                    Path.Combine(modelsPath, "StoGetSomeDataResult.cs"));
                AssertGoldenFileMatch(
                    Path.Combine(goldenFilesPath, "StoGetSomeDataLegacyDiscoveryResult.cs"),
                    Path.Combine(modelsPath, "StoGetSomeDataLegacyDiscoveryResult.cs"));
                AssertGoldenFileMatch(
                    Path.Combine(goldenFilesPath, "StoGetSomeDataMultipleResultsResult.cs"),
                    Path.Combine(modelsPath, "StoGetSomeDataMultipleResultsResult.cs"));
                AssertGoldenFileMatch(
                    Path.Combine(goldenFilesPath, "StoGetSomeDataWithParametersResult.cs"),
                    Path.Combine(modelsPath, "StoGetSomeDataWithParametersResult.cs"));
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Fact]
        public async Task EfcptCliNet8DacpacSmokeTestCompletes()
        {
            // This guards the host-lifecycle regression where the net10 CLI stopped after
            // "Getting database objects..." without completing scaffolding.
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var dacpacPath = GetDockerPlaygroundDacpacPath();
                var stdout = await RunEfcptCliAsync(GetEfcpt8CliPath(), dacpacPath, workingDirectory);

                Assert.Contains("Getting database objects...", stdout);
                Assert.Contains("database objects discovered", stdout);
                Assert.Contains("files generated", stdout);
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Fact]
        public void DacpacAndLiveDbPathsProduceMatchingSingleResultProcedureMetadata()
        {
            // This parity check stays intentionally narrow:
            // - StoGetSomeDataDirect is excluded because the current DACPAC parser does not infer
            //   result metadata from its direct literal SELECT shape, even though live-db metadata does.
            // - StoGetSomeDataMultipleResults is excluded because the current DACPAC path here only
            //   models the first result set, while the live-db path in this test is exercising multi-result recovery.
            var modules = new[]
            {
                "[dbo].[StoGetSomeData]",
                "[dbo].[StoGetSomeDataLegacyDiscovery]",
                "[dbo].[StoGetSomeDataWithParameters]",
            };

            var liveModel = CreateRoutineModel(
                discoverMultipleResultSets: false,
                useLegacyForLegacyProcedure: true,
                modules: modules);

            var dacpacFactory = new SqlServerDacpacStoredProcedureModelFactory(new SqlServerDacpacDatabaseModelFactoryOptions());
            var dacpacModel = dacpacFactory.Create(GetDockerPlaygroundDacpacPath(), new ModuleModelFactoryOptions
            {
                FullModel = true,
                Modules = modules,
            });

            Assert.Empty(liveModel.Errors);
            Assert.Empty(dacpacModel.Errors);

            var liveRoutines = liveModel.Routines.ToDictionary(r => r.Name);
            var dacpacRoutines = dacpacModel.Routines.ToDictionary(r => r.Name);

            foreach (var routineName in new[] { "StoGetSomeData", "StoGetSomeDataLegacyDiscovery", "StoGetSomeDataWithParameters" })
            {
                Assert.True(dacpacRoutines.ContainsKey(routineName), routineName);
                Assert.True(liveRoutines.ContainsKey(routineName), routineName);

                var liveRoutine = liveRoutines[routineName];
                var dacpacRoutine = dacpacRoutines[routineName];

                Assert.Equal(liveRoutine.Results.Count, dacpacRoutine.Results.Count);
                Assert.Equal(liveRoutine.Results[0].Select(c => c.Name), dacpacRoutine.Results[0].Select(c => c.Name));
            }

            var liveSearchTerm = liveRoutines["StoGetSomeDataWithParameters"].Parameters.Single(p => p.Name == "SearchTerm");
            var dacpacSearchTerm = dacpacRoutines["StoGetSomeDataWithParameters"].Parameters.Single(p => p.Name == "SearchTerm");

            Assert.Equal(liveSearchTerm.Length, dacpacSearchTerm.Length);
        }

        private RoutineModel CreateRoutineModel(
            bool discoverMultipleResultSets,
            bool useLegacyForLegacyProcedure,
            bool useGlobalLegacyDiscovery = false,
            bool useStoredProcedureResultSetFallback = true,
            IEnumerable<string> modules = null)
        {
            var factory = new SqlServerStoredProcedureModelFactory();

            return factory.Create(fixture.DatabaseConnectionString, new ModuleModelFactoryOptions
            {
                DiscoverMultipleResultSets = discoverMultipleResultSets,
                FullModel = true,
                UseLegacyResultSetDiscovery = useGlobalLegacyDiscovery,
                UseStoredProcedureResultSetFallback = useStoredProcedureResultSetFallback,
                Modules = modules ?? new[]
                {
                    "[dbo].[StoGetSomeData]",
                    "[dbo].[StoGetSomeDataDirect]",
                    "[dbo].[StoGetSomeDataLegacyDiscovery]",
                    "[dbo].[StoGetSomeDataMultipleResults]",
                    "[dbo].[StoGetSomeDataWithParameters]",
                },
                ModulesUsingLegacyDiscovery = useLegacyForLegacyProcedure
                    ? new[] { "[dbo].[StoGetSomeDataLegacyDiscovery]" }
                    : Array.Empty<string>(),
            });
        }

        private static void AssertRoutineColumns(RoutineModel model, string routineName, params string[] expectedColumnNames)
        {
            var routine = model.Routines.Single(r => r.Name == routineName);

            Assert.True(routine.HasValidResultSet);
            Assert.True(routine.Results.Count > 0);
            Assert.Equal(expectedColumnNames, routine.Results[0].Select(c => c.Name));
        }

        internal static string GetDockerPlaygroundDacpacPath()
        {
            var dacpacPath = Path.Combine(GetRepositoryRoot(), "test", "ScaffoldingTester", "DockerPlayground", "bin", "Debug", "net10.0", "DockerPlayground.dacpac");

            if (!File.Exists(dacpacPath))
            {
                throw new FileNotFoundException("DockerPlayground dacpac was not created by the build graph.", dacpacPath);
            }

            return dacpacPath;
        }

        private static string GetEfcpt8CliPath()
        {
            var cliPath = Path.Combine(GetRepositoryRoot(), "src", "Core", "efcpt.8", "bin", "Debug", "net8.0", "efcpt.8.dll");

            if (!File.Exists(cliPath))
            {
                throw new FileNotFoundException("efcpt.8 CLI assembly was not created by the build graph.", cliPath);
            }

            return cliPath;
        }

        private static string GetEfcpt10CliPath()
        {
            var cliPath = Path.Combine(GetRepositoryRoot(), "src", "Core", "efcpt.10", "bin", "Debug", "net10.0", "efcpt.10.dll");

            if (!File.Exists(cliPath))
            {
                throw new FileNotFoundException("efcpt.10 CLI assembly was not created by the build graph.", cliPath);
            }

            return cliPath;
        }

        private static string CreateSmokeTestWorkingDirectory()
        {
            var repoRoot = GetRepositoryRoot();
            var sourceConfigPath = Path.Combine(repoRoot, "test", "ScaffoldingTester", "DockerPlayground", "efcpt-config.json");
            var workingDirectory = Path.Combine(Path.GetTempPath(), "efcpt-live-smoke-" + Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(workingDirectory);
            File.Copy(sourceConfigPath, Path.Combine(workingDirectory, "efcpt-config.json"));

            return workingDirectory;
        }

        private static void SetStoredProcedureResultSetFallback(string workingDirectory, bool enabled)
        {
            var configPath = Path.Combine(workingDirectory, "efcpt-config.json");
            var config = JsonNode.Parse(File.ReadAllText(configPath))?.AsObject()
                ?? throw new InvalidOperationException("Unable to parse efcpt-config.json.");

            var codeGeneration = config["code-generation"]?.AsObject()
                ?? throw new InvalidOperationException("efcpt-config.json is missing the code-generation section.");

            codeGeneration["use-stored-procedure-resultset-fallback"] = enabled;

            File.WriteAllText(
                configPath,
                config.ToJsonString(new JsonSerializerOptions
                {
                    WriteIndented = true,
                }));
        }

        private static string NormalizeConsoleOutput(string value)
        {
            var withoutAnsi = Regex.Replace(value, @"\x1B\[[0-9;?]*[ -/]*[@-~]", string.Empty);
            return Regex.Replace(withoutAnsi, "\\s+", " ").Trim();
        }

        private static async Task<string> RunEfcptCliAsync(string cliPath, string input, string workingDirectory)
        {
            var startInfo = new ProcessStartInfo("dotnet", $"\"{cliPath}\" \"{input}\" mssql")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory,
            };

            using var process = Process.Start(startInfo);
            Assert.NotNull(process);

            var stdout = await process.StandardOutput.ReadToEndAsync();
            var stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync(TestContext.Current.CancellationToken);

            Assert.True(process.ExitCode == 0, $"efcpt failed.{Environment.NewLine}{stdout}{Environment.NewLine}{stderr}");

            return stdout;
        }

        private static string GetRepositoryRoot()
        {
            var current = new DirectoryInfo(AppContext.BaseDirectory);

            while (current != null)
            {
                if (Directory.Exists(Path.Combine(current.FullName, ".git")))
                {
                    return current.FullName;
                }

                current = current.Parent;
            }

            throw new DirectoryNotFoundException("Unable to locate repository root from test output directory.");
        }

        private static void AssertGoldenFileMatch(string expectedPath, string actualPath)
        {
            Assert.True(File.Exists(expectedPath), expectedPath);
            Assert.True(File.Exists(actualPath), actualPath);

            var expected = NormalizeLineEndings(File.ReadAllText(expectedPath));
            var actual = NormalizeLineEndings(File.ReadAllText(actualPath));

            Assert.Equal(expected, actual);
        }

        private static string NormalizeLineEndings(string text)
        {
            return text.Replace("\r\n", "\n", StringComparison.Ordinal);
        }
    }
}
