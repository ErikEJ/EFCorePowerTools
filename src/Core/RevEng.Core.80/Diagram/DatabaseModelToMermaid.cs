using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace RevEng.Core.Diagram
{
    public class DatabaseModelToMermaid
    {
        private static readonly bool[] Lookup = new bool[65536];
        private readonly DatabaseModel databaseModel;

        public DatabaseModelToMermaid(DatabaseModel databaseModel)
        {
            this.databaseModel = databaseModel;
            for (char c = '0'; c <= '9'; c++)
            {
                Lookup[c] = true;
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                Lookup[c] = true;
            }

            for (char c = 'a'; c <= 'z'; c++)
            {
                Lookup[c] = true;
            }

            Lookup['.'] = true;
            Lookup['_'] = true;
            Lookup['-'] = true;
        }

        public string CreateMermaid(bool createMarkdown = true)
        {
            var sb = new System.Text.StringBuilder();

            if (createMarkdown)
            {
                sb.AppendLine("```mermaid");
            }

            sb.AppendLine("erDiagram");

            foreach (var table in databaseModel.Tables)
            {
                if (table.ForeignKeys.Count == 0 && table.PrimaryKey == null)
                {
                    continue;
                }

                var formattedTableName = Sanitize(table.Name);

                sb.AppendLine(CultureInfo.InvariantCulture, $"  {formattedTableName} {{");
                foreach (var column in table.Columns)
                {
                    var formattedColumnName = Sanitize(column.Name);

                    var pkfk = string.Empty;

                    if (table.PrimaryKey?.Columns.Contains(column) ?? false)
                    {
                        pkfk = "PK";
                    }

                    if (table.ForeignKeys.Any(c => c.Columns.Contains(column)))
                    {
                        pkfk = string.IsNullOrEmpty(pkfk) ? "FK" : "PK,FK";
                    }

                    var nullable = column.IsNullable ? "(NULL)" : string.Empty;
                    sb.AppendLine(CultureInfo.InvariantCulture, $"    {formattedColumnName} {column.StoreType?.Replace(", ", "-", System.StringComparison.OrdinalIgnoreCase)}{nullable} {pkfk}");
                }

                sb.AppendLine("  }");

                foreach (var foreignKey in table.ForeignKeys)
                {
                    var relationship = "}o--|";

                    var dependentIndexes = foreignKey.PrincipalTable.Indexes
                        .Where(i => i.Columns.SequenceEqual(foreignKey.PrincipalColumns));

                    if (dependentIndexes.Any(i => i.IsUnique))
                    {
                        relationship = "|o--|";
                    }

                    if (foreignKey.PrincipalColumns.Any(c => c.IsNullable))
                    {
                        relationship = "}o--o";
                    }

                    var formattedPrincipalTableName = Sanitize(foreignKey.PrincipalTable.Name);
                    var formattedForeignKeyName = Sanitize(foreignKey.Name);

                    sb.AppendLine(CultureInfo.InvariantCulture, $"  {formattedTableName} {relationship}| {formattedPrincipalTableName} : {formattedForeignKeyName}");
                }
            }

            if (createMarkdown)
            {
                sb.AppendLine("```");
            }

            return sb.ToString();
        }

        private static string Sanitize(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                char[] buffer = new char[name.Length];
                int index = 0;
                foreach (char c in name)
                {
                    if (Lookup[c])
                    {
                        buffer[index] = c;
                        index++;
                    }
                }

                return new string(buffer, 0, index);
            }

            return name;
        }
    }
}