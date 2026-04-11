using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    [NonParallelizable]
    public class CliDacpacSmokeTests
    {
        [Test]
        public async Task Efcpt8DockerPlaygroundDacpacSmokeTestCompletes()
        {
            var workingDirectory = CreateWorkingDirectory("efcpt-dockerplayground8-");
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt8CliPath(), GetDockerPlaygroundDacpacPath(), workingDirectory);

                Assert.That(stdout, Does.Contain("Getting database objects..."));
                Assert.That(stdout, Does.Contain("database objects discovered"));
                Assert.That(stdout, Does.Contain("files generated"));
            }
            finally
            {
                CleanupWorkingDirectory(workingDirectory);
            }
        }

        [Test]
        public async Task Efcpt10DockerPlaygroundDacpacSmokeTestCompletes()
        {
            var workingDirectory = CreateWorkingDirectory("efcpt-dockerplayground10-");
            try
            {
                var stdout = await RunEfcptCliAsync(GetEfcpt10CliPath(), GetDockerPlaygroundDacpacPath(), workingDirectory);

                Assert.That(stdout, Does.Contain("Getting database objects..."));
                Assert.That(stdout, Does.Contain("database objects discovered"));
                Assert.That(stdout, Does.Contain("files generated"));
            }
            finally
            {
                CleanupWorkingDirectory(workingDirectory);
            }
        }

        private static string GetDockerPlaygroundDacpacPath()
        {
            var dacpacPath = Path.Combine(
                GetRepositoryRoot(),
                "test",
                "ScaffoldingTester",
                "DockerPlayground",
                "bin",
                "Debug",
                "net10.0",
                "DockerPlayground.dacpac");

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

        private static string CreateWorkingDirectory(string prefix)
        {
            var workingDirectory = Path.Combine(Path.GetTempPath(), prefix + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(workingDirectory);
            return workingDirectory;
        }

        private static void CleanupWorkingDirectory(string workingDirectory)
        {
            if (Directory.Exists(workingDirectory))
            {
                Directory.Delete(workingDirectory, recursive: true);
            }
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
    }
}
