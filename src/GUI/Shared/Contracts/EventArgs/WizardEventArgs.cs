using System;
using System.Collections.Generic;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Handlers.ReverseEngineer;
using RevEng.Common;

namespace EFCorePowerTools.Contracts.EventArgs
{
    public class WizardEventArgs : System.EventArgs
    {
        public bool IsInvokedByWizard { get; set; }

        public Project Project { get; set; }

        public string OptionsPath { get; set; }

        public string Filename { get; set; }

        public bool OnlyGenerate { get; set; }

        public bool FromSqlProject { get; set; }

        public string UiHint { get; set; }

        public List<ConfigModel> Configurations { get; set; } = [];

        public IServiceProvider ServiceProvider { get; set; }

        // WizardPage1
        public IPickServerDatabaseDialog PickServerDatabaseDialog { get; set; }

        public bool PickServerDatabaseComplete { get; set; }

        public DatabaseConnectionModel DbInfo { get; set; }

        public ReverseEngineerOptions Options { get; set; }

        public Tuple<List<Schema>, string> NamingOptionsAndPath { get; set; }

        // WizardPage2
        public IPickTablesDialog PickTablesDialog { get; set; }

        public bool PickTablesDialogComplete { get; set; }

        // WizardPage3
        public IModelingOptionsDialog ModelingOptionsDialog { get; set; }

        public bool ModelingDialogOptionsComplete { get; set; }

        public bool NewOptions { get; set; }

        public ReverseEngineerUserOptions UserOptions { get; set; }

        public bool ForceEdit { get; set; }

        public string ReverseEngineerStatus { get; set; }

        // If populated, will be sent to statusbar by wizard processes
        public string StatusbarMessage { get; internal set; }
    }
}