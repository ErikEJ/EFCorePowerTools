using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace RevEng.Core.Diagram
{
    internal sealed class DatabaseModelToDgml
    {
        private readonly DatabaseModel databaseModel;
        private readonly string fileName;

        public DatabaseModelToDgml(DatabaseModel databaseModel, string fileName)
        {
            this.fileName = fileName;
            this.databaseModel = databaseModel;
        }

        public void CreateDgml()
        {
            using (var dgmlHelper = new DgmlHelper(this.fileName))
            {
                dgmlHelper.BeginElement("Nodes");

                dgmlHelper.WriteNode("Database", databaseModel.DatabaseName, null, "Database", "Expanded", null);

                var schemas = databaseModel.Tables.Select(t => t.Schema).ToList();

                foreach (var schema in schemas)
                {
                    dgmlHelper.WriteNode(schema, schema, null, "Schema", "Expanded", null);
                }

                foreach (var table in databaseModel.Tables)
                {
                    var desc = table.Comment;

                    dgmlHelper.WriteNode(table.Name, table.Name, null, "Table", "Collapsed", desc);

                    foreach (var col in table.Columns)
                    {
                        var colDesc = col.Comment;
                        var shortType = col.StoreType;
                        var category = "NOT NULL";

                        if (col.IsNullable)
                        {
                            category = "NULL";
                        }

                        if (table.ForeignKeys.Any(c => c.Columns.Contains(col)))
                        {
                            category = "FK";
                        }

                        if (table.PrimaryKey?.Columns.Contains(col) ?? true)
                        {
                            category = "PK";
                        }

                        if (!string.IsNullOrEmpty(colDesc))
                        {
                            shortType = shortType + Environment.NewLine + colDesc;
                        }

                        dgmlHelper.WriteNode($"{table.Name}_{col.Name}", $"{col.Name} {col.StoreType}", null, category, null, shortType);
                    }
                }

                dgmlHelper.EndElement();

                dgmlHelper.BeginElement("Links");

                foreach (var schema in schemas)
                {
                    dgmlHelper.WriteLink("Database", schema, null, "Contains");
                }

                foreach (var table in databaseModel.Tables)
                {
                    dgmlHelper.WriteLink(table.Schema, table.Name, null, "Contains");

                    foreach (var col in table.Columns)
                    {
                        dgmlHelper.WriteLink(table.Name, $"{table.Name}_{col.Name}", null, "Contains");
                    }

                    foreach (var key in table.ForeignKeys)
                    {
                        var source = $"{table.Name}_{key.Columns[0].Name}";
                        var target = $"{key.PrincipalTable.Name}_{key.PrincipalColumns[0].Name}";
                        dgmlHelper.WriteLink(source, target, key.Name, "Foreign Key");
                    }
                }

                dgmlHelper.EndElement();

                dgmlHelper.Close();
            }
        }
    }
}
