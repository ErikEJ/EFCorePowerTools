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
            else if(schema.DatabaseCustomNameOptions.Count > 0)
            {
                var newTableName = _customNameOptions.Where(x => x.SchemaName == schema.SchemaName)
                    .Select(x => x.DatabaseCustomNameOptions)
                    .Select(x => x.Where(t => t.OldTableName == originalTable.Name)
                    .Select(table => table.NewTableName).FirstOrDefault())
                    .FirstOrDefault();

                if(string.IsNullOrWhiteSpace(newTableName))
                {
                    candidateStringBuilder.Append(originalTable.Name);

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

            if(schema == null)
            {
                return base.GenerateCandidateIdentifier(originalColumn);
            }

            if(schema.DatabaseCustomNameOptions == null)
            {
                return base.GenerateCandidateIdentifier(originalColumn);
            }

            var columns = _customNameOptions
                .Where(x => x.SchemaName == schema.SchemaName)
                .Select(x => x.DatabaseCustomNameOptions)
                .Select(x => x.Where(table => table.OldTableName == originalColumn.Table.Name)
                .FirstOrDefault()).FirstOrDefault()?
                .Columns
                .Where(x => x.OldColumnName == originalColumn.Name).FirstOrDefault();
                

            if(columns != null)
            {
                candidateStringBuilder.Append(columns.NewColumnName);
                return candidateStringBuilder.ToString();
            }
            else
            {
                return base.GenerateCandidateIdentifier(originalColumn);
            }

            //foreach (var schemas in _customNameOptions)
            //{
            //    if(schemas.SchemaName == originalColumn.Table.Schema)
            //    {
            //        foreach (var tables in schemas.DatabaseCustomNameOptions)
            //        {
            //            foreach (var item in tables.Columns)
            //            {
            //                if (item.OldColumnName == originalColumn.Name)
            //                {
            //                    temp = item.NewColumnName;
            //                }
            //                else
            //                {
            //                    return base.GenerateCandidateIdentifier(originalColumn);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return base.GenerateCandidateIdentifier(originalColumn);
            //    }
            //}

            //candidateStringBuilder.Append(temp);

            //return candidateStringBuilder.ToString();
        }

        private Schema GetSchema(string originalSchema)
            =>_customNameOptions
                  .Where(x => x.SchemaName.Contains(originalSchema))
                  .Select(x =>
                      new Schema
                      {
                          SchemaName = x.SchemaName,
                          UseSchemaName = x.UseSchemaName,
                          DatabaseCustomNameOptions = x.DatabaseCustomNameOptions

                      })
                  .FirstOrDefault();

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
