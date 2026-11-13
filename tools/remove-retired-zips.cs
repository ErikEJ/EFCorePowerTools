#:property TargetFramework=net10.0
#:property PublishAot=false

using System.Diagnostics;
using System.Text;
using System.Text.Json;

// Run with: dotnet run --file tools/remove-retired-zips.cs -- <source> <new-mirror>
string[] paths = ["efreveng", "efreveng50", "efreveng60", "efreveng70", "efreveng60arm", "efreveng70arm"];
paths = paths.Select(name => $"src/GUI/lib/{name}.exe.zip").ToArray();

if (args.Length == 1 && args[0] is "--help" or "-h")
{
    Usage();
    return 0;
}

if (args.Length < 2 || (args.Length - 2) % 2 != 0)
{
    Usage();
    return 1;
}

try
{
    string? filterRepo = null;
    string python = OperatingSystem.IsWindows() ? "python" : "python3";
    for (int i = 2; i < args.Length; i += 2)
    {
        switch (args[i])
        {
            case "--filter-repo": filterRepo = Path.GetFullPath(args[i + 1]); break;
            case "--python": python = args[i + 1]; break;
            default: throw new ArgumentException($"Unknown option: {args[i]}");
        }
    }

    string destination = Path.GetFullPath(args[1]);
    if (Path.Exists(destination))
    {
        throw new ArgumentException("Destination already exists; choose a new directory.");
    }

    string filterExecutable = filterRepo is null ? "git" : python;
    string[] filterPrefix = filterRepo is null ? ["filter-repo"] : [filterRepo];
    Run(filterExecutable, [.. filterPrefix, "--version"]);
    Run("git", ["clone", "--mirror", "--", args[0], destination]);
    if (Git(destination, "rev-parse", "--is-shallow-repository").Trim() != "false")
    {
        throw new InvalidOperationException("A complete source history is required.");
    }

    var beforeRefs = Refs(destination);
    if (beforeRefs.Count == 0)
    {
        throw new InvalidOperationException("Source repository has no references.");
    }

    var beforeTrees = beforeRefs.Keys.ToDictionary(reference => reference, reference => Tree(destination, reference));
    string beforeSize = Git(destination, "count-objects", "-vH");
    Console.WriteLine(beforeSize);

    // --force is confined to the new mirror. Local hard-linked clones fail
    // filter-repo's freshness check; repacking does not modify source objects.
    // Include every ref explicitly to preserve local remote-tracking names.
    Run(filterExecutable,
        [.. filterPrefix, "--force", "--invert-paths",
         .. paths.SelectMany(path => new[] { "--path", path }), "--refs", .. beforeRefs.Keys], destination);

    var afterRefs = Refs(destination);
    if (!beforeRefs.Keys.ToHashSet(StringComparer.Ordinal).SetEquals(afterRefs.Keys))
    {
        throw new InvalidOperationException("Reference set changed; inspect the mirror before publication.");
    }

    foreach (string reference in afterRefs.Keys)
    {
        if (!beforeTrees[reference].SequenceEqual(Tree(destination, reference)))
        {
            throw new InvalidOperationException($"Unexpected retained-file changes at {reference}");
        }
    }

    if (Git(destination, ["log", "--all", "--format=%H", "--", .. paths]).Trim().Length != 0)
    {
        throw new InvalidOperationException("Retired ZIP paths remain in reachable history.");
    }

    // --refs disables automatic remote removal, reflog expiry, and GC.
    // Remove only remote configuration, retaining rewritten tracking refs.
    Git(destination, "config", "--remove-section", "remote.origin");
    Git(destination, "reflog", "expire", "--expire=now", "--all");
    Run("git", ["-C", destination, "gc", "--prune=now"]);
    Run("git", ["-C", destination, "fsck", "--full"]);
    string afterSize = Git(destination, "count-objects", "-vH");
    string report = Path.Combine(destination, "filter-repo", "cleanup-report.json");
    File.WriteAllText(report, JsonSerializer.Serialize(new
    {
        source = args[0],
        removed_paths = paths,
        before_refs = beforeRefs,
        after_refs = afterRefs,
        before_size = beforeSize,
        after_size = afterSize,
        validation = "All ref tips retain identical non-target files; retired paths absent from history; fsck passed",
    }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
    Console.WriteLine($"{afterSize}\nVerified mirror: {destination}\nReport: {report}\nNothing has been pushed.");
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Cleanup failed: {exception.Message}");
    return 1;
}

static void Usage() => Console.WriteLine(
    "Usage: dotnet run --file tools/remove-retired-zips.cs -- <source> <new-mirror> [--filter-repo <script>] [--python <executable>]");

static byte[] Run(string executable, string[] arguments, string? directory = null, bool capture = false)
{
    var start = new ProcessStartInfo(executable)
    {
        UseShellExecute = false,
        RedirectStandardOutput = capture,
        WorkingDirectory = directory ?? Environment.CurrentDirectory,
    };
    foreach (string argument in arguments)
    {
        start.ArgumentList.Add(argument);
    }

    using var process = Process.Start(start) ?? throw new InvalidOperationException($"Cannot start {executable}");
    using var output = new MemoryStream();
    if (capture)
    {
        process.StandardOutput.BaseStream.CopyTo(output);
    }

    process.WaitForExit();
    if (process.ExitCode != 0)
    {
        throw new InvalidOperationException($"{executable} exited with code {process.ExitCode}.");
    }

    return output.ToArray();
}

static string Git(string repository, params string[] arguments) =>
    Encoding.UTF8.GetString(Run("git", ["-C", repository, .. arguments], capture: true));

static Dictionary<string, string> Refs(string repository) =>
    Git(repository, "for-each-ref", "--format=%(refname) %(objectname)")
        .Split('\n', StringSplitOptions.RemoveEmptyEntries)
        .Select(line => line.TrimEnd('\r').Split(' ', 2))
        .ToDictionary(parts => parts[0], parts => parts[1], StringComparer.Ordinal);

string[] Tree(string repository, string reference)
{
    // Latin1 maps bytes one-to-one, preserving even non-UTF8 Git file names.
    // Include modes and blob IDs, and split on NUL to support tabs/newlines.
    return Encoding.Latin1.GetString(Run("git", ["-C", repository, "ls-tree", "-rz", reference], capture: true))
        .Split('\0', StringSplitOptions.RemoveEmptyEntries)
        .Where(entry => !paths.Contains(entry[(entry.IndexOf('\t') + 1)..], StringComparer.Ordinal))
        .Order(StringComparer.Ordinal)
        .ToArray();
}
