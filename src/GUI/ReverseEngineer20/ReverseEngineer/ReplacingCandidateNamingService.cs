using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReverseEngineer20.ReverseEngineer
{
    public class ReplacingCandidateNamingService: CandidateNamingService
    {
        private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

        public ReplacingCandidateNamingService(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger) : base()
        {
            _logger = logger;
        }

        public override string GenerateCandidateIdentifier(DatabaseTable originalTable)
        {
            return GenerateCandidateIdentifier(originalTable.Name, originalTable.Schema);
        }

        private static string GenerateCandidateIdentifier(string originalIdentifier, string tableSchema)
        {
            
            if (string.IsNullOrWhiteSpace(originalIdentifier))
            {
                throw new ArgumentException("Argument is empty", nameof(originalIdentifier));
            }

            if (string.IsNullOrWhiteSpace(tableSchema))
            {
                throw new ArgumentException("Argument is empty", nameof(tableSchema));
            }

            var candidateStringBuilder = new StringBuilder();

            candidateStringBuilder.Append(ToPascalCase(tableSchema));
            candidateStringBuilder.Append(ToPascalCase(originalIdentifier));


            return candidateStringBuilder.ToString();
        }

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
