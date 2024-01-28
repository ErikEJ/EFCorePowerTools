using RevEng.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

[assembly: CLSCompliant(false)]

namespace RevEng.Core
{
    public static class DbContextSplitter
    {
        // Adapted from https://github.com/lauxjpn/DbContextOnModelCreatingSplitter
        public static List<string> Split(string dbContextPath, string configNamespace, bool supportNullable, string dbContextName)
        {
            ArgumentNullException.ThrowIfNull(dbContextPath);

            var dbContextFilePath = Path.GetFullPath(dbContextPath);

            var configurationsDirectoryPath = Path.Combine(Path.GetDirectoryName(dbContextFilePath) ?? string.Empty, "Configurations");

            Directory.CreateDirectory(configurationsDirectoryPath);

            var source = File.ReadAllText(dbContextFilePath, Encoding.UTF8);

            var configBlocks = GetConfigurationBlocks(File.ReadAllLines(dbContextFilePath, Encoding.UTF8));

            if (configBlocks.Count == 0)
            {
                return new List<string>();
            }

            var contextNamespace = GetContextNamespace(source);

            var configurationNamespace = configNamespace ?? contextNamespace;

            configurationNamespace = configurationNamespace + ".Configurations";

            var contextUsingStatements = Regex.Matches(source, @"^using\s+.*?;", RegexOptions.Multiline | RegexOptions.Singleline, TimeSpan.FromSeconds(5))
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .OrderBy(m => m)
                .ToList();

            if (configurationNamespace != contextNamespace)
            {
                contextUsingStatements.Add($"using {contextNamespace};");
            }

            contextUsingStatements.Add("using Microsoft.EntityFrameworkCore.Metadata.Builders;");

            contextUsingStatements = contextUsingStatements
                .Distinct()
                .OrderBy(m => m)
                .ToList();

            const string statementsInnerBlockPattern = @"(?<=modelBuilder\.Entity<(?<EntityName>.*?)>\((?<EntityParameterName>.*?)\s*=>\s*\{).*?(?=\s{2,}\}\);)";

            var statementsBlockMatches = Regex.Matches(source, statementsInnerBlockPattern, RegexOptions.Multiline | RegexOptions.Singleline)
                .Cast<Match>()
                .Where(m => !m.Value.StartsWith(
                    @"
                            ",
                    StringComparison.OrdinalIgnoreCase))
                .Where(m => m.Value.Trim().StartsWith("entity", StringComparison.Ordinal))
                .ToList();

            if (statementsBlockMatches.Count == 0)
            {
                return new List<string>();
            }

            if (statementsBlockMatches.Count != configBlocks.Count)
            {
                return new List<string>();
            }

            var result = new List<string>();
            var configurationLines = new List<string>();
            var index = 0;

            foreach (var blockMatch in statementsBlockMatches)
            {
                var entityName = blockMatch.Groups["EntityName"].Value;
                var entityParameterName = blockMatch.Groups["EntityParameterName"].Value;
#if !CORE80
                var statements = configBlocks[index].Replace(new string(' ', 16), new string(' ', 12), StringComparison.OrdinalIgnoreCase);
#else
                var statements = configBlocks[index];
#endif
                var sb = new StringBuilder();
                sb.AppendLine(PathHelper.Header);
                sb.AppendLine(string.Join(Environment.NewLine, contextUsingStatements));
                if (supportNullable)
                {
                    sb.AppendLine();
                    sb.AppendLine("#nullable disable");
                }

                sb.AppendLine();
#pragma warning disable CA1305 // Specify IFormatProvider
                sb.AppendLine($"namespace {configurationNamespace}");
#pragma warning restore CA1305 // Specify IFormatProvider
                sb.AppendLine("{");
                sb.AppendLine(new string(' ', 4) + $"public partial class {entityName}Configuration : IEntityTypeConfiguration<{entityName}>");
                sb.AppendLine(new string(' ', 4) + "{");
                sb.AppendLine(new string(' ', 8) + $"public void Configure(EntityTypeBuilder<{entityName}> {entityParameterName})");
                sb.AppendLine(new string(' ', 8) + "{");
                sb.AppendLine(new string(' ', 12) + statements);
                sb.AppendLine(new string(' ', 12) + "OnConfigurePartial(entity);");
                sb.AppendLine(new string(' ', 8) + "}");
                sb.AppendLine();
                sb.AppendLine(new string(' ', 8) + $"partial void OnConfigurePartial(EntityTypeBuilder<{entityName}> entity);");
                sb.AppendLine(new string(' ', 4) + "}");
                sb.AppendLine("}");

                var configurationContents = sb.ToString();

                var configurationFilePath = Path.Combine(configurationsDirectoryPath, $"{entityName}Configuration.cs");

                ReverseEngineerRunner.RetryFileWrite(configurationFilePath, configurationContents);

                result.Add(configurationFilePath);

                configurationLines.Add($"{new string(' ', 8)}modelBuilder.ApplyConfiguration(new Configurations.{entityName}Configuration());");

                index++;
            }

            var finalSource = BuildDbContext(configurationNamespace, configurationLines, File.ReadAllLines(dbContextFilePath, Encoding.UTF8));

            ReverseEngineerRunner.RetryFileWrite(dbContextFilePath, finalSource);

            return result;
        }

