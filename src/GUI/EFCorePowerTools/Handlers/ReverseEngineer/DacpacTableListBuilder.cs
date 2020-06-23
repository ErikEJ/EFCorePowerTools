using EFCorePowerTools.Shared.Models;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReverseEngineer20
{
    public class DacpacTableListBuilder
    {
        private readonly string _dacpacPath;

        public DacpacTableListBuilder(string dacpacPath)
        {
            if (string.IsNullOrEmpty(dacpacPath))
            {
                throw new ArgumentNullException(@"invalid path", nameof(dacpacPath));
            }
            if (!File.Exists(dacpacPath))
            {
                throw new ArgumentException("Dacpac file not found");
            }
            _dacpacPath = dacpacPath;
        }

        public List<TableInformationModel> GetTableDefinitions()
        {
            var launcher = new EfRevEngLauncher(null);

            var tables = launcher.GetDacpacTables(_dacpacPath);

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
