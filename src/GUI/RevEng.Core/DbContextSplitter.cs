using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RevEng.Core
{
    public static class DbContextSplitter
    {
        // Adapted from https://github.com/lauxjpn/DbContextOnModelCreatingSplitter
        public static List<string> Split(string dbContextPath, string configNamespace, bool supportNullable)
        {
            var dbContextFilePath = Path.GetFullPath(dbContextPath);

            var configurationsDirectoryPath = Path.Combine(Path.GetDirectoryName(dbContextFilePath), "Configurations");

            Directory.CreateDirectory(configurationsDirectoryPath);

            var source = File.ReadAllText(dbContextFilePath, Encoding.UTF8);

            var contextNamespace = Regex.Match(source, @"(?<=(?:^|\s|;)namespace\s+).*?(?=(?:\s|\{))", RegexOptions.Multiline | RegexOptions.Singleline).Value;

            var configurationNamespace = configNamespace ?? contextNamespace;

            configurationNamespace = configurationNamespace + ".Configurations";

            var contextUsingStatements = Regex.Matches(source, @"^using\s+.*?;", RegexOptions.Multiline | RegexOptions.Singleline)
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

                var _sb = new StringBuilder();
                _sb.AppendLine(PathHelper.Header);
                _sb.AppendLine(string.Join(Environment.NewLine, contextUsingStatements));
                _sb.AppendLine();
                if (supportNullable)
                {
                    _sb.AppendLine("#nullable disable");
                }
                _sb.AppendLine();
                _sb.AppendLine($"namespace {configurationNamespace}");
                _sb.AppendLine("{");
                _sb.AppendLine(new string(' ', 4) + $"public partial class {entityName}Configuration : IEntityTypeConfiguration<{entityName}>");
                _sb.AppendLine(new string(' ', 4) + "{");
                _sb.AppendLine(new string(' ', 8) + $"public void Configure(EntityTypeBuilder<{entityName}> {entityParameterName})");
                _sb.AppendLine(new string(' ', 8) + "{");
                _sb.AppendLine(new string(' ', 12) + statements);
                _sb.AppendLine();
                _sb.AppendLine(new string(' ', 12) + "OnConfigurePartial(entity);");
                _sb.AppendLine(new string(' ', 8) + "}");
                _sb.AppendLine();
                _sb.AppendLine(new string(' ', 8) + $"partial void OnConfigurePartial(EntityTypeBuilder<{entityName}> entity);");
                _sb.AppendLine(new string(' ', 4) + "}");
                _sb.AppendLine("}");

                var configurationContents = _sb.ToString();

                var configurationFilePath = Path.Combine(configurationsDirectoryPath, $"{entityName}Configuration.cs");

                File.WriteAllText(configurationFilePath, configurationContents, Encoding.UTF8);

                result.Add(configurationFilePath);

                configurationLines.Add($"{new string(' ', 12)}modelBuilder.ApplyConfiguration(new Configurations.{entityName}Configuration());");
            }

            var finalSource = BuildDbContext(configurationNamespace, configurationLines, File.ReadAllLines(dbContextFilePath, Encoding.UTF8));

            File.WriteAllLines(dbContextFilePath, finalSource, Encoding.UTF8);

            return result;
        }

        private static List<string> BuildDbContext(string configurationNamespace, List<string> configurationLines, string[] sourceLines)
        {
            var finalSource = new List<string>();
            var usings = new List<string>();
            var inEntityBuilder = false;
            var configLinesWritten = false;
            string prevLine = null;

            foreach (var line in sourceLines)
            {
                if (prevLine != null && prevLine == string.Empty && line.Trim() == string.Empty)
                {
                    continue;
                }

                if (line.Trim().StartsWith("modelBuilder.Entity<", StringComparison.InvariantCulture))
                {
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

            usings.Add($"using {configurationNamespace};");

            usings.Sort();

            finalSource.InsertRange(1, usings);
            return finalSource;
        }
    }
}
