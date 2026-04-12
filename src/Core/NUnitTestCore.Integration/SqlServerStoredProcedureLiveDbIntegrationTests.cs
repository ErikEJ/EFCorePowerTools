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
using NUnit.Framework;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Procedures;
using Testcontainers.MsSql;

namespace IntegrationTests
{
    [TestFixture]
    [NonParallelizable]
    public class SqlServerStoredProcedureLiveDbIntegrationTests
    {
        private const string DatabaseName = "DockerPlayground";
        private const string SaPassword = "Password!123";

        private MsSqlContainer _container;
        private string _databaseConnectionString;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword(SaPassword)
                .Build();

            await _container.StartAsync();

            var dacpacPath = GetDockerPlaygroundDacpacPath();
            DeployDacpac(dacpacPath);

            var connectionStringBuilder = new SqlConnectionStringBuilder(_container.GetConnectionString())
            {
                InitialCatalog = DatabaseName,
                TrustServerCertificate = true,
                Encrypt = false,
            };

            _databaseConnectionString = connectionStringBuilder.ConnectionString;
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if (_container != null)
            {
                await _container.DisposeAsync();
            }
        }

        [Test]
        public void LiveDbDiscoveryWithDefinitionFallbackRecoversObservedTempTableProcedures()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: true, useLegacyForLegacyProcedure: true);

            Assert.That(model.Errors, Is.Empty);

            AssertRoutineColumns(model, "StoGetSomeData", "SomeName", "SomeValue");
            AssertRoutineColumns(model, "StoGetSomeDataLegacyDiscovery", "OrderName", "OrderValue");
            AssertRoutineColumns(model, "StoGetSomeDataWithParameters", "CategoryId", "SearchTerm", "Amount");
        }

        [Test]
        public void LiveDbDiscoveryWithDefinitionFallbackRecoversMultipleTempTableResultSets()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: true, useLegacyForLegacyProcedure: true);
            var routine = model.Routines.Single(r => r.Name == "StoGetSomeDataMultipleResults");

            Assert.That(routine.SupportsMultipleResultSet, Is.True);
            Assert.That(routine.Results, Has.Count.EqualTo(2));
            Assert.That(routine.Results[0].Select(c => c.Name), Is.EqualTo(new[] { "CategoryId", "TotalCount" }));
            Assert.That(routine.Results[1].Select(c => c.Name), Is.EqualTo(new[] { "CategoryId", "ItemName" }));
        }

        [Test]
        public void LiveDbDiscoveryReturnsOnlyFirstResultSetWhenMultipleResultDiscoveryIsDisabled()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: false, useLegacyForLegacyProcedure: true);
            var routine = model.Routines.Single(r => r.Name == "StoGetSomeDataMultipleResults");

            Assert.That(routine.SupportsMultipleResultSet, Is.False);
            Assert.That(routine.Results, Has.Count.EqualTo(1));
            Assert.That(routine.Results[0].Select(c => c.Name), Is.EqualTo(new[] { "CategoryId", "TotalCount" }));
        }

        [Test]
        public void LiveDbDiscoveryNormalizesUnicodeParameterLengthFromBytesToCharacters()
        {
            var model = CreateRoutineModel(discoverMultipleResultSets: true, useLegacyForLegacyProcedure: true);
            var routine = model.Routines.Single(r => r.Name == "StoGetSomeDataWithParameters");
            var searchTerm = routine.Parameters.Single(p => p.Name == "SearchTerm");

            Assert.That(searchTerm.Length, Is.EqualTo(50));
        }

        [Test]
        public void LiveDbDiscoveryStillSucceedsForNonTempTableProcedure()
        {
            var model = CreateRoutineModel(
                discoverMultipleResultSets: false,
                useLegacyForLegacyProcedure: false,
                useGlobalLegacyDiscovery: false,
                modules: new[] { "[dbo].[StoGetSomeDataDirect]" });

            Assert.That(model.Errors, Is.Empty);
            AssertRoutineColumns(model, "StoGetSomeDataDirect", "SomeName", "SomeValue");
        }

        [Test]
        public void LiveDbDiscoveryWithGlobalLegacySettingRecoversTempTableProcedure()
        {
            var model = CreateRoutineModel(
                discoverMultipleResultSets: false,
                useLegacyForLegacyProcedure: false,
                useGlobalLegacyDiscovery: true,
                modules: new[] { "[dbo].[StoGetSomeDataLegacyDiscovery]" });

            Assert.That(model.Errors, Is.Empty);
            AssertRoutineColumns(model, "StoGetSomeDataLegacyDiscovery", "OrderName", "OrderValue");
        }

        [Test]
        public void LiveDbDiscoveryWithoutStoredProcedureResultSetFallbackFailsForObservedTempTableProcedures()
        {
            var model = CreateRoutineModel(
                discoverMultipleResultSets: true,
                useLegacyForLegacyProcedure: true,
                useGlobalLegacyDiscovery: false,
                useStoredProcedureResultSetFallback: false);

            Assert.That(model.Errors, Is.EqualTo(new[]
            {
                "Unable to get result set shape for procedure 'dbo.StoGetSomeData'. Invalid object name '#OrderTable'..",
                "Unable to get result set shape for procedure 'dbo.StoGetSomeDataLegacyDiscovery'. The metadata could not be determined because statement 'SELECT\n        t.OrderName,\n        t.OrderValue\n    FROM #OrderLegacyTable t' in procedure 'StoGetSomeDataLegacyDiscovery' uses a temp table..",
                "Unable to get result set shape for procedure 'dbo.StoGetSomeDataMultipleResults'. Invalid object name '#OrderSummaryTable'..",
                "Unable to get result set shape for procedure 'dbo.StoGetSomeDataWithParameters'. Invalid object name '#OrderSearchTable'..",
            }));

            foreach (var routineName in new[]
                     {
                         "StoGetSomeData",
                         "StoGetSomeDataLegacyDiscovery",
                         "StoGetSomeDataMultipleResults",
                         "StoGetSomeDataWithParameters",
                     })
            {
                var routine = model.Routines.Single(r => r.Name == routineName);
                Assert.That(routine.HasValidResultSet, Is.False, routineName);
            }

            Assert.That(model.Routines.Single(r => r.Name == "StoGetSomeDataDirect").HasValidResultSet, Is.True);
        }

        [Test]
        public async Task EfcptCliNet8LiveDbSmokeTestCompletes()
        {
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt8CliPath(), _databaseConnectionString, workingDirectory);

                Assert.That(stdout, Does.Contain("Getting database objects..."));
                Assert.That(stdout, Does.Contain("database objects discovered"));
                Assert.That(stdout, Does.Contain("files generated"));
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Test]
        public async Task EfcptCliLiveDbWithStoredProcedureResultSetFallbackDisabledShowsDiscoveryWarning()
        {
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                SetStoredProcedureResultSetFallback(workingDirectory, enabled: false);

                var stdout = await RunEfcptCliAsync(GetEfcpt10CliPath(), _databaseConnectionString, workingDirectory);
                const string expectedWarning = "warning: Unable to get result set shape for procedure 'dbo.StoGetSomeData'. Invalid object name '#OrderTable'..";

                Assert.That(NormalizeConsoleOutput(stdout), Does.Contain(NormalizeConsoleOutput(expectedWarning)));
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Test]
        public async Task EfcptCliNet10LiveDbSmokeTestGeneratesExpectedStoredProcedureOutput()
        {
            // This is a narrow end-to-end smoke test of the actual efcpt live-db path.
            // It complements the model-factory assertions above by checking the user-facing generated code.
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt10CliPath(), _databaseConnectionString, workingDirectory);
                Assert.That(stdout, Does.Not.Contain("warning: Unable to get result set shape"));

                var modelsPath = Path.Combine(workingDirectory, "Models");
                var proceduresPath = Path.Combine(modelsPath, "DockerPlaygroundContextProcedures.cs");
                var resultPath = Path.Combine(modelsPath, "StoGetSomeDataResult.cs");
                var legacyResultPath = Path.Combine(modelsPath, "StoGetSomeDataLegacyDiscoveryResult.cs");

                Assert.That(File.Exists(proceduresPath), Is.True, proceduresPath);
                Assert.That(File.Exists(resultPath), Is.True, resultPath);
                Assert.That(File.Exists(legacyResultPath), Is.True, legacyResultPath);

                var procedures = await File.ReadAllTextAsync(proceduresPath);
                var result = await File.ReadAllTextAsync(resultPath);
                var legacyResult = await File.ReadAllTextAsync(legacyResultPath);

                Assert.That(procedures, Does.Contain("Size = 50"));
                Assert.That(result, Does.Contain("SomeName"));
                Assert.That(result, Does.Contain("SomeValue"));
                Assert.That(legacyResult, Does.Contain("OrderName"));
                Assert.That(legacyResult, Does.Contain("OrderValue"));
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Test]
        public async Task EfcptCliLiveDbGeneratedResultModelsMatchGoldenFiles()
        {
            // Keep this focused on the generated result models that were affected by the temp-table
            // discovery and Unicode-length fixes. If a future change alters the emitted code, this
            // test forces that change to be intentional and reviewable.
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt10CliPath(), _databaseConnectionString, workingDirectory);
                Assert.That(stdout, Does.Not.Contain("warning: Unable to get result set shape"));

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

        [Test]
        public async Task EfcptCliNet8DacpacSmokeTestCompletes()
        {
            // This guards the host-lifecycle regression where the net10 CLI stopped after
            // "Getting database objects..." without completing scaffolding.
            var workingDirectory = CreateSmokeTestWorkingDirectory();
            try
            {
                var dacpacPath = GetDockerPlaygroundDacpacPath();
                var stdout = await RunEfcptCliAsync(GetEfcpt8CliPath(), dacpacPath, workingDirectory);

                Assert.That(stdout, Does.Contain("Getting database objects..."));
                Assert.That(stdout, Does.Contain("database objects discovered"));
                Assert.That(stdout, Does.Contain("files generated"));
            }
            finally
            {
                if (Directory.Exists(workingDirectory))
                {
                    Directory.Delete(workingDirectory, recursive: true);
                }
            }
        }

        [Test]
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

            Assert.That(liveModel.Errors, Is.Empty);
            Assert.That(dacpacModel.Errors, Is.Empty);

            var liveRoutines = liveModel.Routines.ToDictionary(r => r.Name);
            var dacpacRoutines = dacpacModel.Routines.ToDictionary(r => r.Name);

            foreach (var routineName in new[] { "StoGetSomeData", "StoGetSomeDataLegacyDiscovery", "StoGetSomeDataWithParameters" })
            {
                Assert.That(dacpacRoutines.ContainsKey(routineName), Is.True, routineName);
                Assert.That(liveRoutines.ContainsKey(routineName), Is.True, routineName);

                var liveRoutine = liveRoutines[routineName];
                var dacpacRoutine = dacpacRoutines[routineName];

                Assert.That(dacpacRoutine.Results.Count, Is.EqualTo(liveRoutine.Results.Count), routineName);
                Assert.That(dacpacRoutine.Results[0].Select(c => c.Name), Is.EqualTo(liveRoutine.Results[0].Select(c => c.Name)), routineName);
            }

            var liveSearchTerm = liveRoutines["StoGetSomeDataWithParameters"].Parameters.Single(p => p.Name == "SearchTerm");
            var dacpacSearchTerm = dacpacRoutines["StoGetSomeDataWithParameters"].Parameters.Single(p => p.Name == "SearchTerm");

            Assert.That(dacpacSearchTerm.Length, Is.EqualTo(liveSearchTerm.Length));
        }

        private RoutineModel CreateRoutineModel(
            bool discoverMultipleResultSets,
            bool useLegacyForLegacyProcedure,
            bool useGlobalLegacyDiscovery = false,
            bool useStoredProcedureResultSetFallback = true,
            IEnumerable<string> modules = null)
        {
            var factory = new SqlServerStoredProcedureModelFactory();

            return factory.Create(_databaseConnectionString, new ModuleModelFactoryOptions
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

            Assert.That(routine.HasValidResultSet, Is.True);
            Assert.That(routine.Results, Has.Count.GreaterThan(0));
            Assert.That(routine.Results[0].Select(c => c.Name), Is.EqualTo(expectedColumnNames));
        }

        private static string GetDockerPlaygroundDacpacPath()
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
            Assert.That(process, Is.Not.Null);

            var stdout = await process.StandardOutput.ReadToEndAsync();
            var stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            Assert.That(process.ExitCode, Is.EqualTo(0), $"efcpt failed.{Environment.NewLine}{stdout}{Environment.NewLine}{stderr}");

            return stdout;
        }

        private void DeployDacpac(string dacpacPath)
        {
            var adminConnectionString = new SqlConnectionStringBuilder(_container.GetConnectionString())
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

        private static string GetRepositoryRoot()
        {
            var current = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

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
            Assert.That(File.Exists(expectedPath), Is.True, expectedPath);
            Assert.That(File.Exists(actualPath), Is.True, actualPath);

            var expected = NormalizeLineEndings(File.ReadAllText(expectedPath));
            var actual = NormalizeLineEndings(File.ReadAllText(actualPath));

            Assert.That(actual, Is.EqualTo(expected), actualPath);
        }

        private static string NormalizeLineEndings(string text)
        {
            return text.Replace("\r\n", "\n", StringComparison.Ordinal);
        }
    }
}
