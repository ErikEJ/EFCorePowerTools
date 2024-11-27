using System;
using System.Collections.Generic;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.Views;
using RevEng.Common;

namespace EFCorePowerTools.Contracts.EventArgs
{
    public class WizardEventArgs : System.EventArgs
    {
        public Project Project { get; set; }

        public string OptionsPath { get; set; }

        public string Filename { get; set; }

        public bool OnlyGenerate { get; set; }

        public bool FromSqlProject { get; set; }

        public string UiHint { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        // WizardPage1
        public IPickServerDatabaseDialog PickServerDatabaseDialog { get; set; }
        public bool PickServerDatabaseComplete { get; set; }
        public DatabaseConnectionModel DbInfo { get; set; }
        public ReverseEngineerOptions RevEngOptions { get; set; }
        public Tuple<List<Schema>, string> NamingOptionsAndPath { get; set; }

        // WizardPage2
        public IPickTablesDialog PickTablesDialog { get; set; }
        public bool PickTablesDialogComplete { get; set; }

        public List<ConfigModel> Options { get; set; } = [];
    }
}
