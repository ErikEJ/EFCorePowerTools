// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.BLL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    /// <summary>
    /// WizardData will serve as view model for the wizard pages.
    /// </summary>
    public class WizardDataViewModel : ViewModelBase
    {
        private ConfigModel selectedConfiguration;

        public IReverseEngineerBll Bll { get; set; }

        public string UiHint { get; set; }
        public Project Project { get; internal set; }
        public string Filename { get; internal set; }
        public bool OnlyGenerate { get; internal set; }
        public string OptionsPath { get; internal set; }
        public bool FromSqlProject { get; internal set; }

        //-- Configuration
        public ObservableCollection<ConfigModel> Configurations { get; set; } = [];

        public ConfigModel SelectedConfiguration
        {
            get => selectedConfiguration;
            set
            {
                if (Equals(value, selectedConfiguration))
                {
                    return;
                }

                selectedConfiguration = value;
                RaisePropertyChanged();
            }
        }

        //-- Database connection
        public ICommand LoadedCommand { get; set; }
        public ICommand AddDatabaseConnectionCommand { get; set; }
        public ICommand AddAdhocDatabaseConnectionCommand { get; set; }
        public ICommand AddDatabaseDefinitionCommand { get; set; }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; set; }
        public ObservableCollection<CodeGenerationItem> CodeGenerationModeList { get; set; }

        public List<SchemaInfo> Schemas { get; set; }

        public DatabaseConnectionModel SelectedDatabaseConnection { get; set; }
        public int CodeGenerationMode { get; set; }
        public bool FilterSchemas { get; set; }

        public string DataItem1 { get; set; }
        public string DataItem2 { get; set; }
        public string DataItem3 { get; set; }

    }
}
