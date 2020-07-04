using EFCorePowerTools.Shared.Models;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;

namespace ReverseEngineer20
{
    public class TableListBuilder
    {
        private readonly string _connectionString;
        private readonly DatabaseType _databaseType;

        public TableListBuilder(string connectionString, DatabaseType databaseType)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
            _databaseType = databaseType;
        }

        public List<TableInformationModel> GetTableDefinitions()
        {
            var launcher = new EfRevEngLauncher(null);

            var tables = new List<TableInformationModel>();

            if (_databaseType == DatabaseType.Undefined)
            {
                tables = launcher.GetDacpacTables(_connectionString);
            }
            else
            {
                tables = launcher.GetTables(_connectionString, _databaseType);
            }

            foreach (var item in tables)
            {
                if (!item.HasPrimaryKey)
                {
                    item.HasPrimaryKey = true;
                    item.ShowKeylessWarning = true;
                }
            }

            return tables;
        }
    }
}
