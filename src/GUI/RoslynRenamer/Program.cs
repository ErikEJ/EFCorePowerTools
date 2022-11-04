using System;

namespace RoslynRenamer
{
    public static class Program
    {
        public static async System.Threading.Tasks.Task<int> Main(string[] args)
        {
            try
            {
                if (!RoslynEntityPropertyRenamer.TryParseArgs(args, out var projectBasePath))
                {
                    await Console.Out.WriteLineAsync("Error: Invalid command line").ConfigureAwait(false);
                    await Console.Out.WriteLineAsync("Valid examples:").ConfigureAwait(false);
                    await Console.Out.WriteLineAsync("> RoslynRenamer pathContainingRulesAndCsProj")
                        .ConfigureAwait(false);
                    await Console.Out
                        .WriteLineAsync("> RoslynRenamer . (assuming current directory contains rules and csproj)")
                        .ConfigureAwait(false);
                    return 1;
                }

                await RoslynEntityPropertyRenamer.ApplyRenamingRulesAsync(projectBasePath);
                return 0;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Unexpected error: {ex.Message}").ConfigureAwait(false);
                return 1;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
