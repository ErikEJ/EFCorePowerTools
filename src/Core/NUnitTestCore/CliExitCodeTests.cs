using System;
using System.IO;
using System.Threading.Tasks;
using ErikEJ.EFCorePowerTools;
using Xunit;

namespace NUnitTestCore
{
    // Program.MainAsync uses the process-wide Environment.ExitCode, so these tests must not run in parallel.
    [Collection(nameof(CliExitCodeTests))]
    public sealed class CliExitCodeTests : IDisposable
    {
        private static readonly TimeSpan RunTimeout = TimeSpan.FromSeconds(60);

        private readonly string workDirectory;
        private readonly string dacpacPath;
        private readonly string configPath;

        public CliExitCodeTests()
        {
            workDirectory = Path.Combine(Path.GetTempPath(), "EFCorePowerTools.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(workDirectory);

            dacpacPath = Path.Combine(workDirectory, "abc.dacpac");
            File.Copy(Path.Combine(AppContext.BaseDirectory, "Dacpac", "abc.dacpac"), dacpacPath);

            configPath = Path.Combine(workDirectory, "efcpt-config.json");
            File.WriteAllText(
                configPath,
                @"{
  ""code-generation"": { ""use-t4"": true, ""refresh-object-lists"": true },
  ""names"": { ""dbcontext-name"": ""TestContext"", ""root-namespace"": ""TestNamespace"" }
}");

            Environment.ExitCode = 0;
        }

        public void Dispose()
        {
            Environment.ExitCode = 0;

            if (Directory.Exists(workDirectory))
            {
                Directory.Delete(workDirectory, true);
            }
        }

        [Fact]
        public async Task MainAsyncReturnsZeroForValidDacpac()
        {
            var exitCode = await RunEfcptAsync(dacpacPath, configPath);

            Assert.Equal(0, exitCode);
        }

        [Fact]
        public async Task MainAsyncReturnsOneWhenGenerationFails()
        {
            Assert.Equal(0, await RunEfcptAsync(dacpacPath, configPath));

            await File.WriteAllTextAsync(
                Path.Combine(workDirectory, "CodeTemplates", "EFCore", "DbContext.t4"),
                "<#@ template hostSpecific=\"true\" #>\n<# this is not valid C#; #>\n",
                TestContext.Current.CancellationToken);

            var exitCode = await RunEfcptAsync(dacpacPath, configPath);

            Assert.Equal(1, exitCode);
        }

        [Fact]
        public async Task MainAsyncReturnsOneWhenDacpacIsInvalid()
        {
            var invalidDacpacPath = Path.Combine(workDirectory, "Invalid.dacpac");
            await File.WriteAllTextAsync(invalidDacpacPath, "not a dacpac", TestContext.Current.CancellationToken);

            var exitCode = await RunEfcptAsync(invalidDacpacPath, configPath);

            Assert.Equal(1, exitCode);
        }

        [Fact]
        public async Task MainAsyncReturnsOneWhenConfigFileIsInvalid()
        {
            var invalidConfigPath = Path.Combine(workDirectory, "invalid-config.json");
            await File.WriteAllTextAsync(invalidConfigPath, "{ not json", TestContext.Current.CancellationToken);

            var exitCode = await RunEfcptAsync(dacpacPath, invalidConfigPath);

            Assert.Equal(1, exitCode);
        }

        private async Task<int> RunEfcptAsync(string dacpac, string config)
        {
            Environment.ExitCode = 0;

            var run = Program.MainAsync(new[] { dacpac, "mssql", "-i", config, "-o", workDirectory });
            var completed = await Task.WhenAny(run, Task.Delay(RunTimeout, TestContext.Current.CancellationToken));

            Assert.True(completed == run, $"efcpt did not exit within {RunTimeout.TotalSeconds} seconds.");

            return await run;
        }
    }

    [CollectionDefinition(nameof(CliExitCodeTests), DisableParallelization = true)]
    public sealed class CliExitCodeTestsDefinition
    {
    }
}
