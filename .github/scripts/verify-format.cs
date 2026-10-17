#:property PublishAot=false

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

if (args.Length > 1)
{
    await Console.Error.WriteLineAsync("Usage: dotnet run --file .github/scripts/verify-format.cs -- [solution.slnx]");
    return 2;
}

var solutions = args;
if (solutions.Length == 0)
{
    // File-based apps run from a build cache, so locate the repo using the source file.
    Directory.SetCurrentDirectory(Path.GetFullPath("../..", ScriptDirectory()));
    using var git = StartProcess("git", "ls-files", "-z", "--", "*.slnx");
    var output = git.StandardOutput.ReadToEndAsync();
    var errors = git.StandardError.ReadToEndAsync();
    await git.WaitForExitAsync();
    await Console.Error.WriteAsync(await errors);
    if (git.ExitCode != 0)
    {
        return git.ExitCode;
    }

    solutions = (await output).Split('\0', StringSplitOptions.RemoveEmptyEntries);
}

if (solutions.Length == 0)
{
    await Console.Error.WriteLineAsync("No tracked .slnx solutions found.");
    return 1;
}

var msbuild = OperatingSystem.IsWindows() ? await FindMSBuildAsync() : null;
var result = 0;
foreach (var solution in solutions)
{
    await Console.Out.WriteLineAsync($"\nRestoring: {solution}");
    using var restore = msbuild is null
        ? StartProcess("dotnet", "restore", solution, "-p:Configuration=Debug", "--verbosity", "minimal")
        : StartProcess(msbuild, solution, "/t:Restore", "/p:Configuration=Debug", "/v:minimal", "/nologo");
    var restored = await LogOutputAsync(restore);
    if (restored.ExitCode != 0)
    {
        await Console.Error.WriteLineAsync($"Restore failed for {solution}; formatting verification is incomplete. See the diagnostics above.");
        result = restored.ExitCode;
        continue;
    }

    await Console.Out.WriteLineAsync($"\nVerifying formatting: {solution}");
    using var format = StartProcess("dotnet", "format", solution, "--verify-no-changes", "--no-restore", "--verbosity", "diagnostic");
    var formatted = await LogOutputAsync(format);
    if (formatted.ExitCode != 0)
    {
        result = formatted.ExitCode;
    }
    else if (formatted.MissingReferences)
    {
        // dotnet format can exit successfully after skipping analyzer checks.
        await Console.Error.WriteLineAsync("Formatting verification is incomplete. Check the workspace diagnostics above for missing workloads or targeting packs.");
        result = 1;
    }
}

return result;

static async Task<(int ExitCode, bool MissingReferences)> LogOutputAsync(Process process)
{
    var missingReferences = false;
    await Task.WhenAll(
        ForwardOutputAsync(process.StandardOutput, Console.Out),
        ForwardOutputAsync(process.StandardError, Console.Error),
        process.WaitForExitAsync());
    return (process.ExitCode, missingReferences);

    async Task ForwardOutputAsync(StreamReader reader, TextWriter writer)
    {
        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.Contains("Required references did not load for ", StringComparison.Ordinal))
            {
                missingReferences = true;
            }

            await writer.WriteLineAsync(line);
        }
    }
}

static string ScriptDirectory([CallerFilePath] string path = "") => Path.GetDirectoryName(path)!;

static async Task<string?> FindMSBuildAsync()
{
    // Respect the version selected by a developer shell or setup-msbuild in CI.
    foreach (var directory in (Environment.GetEnvironmentVariable("PATH") ?? string.Empty).Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
    {
        var path = Path.Combine(directory.Trim('"'), "MSBuild.exe");
        if (File.Exists(path))
        {
            return path;
        }
    }

    // Find Visual Studio from a regular shell without requiring a PATH change.
    var vswhere = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft Visual Studio", "Installer", "vswhere.exe");
    if (!File.Exists(vswhere))
    {
        return null;
    }

    using var process = StartProcess(vswhere, "-latest", "-prerelease", "-products", "*", "-requires", "Microsoft.Component.MSBuild", "-find", @"MSBuild\Current\Bin\MSBuild.exe", "-utf8");
    var output = process.StandardOutput.ReadToEndAsync();
    var errors = process.StandardError.ReadToEndAsync();
    await process.WaitForExitAsync();
    await Console.Error.WriteAsync(await errors);
    var msbuild = (await output).Trim();
    return process.ExitCode == 0 && File.Exists(msbuild) ? msbuild : null;
}

static Process StartProcess(string fileName, params string[] arguments)
{
    var startInfo = new ProcessStartInfo(fileName)
    {
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        StandardOutputEncoding = Encoding.UTF8,
        StandardErrorEncoding = Encoding.UTF8,
    };

    startInfo.Environment["DOTNET_CLI_UI_LANGUAGE"] = "en-US";
    foreach (var argument in arguments)
    {
        startInfo.ArgumentList.Add(argument);
    }

    return Process.Start(startInfo)!;
}