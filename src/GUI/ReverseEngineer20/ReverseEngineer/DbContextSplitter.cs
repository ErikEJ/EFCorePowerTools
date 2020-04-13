using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReverseEngineer20.ReverseEngineer
{
    public static class DbContextSplitter
    {
        // Adapted from https://github.com/lauxjpn/DbContextOnModelCreatingSplitter
        public static List<string> Split(string dbContextPath, string configNamespace)
        {
            var dbContextFilePath = Path.GetFullPath(dbContextPath);

            var configurationsDirectoryPath = Path.GetDirectoryName(dbContextFilePath);

            var source = File.ReadAllText(dbContextFilePath, Encoding.UTF8);

            var contextUsingStatements = Regex.Matches(source, @"^using\s+.*?;", RegexOptions.Multiline | RegexOptions.Singleline)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            var contextNamespace = Regex.Match(source, @"(?<=(?:^|\s|;)namespace\s+).*?(?=(?:\s|\{))", RegexOptions.Multiline | RegexOptions.Singleline).Value;

            var configurationNamespace = configNamespace ?? contextNamespace;

            const string statementsInnerBlockPattern = @"(?<=modelBuilder\.Entity<(?<EntityName>.*?)>\((?<EntityParameterName>.*?)\s*=>\s*\{).*?(?=\s{2,}\}\);)";

            var statementsBlockMatches = Regex.Matches(source, statementsInnerBlockPattern, RegexOptions.Multiline | RegexOptions.Singleline)
                .Cast<Match>()
                .ToList();

            if (!statementsBlockMatches.Any())
            {
                return new List<string>();
            }

            var result = new List<string>();
            var configurationLines = new List<string>();

            foreach (var blockMatch in statementsBlockMatches)
            {
                var entityName = blockMatch.Groups["EntityName"].Value;
                var entityParameterName = blockMatch.Groups["EntityParameterName"].Value;
                var statements = Regex.Replace(blockMatch.Value, @"^\t+", new string(' ', 4), RegexOptions.Multiline)
                    .TrimStart('\r', '\n', '\t', ' ')
                    .Replace(new string(' ', 16), new string(' ', 12));

                var configuration = new StringBuilder();
                configuration.AppendLine(PathHelper.Header);
                configuration.AppendLine(string.Join(Environment.NewLine, contextUsingStatements));
                configuration.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
                if (configurationNamespace != contextNamespace)
                {
                    configuration.AppendLine($"using {contextNamespace};");
                }
                configuration.AppendLine();
                configuration.AppendLine($"namespace {configurationNamespace}");
                configuration.AppendLine("{");
                configuration.AppendLine(new string(' ', 4) + $"public class {entityName}Configuration : IEntityTypeConfiguration<{entityName}>");
                configuration.AppendLine(new string(' ', 4) + "{");
                configuration.AppendLine(new string(' ', 8) + $"public void Configure(EntityTypeBuilder<{entityName}> {entityParameterName})");
                configuration.AppendLine(new string(' ', 8) + "{");
                configuration.AppendLine(new string(' ', 12) + statements);
                configuration.AppendLine(new string(' ', 8) + "}");
                configuration.AppendLine(new string(' ', 4) + "}");
                configuration.AppendLine("}");

                var configurationContents = configuration.ToString();

                var configurationFilePath = Path.Combine(configurationsDirectoryPath, $"{entityName}Configuration.cs");

                File.WriteAllText(configurationFilePath, configurationContents, Encoding.UTF8);

                result.Add(configurationFilePath);

                configurationLines.Add( $"{new string(' ', 12)}modelBuilder.ApplyConfiguration(new {entityName}Configuration());");
            }

            configurationLines.Add(string.Empty);

            var sourceLines = File.ReadAllLines(dbContextFilePath, Encoding.UTF8);

            var finalSource = new List<string>();
            var inEntityBuilder = false;
            var configLinesWritten = false;
            string prevLine = null;

            foreach (var line in sourceLines)
            {
                if (prevLine != null && prevLine == string.Empty && line.Trim() == string.Empty)
                {
                    continue;
                }

                if (line.Trim().StartsWith("modelBuilder.Entity<"))
                {
                    inEntityBuilder = true;
                    if (!configLinesWritten)
                    {
                        finalSource.AddRange(configurationLines);
                        configLinesWritten = true;
                    }
                    continue;
                }

                if (line.Trim().StartsWith("});") && inEntityBuilder)
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

            File.WriteAllLines(dbContextFilePath, finalSource, Encoding.UTF8);

            return result;
        }
    }
}
