using System.IO;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace RevEng.Core.Dgml
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

        public FileInfo CreateDgml()
        {
            using (var dgmlHelper = new DgmlHelper(this.fileName))
            {
                ////List<string> schemas = new List<string>();

                ////foreach (var schema in schemas)
                ////{
                ////    dgmlHelper.WriteNode(schema, schema, null, "Schema", "Expanded", null);
                ////}

                ////foreach (string table in _tableNames)
                ////{
                ////    //desc = Comment!

                ////    // Create Nodes
                ////    dgmlHelper.WriteNode(table, table, null, "Table", "Collapsed", desc);

                ////    List<Column> columns = _allColumns.Where(c => c.TableName == table).ToList();
                ////    foreach (Column col in columns)
                ////    {

                ////        string shortType = col.ShortType.Remove(col.ShortType.Length - 1);

                ////        string category = "Field";
                ////        if (col.IsNullable == YesNoOption.YES)
                ////            category = "Field Optional";

                ////        // Fix for multiple constraints with same columns
                ////        Dictionary<string, Constraint> columnForeignKeys = new Dictionary<string, Constraint>();

                ////        var tableKeys = _allForeignKeys.Where(c => c.ConstraintTableName == col.TableName);
                ////        foreach (var constraint in tableKeys)
                ////        {
                ////            if (!columnForeignKeys.ContainsKey(constraint.Columns.ToString()))
                ////            {
                ////                columnForeignKeys.Add(constraint.Columns.ToString(), constraint);
                ////            }
                ////        }

                ////        if (columnForeignKeys.ContainsKey(string.Format("[{0}]", col.ColumnName)))
                ////        {
                ////            category = "Field Foreign";
                ////        }

                ////        List<PrimaryKey> primaryKeys = _allPrimaryKeys.Where(p => p.TableName == table).ToList();
                ////        if (primaryKeys.Count > 0)
                ////        {
                ////            var keys = (from k in primaryKeys
                ////                        where k.ColumnName == col.ColumnName
                ////                        select k.ColumnName).SingleOrDefault();
                ////            if (!string.IsNullOrEmpty(keys))
                ////                category = "Field Primary";

                ////        }
                ////        var colDesc = descriptionCache.Where(dc => dc.Parent == table && dc.Object == col.ColumnName).Select(dc => dc.Description).SingleOrDefault();
                ////        if (!string.IsNullOrEmpty(colDesc))
                ////            shortType = shortType + Environment.NewLine + colDesc;
                ////        dgmlHelper.WriteNode(string.Format("{0}_{1}", table, col.ColumnName), col.ColumnName, null, category, null, shortType);
                ////    }
                ////}

                ////dgmlHelper.EndElement();

                ////dgmlHelper.BeginElement("Links");
                ////foreach (var schema in schemas)
                ////{
                ////    dgmlHelper.WriteLink("Database", schema, null, "Contains");
                ////}
                ////foreach (string table in _tableNames)
                ////{
                ////    if (_repository.IsServer())
                ////    {
                ////        var split = table.Split('.');
                ////        dgmlHelper.WriteLink(split[0], table, null, "Contains");
                ////    }

                ////    List<Column> columns = _allColumns.Where(c => c.TableName == table).ToList();
                ////    foreach (Column col in columns)
                ////    {
                ////        dgmlHelper.WriteLink(table, string.Format("{0}_{1}", table, col.ColumnName),
                ////            null, "Contains");
                ////    }

                ////    List<Constraint> foreignKeys = _allForeignKeys.Where(c => c.ConstraintTableName == table).ToList();
                ////    foreach (Constraint key in foreignKeys)
                ////    {
                ////        var col = key.Columns[0];
                ////        col = RemoveBrackets(col);
                ////        var uniqueCol = key.UniqueColumns[0];
                ////        uniqueCol = RemoveBrackets(uniqueCol);
                ////        string source = string.Format("{0}_{1}", table, col);
                ////        string target = string.Format("{0}_{1}", key.UniqueConstraintTableName, uniqueCol);
                ////        dgmlHelper.WriteLink(source, target, key.ConstraintName, "Foreign Key");
                ////    }
                ////}
                ////dgmlHelper.EndElement();

                //////Close the DGML document
                ////dgmlHelper.Close();
            }

            return null;
        }
    }
}
