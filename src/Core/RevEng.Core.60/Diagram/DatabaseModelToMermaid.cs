using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace RevEng.Core.Diagram
{
    public class DatabaseModelToMermaid
    {
        private readonly DatabaseModel databaseModel;

        public DatabaseModelToMermaid(DatabaseModel databaseModel)
        {
            this.databaseModel = databaseModel;
        }

        public string CreateMermaid(bool createMarkdown = true)
        {
            var sb = new System.Text.StringBuilder();

            if (createMarkdown)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"## {databaseModel.DatabaseName}");
                sb.AppendLine();
                sb.AppendLine("```mermaid");
            }

            sb.AppendLine("erDiagram");

            foreach (var table in databaseModel.Tables)
            {
                if (table.ForeignKeys.Count == 0 && table.PrimaryKey == null)
                {
                    continue;
                }

                var formattedTableName = table.Name.Replace(" ", string.Empty, System.StringComparison.OrdinalIgnoreCase);

                sb.AppendLine(CultureInfo.InvariantCulture, $"  {formattedTableName} {{");
                foreach (var column in table.Columns)
                {
                    var formattedColumnName = column.Name.Replace(" ", string.Empty, System.StringComparison.OrdinalIgnoreCase);

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

                    sb.AppendLine(CultureInfo.InvariantCulture, $"  {formattedTableName} {relationship}| {foreignKey.PrincipalTable.Name} : {foreignKey.Name}");
                }
            }

            if (createMarkdown)
            {
                sb.AppendLine("```");
            }

            return sb.ToString();
        }
    }
}
