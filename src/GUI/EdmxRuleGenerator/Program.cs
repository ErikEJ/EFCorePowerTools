using System;
using EdmxRuleGenerator.Extensions;

namespace EdmxRuleGenerator;

public static class Program
{
    public static async System.Threading.Tasks.Task<int> Main(string[] args)
    {
        try
        {
            if (!EdmxToRuleProcessor.TryParseArgs(args, out var edmxPath, out var projectBasePath))
            {
                await Console.Out.WriteLineAsync("Error: Invalid command line").ConfigureAwait(false);
                await Console.Out.WriteLineAsync("Valid examples:").ConfigureAwait(false);
                await Console.Out.WriteLineAsync("> EdmxRuleGenerator edmxfilepath <efCoreProjectBasePath>")
                    .ConfigureAwait(false);
                await Console.Out.WriteLineAsync("> EdmxRuleGenerator pathContainingEdmxAndCsProj")
                    .ConfigureAwait(false);
                await Console.Out
                    .WriteLineAsync("> EdmxRuleGenerator . (assuming current directory contains edmx and csproj)")
                    .ConfigureAwait(false);
                return 1;
            }

            var start = DateTimeExtensions.GetTime();
            var edmxProcessor = new EdmxToRuleProcessor(edmxPath, projectBasePath);
            edmxProcessor.TryProcess();
            var elapsed = DateTimeExtensions.GetTime() - start;
            if (edmxProcessor.Errors.Count == 0)
            {
                await Console.Out.WriteLineAsync(
                        $"Successfully generated {edmxProcessor.RulesGeneratedByFile.Count} rule files in {elapsed}ms")
                    .ConfigureAwait(false);
                return 0;
            }

            foreach (var error in edmxProcessor.Errors)
            {
                await Console.Out.WriteLineAsync($"Edmx generated error encountered: {error}").ConfigureAwait(false);
            }

            return edmxProcessor.Errors.Count;
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
