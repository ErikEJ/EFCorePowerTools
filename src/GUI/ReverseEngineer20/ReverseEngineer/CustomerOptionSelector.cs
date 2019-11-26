using System.Collections.Generic;
using System.Linq;

namespace ReverseEngineer20.ReverseEngineer
{
    public static class SelectorHelper
    {
        public static bool HasKey<T>(this Dictionary<string, T> pairs, string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            return pairs.ContainsKey(key);
        }
    }

    public class TableSelector
    {
        public string Name { get; set; }

        public string NewName { get; set; }

        public string VariableName { get; set; }

        public Dictionary<string, ColumnNamer> Columns { get; private set; }

        public TableSelector(TableRenamer table)
        {
            Name = table.Name;
            NewName = table.NewName;
            VariableName = table.VariableName;
            Columns = table.Columns?.ToDictionary(key => key.Name, value => value) ??
                new Dictionary<string, ColumnNamer>();
        }

        public bool HasCustomColumnName(string columnName)
        {
            if (!HasColumn(columnName))
                return false;

            return !string.IsNullOrWhiteSpace(Columns[columnName].NewName);
        }

        public string GetCustomColumnName(string columnName)
        {
            if (!HasCustomColumnName(columnName))
                return null;

            return Columns[columnName].NewName;
        }

        public bool HasColumn(string columnName) => Columns.HasKey(columnName);
    }

    public class SchemaSelector
    {
        public bool UseSchemaName { get; set; }

        public string SchemaName { get; set; }

        public Dictionary<string, TableSelector> Tables { get; private set; }

        public SchemaSelector(Schema schema)
        {
            UseSchemaName = schema.UseSchemaName;
            SchemaName = schema.SchemaName;
            Tables = schema.Tables?.ToDictionary(key => key.Name, value => new TableSelector(value)) ??
                    new Dictionary<string, TableSelector>();
        }

        public bool HasCustomTableName(string tableName)
        {
            if (!HasTable(tableName))
                return false;

            return !string.IsNullOrWhiteSpace(Tables[tableName].NewName);
        }

        public bool HasCustomVariableName(string tableName)
        {
            if (!HasTable(tableName))
                return false;

            return !string.IsNullOrWhiteSpace(Tables[tableName].VariableName);
        }

        public string GetNewTableName(string tableName)
        {
            if (!HasCustomTableName(tableName))
                return null;
            return Tables[tableName].NewName;
        }

        public string GetVariableName(string tableName)
        {
            if (!HasCustomTableName(tableName))
                return null;
            return Tables[tableName].VariableName;
        }

        public TableSelector GetTable(string tableName)
        {
            if (!HasTable(tableName))
                return null;

            return Tables[tableName];
        }

        public bool HasTable(string tableName) => Tables.HasKey(tableName);
    }

    public class CustomerOptionSelector
    {
        private Dictionary<string, SchemaSelector> _selector =
            new Dictionary<string, SchemaSelector>();
        public CustomerOptionSelector(List<Schema> customReplacers)
        {
            foreach(var schema in customReplacers)
            {
                if (!string.IsNullOrWhiteSpace(schema.SchemaName) || !_selector.ContainsKey(schema.SchemaName))
                    _selector.Add(schema.SchemaName, new SchemaSelector(schema));
            }
        }

        public SchemaSelector GetSchema(string schemaName)
        {
            if (!HasSchema(schemaName))
                return null;
            return _selector[schemaName];
        }

        public TableSelector GetTable(string schemaName, string tableName)
        {
            if (!HasSchema(schemaName))
                return null;

            if (!_selector[schemaName].Tables.ContainsKey(tableName))
                return null;
            return _selector[schemaName].Tables[tableName];
        }

        public bool HasCustomTableName(string schemaName, string tableName)
        {
            if (!HasSchema(schemaName))
                return false;

            return _selector[schemaName].HasCustomTableName(tableName);
        }

        public bool HasVariableName(string schemaName, string tableName)
        {
            if (!HasSchema(schemaName))
                return false;

            return _selector[schemaName].HasCustomVariableName(tableName);
        }

        public bool HasCustomColumnName(string schemaName, string tableName, string columnName)
        {
            if (!HasSchema(schemaName))
                return false;

            return _selector[schemaName].GetTable(tableName)?.HasCustomColumnName(columnName) ?? false;
        }

        public string GetCustomColumnName(string schemaName, string tableName, string columnName)
        {
            if (!HasCustomColumnName(schemaName, tableName, columnName))
                return null;

            return _selector[schemaName].GetTable(tableName).GetCustomColumnName(columnName);
        }

        public string GetVariableName(string schemaName, string tableName)
        {
            if (!HasVariableName(schemaName, tableName))
                return null;

            return _selector[schemaName].GetTable(tableName).VariableName;
        }

        public bool HasSchema(string schema) => _selector.HasKey(schema);
    }
}
