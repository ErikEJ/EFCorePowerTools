using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Text;

namespace ReverseEngineer20.ReverseEngineer
{
    public class ReplacingCandidateNamingService : CandidateNamingService, IEnhanceCandidateNamingService
    {
        private readonly CustomerOptionSelector _customNameOptions;

        public ReplacingCandidateNamingService(CustomerOptionSelector customNameOptions)
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

            candidateStringBuilder.Append(AddSchemaName(originalTable));

            if (schema?.HasCustomTableName(originalTable.Name) ?? false)
                candidateStringBuilder.Append(schema.GetNewTableName(originalTable.Name));
            else
                candidateStringBuilder.Append(ToPascalCase(originalTable.Name));

            return candidateStringBuilder.ToString();
        }

        public override string GenerateCandidateIdentifier(DatabaseColumn originalColumn)
        {
            var schema = originalColumn.Table.Schema;
            var table = originalColumn.Table.Name;
            var column = originalColumn.Name;

            var result = _customNameOptions.GetCustomColumnName(schema, table, column);

            if (string.IsNullOrWhiteSpace(result))
                return base.GenerateCandidateIdentifier(originalColumn);
            else
                return result;
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
                var test = base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
                return test;
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
                var test = base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
                return test;
            }
        }

        private SchemaSelector GetSchema(string originalSchema)
            => _customNameOptions.GetSchema(originalSchema);

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

        public string GenerateDbSetCandidateIdentifier(DatabaseTable originalTable)
        {
            if (originalTable == null)
            {
                throw new ArgumentException("Argument is empty", nameof(originalTable));
            }

            var candidateStringBuilder = new StringBuilder();

            var schema = GetSchema(originalTable.Schema);

            candidateStringBuilder.Append(AddSchemaName(originalTable));

            if (schema.HasCustomVariableName(originalTable.Name))
                candidateStringBuilder.Append(schema.GetVariableName(originalTable.Name));
            else
                candidateStringBuilder.Append(ToPascalCase(originalTable.Name));

            return candidateStringBuilder.ToString();
        }

        public string AddSchemaName(DatabaseTable originalTable)
        {
            var schema = GetSchema(originalTable.Schema);

            if (schema?.UseSchemaName ?? false)
                return ToPascalCase(originalTable.Schema);
            return null;
        }
    }
}
