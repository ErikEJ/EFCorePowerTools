using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RevEng.Core
{
    public class ReplacingCandidateNamingService : CandidateNamingService
    {
        private readonly List<Schema> _customNameOptions;
        private readonly List<TableRenamer> _navigationReplacers;

        public ReplacingCandidateNamingService(List<Schema> customNameOptions)
        {
            _customNameOptions = customNameOptions;
            _navigationReplacers = customNameOptions
                .Where(o => o.Tables != null)
                .SelectMany(t => t.Tables)
                .ToList();
        }

        public override string GenerateCandidateIdentifier(DatabaseTable originalTable)
        {
            if (originalTable == null)
            {
                throw new ArgumentException("Argument is empty", nameof(originalTable));
            }

            var candidateStringBuilder = new StringBuilder();

            var schema = GetSchema(originalTable.Schema);

            if (schema == null)
            {
                return base.GenerateCandidateIdentifier(originalTable);
            }

            if (schema.UseSchemaName)
            {
                candidateStringBuilder.Append(ToPascalCase(originalTable.Schema));
                candidateStringBuilder.Append(ToPascalCase(originalTable.Name));

                return candidateStringBuilder.ToString();
            }

            var newTableName = string.Empty;

            if (schema.Tables != null && schema.Tables.Any(t => t.Name == originalTable.Name))
            {
                newTableName = schema.Tables.SingleOrDefault(t => t.Name == originalTable.Name)?.NewName;
            }
            else if (!string.IsNullOrEmpty(schema.TableRegexPattern) && schema.TablePatternReplaceWith != null)
            {
                newTableName = RegexNameReplace(schema.TableRegexPattern, originalTable.Name, schema.TablePatternReplaceWith);
            }
            else
            {
                return base.GenerateCandidateIdentifier(originalTable);
            }

            if (string.IsNullOrWhiteSpace(newTableName))
            {
                candidateStringBuilder.Append(ToPascalCase(originalTable.Name));

                return candidateStringBuilder.ToString();
            }

            candidateStringBuilder.Append(newTableName);

            return candidateStringBuilder.ToString();
        }

        public override string GenerateCandidateIdentifier(DatabaseColumn originalColumn)
        {
            string temp = string.Empty;
            var candidateStringBuilder = new StringBuilder();

            var schema = GetSchema(originalColumn.Table.Schema);

            if (schema == null || schema.Tables == null)
            {
                return base.GenerateCandidateIdentifier(originalColumn);
            }

            var renamers = schema.Tables
                .Where(t => t.Name == originalColumn.Table.Name && t.Columns != null)
                .Select(t => t)
                .ToList();

            if (renamers.Count > 0)
            {
                var column = renamers
                    .SelectMany(c => c.Columns.Where(n => n.Name == originalColumn.Name))
                    .FirstOrDefault();

                if (column != null)
                {
                    candidateStringBuilder.Append(column.NewName);
                    return candidateStringBuilder.ToString();
                }
            }

            if (!string.IsNullOrEmpty(schema.ColumnRegexPattern) && schema.ColumnPatternReplaceWith != null)
            {
                string newColumnName = RegexNameReplace(schema.ColumnRegexPattern, originalColumn.Name, schema.ColumnPatternReplaceWith);

                if (!string.IsNullOrWhiteSpace(newColumnName))
                {
                    candidateStringBuilder.Append(newColumnName);
                    return candidateStringBuilder.ToString();
                }
            }

            return base.GenerateCandidateIdentifier(originalColumn);
        }

        public override string GetPrincipalEndCandidateNavigationPropertyName(IForeignKey foreignKey, string dependentEndNavigationPropertyName)
        {
            var baseName = base.GetPrincipalEndCandidateNavigationPropertyName(foreignKey, dependentEndNavigationPropertyName);
            var tableName = foreignKey.PrincipalEntityType.GetTableName();
            var schemaName = foreignKey.PrincipalEntityType.GetSchema();

            var schema = _customNameOptions
                .Where(o => o.SchemaName == schemaName
                    && o.Tables != null && o.Tables.Any())
                .SingleOrDefault();

            if (schema != null)
            {
                var table = schema.Tables
                    .Where(t => t.Name == tableName
                        && t.Navigations != null)
                    .SingleOrDefault();

                if (table != null)
                {
                    var navigationRenamer = table.Navigations
                        .Where(n => n.Name == baseName)
                        .SingleOrDefault();

                    if (navigationRenamer != null && navigationRenamer.NewName != null)
                    {
                        return navigationRenamer.NewName;
                    }
                }
            }

            return baseName;
        }

        public override string GetDependentEndCandidateNavigationPropertyName(IForeignKey foreignKey)
        {
            var baseName = base.GetDependentEndCandidateNavigationPropertyName(foreignKey);

            var tableName = foreignKey.DeclaringEntityType.GetTableName();
            var schemaName = foreignKey.DeclaringEntityType.GetSchema();

            var schema = _customNameOptions
                .Where(o => o.SchemaName == schemaName
                    && o.Tables != null && o.Tables.Any())
                .SingleOrDefault();

            if (schema != null)
            {
                var table = schema.Tables
                    .Where(t => t.Name == tableName
                        && t.Navigations != null)
                    .SingleOrDefault();

                if (table != null)
                {
                    var navigationRenamer = table.Navigations
                        .Where(n => n.Name == baseName)
                        .SingleOrDefault();

                    if (navigationRenamer != null && navigationRenamer.NewName != null)
                    {
                        return navigationRenamer.NewName;
                    }
                }
            }

            return baseName;
        }

        private Schema GetSchema(string originalSchema)
            => _customNameOptions?
                    .FirstOrDefault(x => x.SchemaName == originalSchema);

        private static string ToPascalCase(string value)
        {
            var candidateStringBuilder = new StringBuilder();
            var previousLetterCharInWordIsLowerCase = false;
            var isFirstCharacterInWord = true;

            foreach (var c in value)
            {
                var isNotLetterOrDigit = !char.IsLetterOrDigit(c);
                if (isNotLetterOrDigit
                    || (previousLetterCharInWordIsLowerCase && char.IsUpper(c)))
                {
                    isFirstCharacterInWord = true;
                    previousLetterCharInWordIsLowerCase = false;
                    if (isNotLetterOrDigit)
                    {
                        continue;
                    }
                }

                candidateStringBuilder.Append(
                    isFirstCharacterInWord ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
                isFirstCharacterInWord = false;
                if (char.IsLower(c))
                {
                    previousLetterCharInWordIsLowerCase = true;
                }
            }

            return candidateStringBuilder.ToString();
        }

        private static string RegexNameReplace(string pattern, string originalName, string replacement, int timeout = 100)
        {
            string newName = string.Empty;

            try
            {
                newName = Regex.Replace(originalName, pattern, replacement, RegexOptions.None, TimeSpan.FromMilliseconds(timeout));
            }
            catch (RegexMatchTimeoutException)
            {
                Console.WriteLine($"Regex pattern {pattern} time out when tryign to match {originalName}, name won't be replaced");
            }

            return newName;
        }
    }
}
