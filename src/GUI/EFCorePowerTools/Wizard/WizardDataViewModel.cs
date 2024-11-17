// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using EFCorePowerTools.Common.Models;
using GalaSoft.MvvmLight;

namespace EFCorePowerTools.Wizard
{
    /// <summary>
    /// WizardData will serve as view model for the wizard pages.
    /// </summary>
    public class WizardDataViewModel : ViewModelBase
    {
        private ConfigModel selectedConfiguration;

        public string DataItem1 { get; set; }
        public string DataItem2 { get; set; }
        public string DataItem3 { get; set; }


        public ObservableCollection<ConfigModel> Configurations { get; set; } =
            new ObservableCollection<ConfigModel>();

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
    }
}
