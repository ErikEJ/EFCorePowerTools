﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReverseEngineer20.ReverseEngineer
{
    public class ReplacingCandidateNamingService : CandidateNamingService
    {
        private readonly List<Schema> _customNameOptions;

        public ReplacingCandidateNamingService(List<Schema> customNameOptions)
        {
            _customNameOptions = customNameOptions;
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
            else if (schema.Tables.Count > 0)
            {
                var newTableName = _customNameOptions
                    .FirstOrDefault(x => x.SchemaName == schema.SchemaName)
                    .Tables?
                    .FirstOrDefault(t => t.Name == originalTable.Name)?.NewName;

                if (string.IsNullOrWhiteSpace(newTableName))
                {
                    candidateStringBuilder.Append(ToPascalCase(originalTable.Name));

                    return candidateStringBuilder.ToString();
                }

                candidateStringBuilder.Append(newTableName);

                return candidateStringBuilder.ToString();
            }
            else
            {
                return base.GenerateCandidateIdentifier(originalTable);
            }
        }

        public override string GenerateCandidateIdentifier(DatabaseColumn originalColumn)
        {
            string temp = string.Empty;
            var candidateStringBuilder = new StringBuilder();

            var schema = GetSchema(originalColumn.Table.Schema);

            if (schema == null)
            {
                return base.GenerateCandidateIdentifier(originalColumn);
            }

            if (schema.Tables == null)
            {
                return base.GenerateCandidateIdentifier(originalColumn);
            }

            var columns = _customNameOptions
                .FirstOrDefault(s => s.SchemaName == schema.SchemaName)?
                .Tables?
                .FirstOrDefault(t => t.Name == originalColumn.Table.Name)?
                .Columns?
                .FirstOrDefault(c => c.Name == originalColumn.Name);


            if (columns != null)
            {
                candidateStringBuilder.Append(columns.NewName);
                return candidateStringBuilder.ToString();
            }
            else
            {
                return base.GenerateCandidateIdentifier(originalColumn);
            }
        }

        public override string GetDependentEndCandidateNavigationPropertyName(IForeignKey foreignKey)
        {
            var baseName = base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
            var originalSchema = foreignKey.PrincipalEntityType.Scaffolding().Schema;
            //var originalTable = foreignKey.DeclaringEntityType.Scaffolding().TableName;
            var schema = GetSchema(originalSchema);

            if (schema == null)
            {
                return base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
            }
            else if (foreignKey.IsSelfReferencing())
            {
                return base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
            }
            else if (schema.SchemaName == originalSchema)
            {
                if (schema.UseSchemaName)
                {
                    return ToPascalCase(schema.SchemaName) + baseName;
                }
                return baseName;
            }
            else
            {
                return base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
            }
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
    }
}
