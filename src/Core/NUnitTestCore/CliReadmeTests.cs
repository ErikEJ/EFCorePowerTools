using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ErikEJ.EFCorePowerTools;
using Xunit;

namespace NUnitTestCore
{
    // Program.MainAsync uses the process-wide Environment.ExitCode, so these tests share the non-parallel CLI collection.
    [Collection(nameof(CliExitCodeTests))]
    public sealed class CliReadmeTests : IDisposable
    {
        private static readonly TimeSpan RunTimeout = TimeSpan.FromSeconds(60);

        private readonly string workDirectory;
        private readonly string dacpacPath;
        private readonly string configPath;
        private readonly string readmePath;

        public CliReadmeTests()
        {
            workDirectory = Path.Combine(Path.GetTempPath(), "EFCorePowerTools.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(workDirectory);

            dacpacPath = Path.Combine(workDirectory, "abc.dacpac");
            File.Copy(Path.Combine(AppContext.BaseDirectory, "Dacpac", "abc.dacpac"), dacpacPath);

            configPath = Path.Combine(workDirectory, "efcpt-config.json");
            File.WriteAllText(
                configPath,
                @"{
  ""code-generation"": { ""refresh-object-lists"": true },
  ""names"": { ""dbcontext-name"": ""TestContext"", ""root-namespace"": ""TestNamespace"" }
}");

            readmePath = Path.Combine(workDirectory, "efcpt-readme.md");

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
        public async Task MainAsyncWritesReadmeByDefault()
        {
            var exitCode = await RunEfcptAsync();

            Assert.Equal(0, exitCode);
            Assert.True(File.Exists(readmePath));
        }

        private async Task<int> RunEfcptAsync(params string[] extraArgs)
        {
            Environment.ExitCode = 0;

            var args = new[] { dacpacPath, "mssql", "-i", configPath, "-o", workDirectory }.Concat(extraArgs).ToArray();
            var run = Program.MainAsync(args);
            var completed = await Task.WhenAny(run, Task.Delay(RunTimeout, TestContext.Current.CancellationToken));

            Assert.True(completed == run, $"efcpt did not exit within {RunTimeout.TotalSeconds} seconds.");

            return await run;
        }
    }
}