        private static string GetContextNamespace(string source)
        {
            var ns = Regex.Match(source, @"(?<=(?:^|\s|;)namespace\s+).*?(?=(?:\s|\{))", RegexOptions.Multiline | RegexOptions.Singleline, TimeSpan.FromSeconds(5)).Value;

            if (ns?.EndsWith(';') ?? false)
            {
                return ns.Remove(ns.Length - 1);
            }

            return ns;
        }

        private static List<string> BuildDbContext(string configurationNamespace, List<string> configurationLines, string[] sourceLines)
        {
            var finalSource = new List<string>();
            var usings = new List<string>();
            var inEntityBuilder = false;
            var configLinesWritten = false;
            string prevLine = null;
            string spaces = null;

            foreach (var line in sourceLines)
            {
                if (prevLine != null && string.IsNullOrEmpty(prevLine) && string.IsNullOrEmpty(line.Trim()))
                {
                    continue;
                }

                if (line.Trim().StartsWith("modelBuilder.Entity<", StringComparison.InvariantCulture))
                {
                    spaces = line.Substring(0, line.IndexOf("modelBuilder.Entity<", StringComparison.OrdinalIgnoreCase));
                    inEntityBuilder = true;
                    if (!configLinesWritten)
                    {
                        finalSource.AddRange(configurationLines);
                        configLinesWritten = true;
                    }

                    continue;
                }

                if (line.StartsWith("using ", StringComparison.InvariantCulture))
                {
                    usings.Add(line);
                    continue;
                }

                if (line.StartsWith(spaces + "});", StringComparison.OrdinalIgnoreCase) && inEntityBuilder)
                {
                    inEntityBuilder = false;
                    continue;
                }

                if (inEntityBuilder)
                {
                    continue;
                }

                prevLine = line.Trim();

                finalSource.Add(line);
            }

            usings.Add($"using {configurationNamespace};");

            usings.Sort();

            finalSource.InsertRange(1, usings);
            return finalSource;
        }

        private static List<string> GetConfigurationBlocks(string[] sourceLines)
        {
            var finalSections = new List<string>();
            var inEntityBuilder = false;
            string spaces = null;
            var section = new StringBuilder();

            foreach (var line in sourceLines)
            {
                if (line.Trim().StartsWith("modelBuilder.Entity<", StringComparison.InvariantCulture))
                {
                    spaces = line.Substring(0, line.IndexOf("modelBuilder.Entity<", StringComparison.OrdinalIgnoreCase));
                    inEntityBuilder = true;
                    continue;
                }

                if (line.StartsWith(spaces + "});", StringComparison.OrdinalIgnoreCase) && inEntityBuilder)
                {
                    if (!string.IsNullOrWhiteSpace(section.ToString()))
                    {
                        finalSections.Add(section.ToString().TrimStart());
                        section.Clear();
                    }

                    inEntityBuilder = false;
                    continue;
                }

                if (inEntityBuilder)
                {
                    if (line.StartsWith(spaces + "{", StringComparison.OrdinalIgnoreCase))
                    {
                        section.AppendLine();
                        continue;
                    }

                    section.AppendLine(line);
                }
            }

            if (!string.IsNullOrWhiteSpace(section.ToString()))
            {
                finalSections.Add(section.ToString());
            }

            return finalSections;
        }
    }
}
