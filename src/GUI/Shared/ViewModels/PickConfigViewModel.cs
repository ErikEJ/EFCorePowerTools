using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EFCorePowerTools.ViewModels
{
    public class PickConfigViewModel : ViewModelBase, IPickConfigViewModel
    {
        private ConfigModel selectedConfiguration;

        public PickConfigViewModel(IVisualStudioAccess visualStudioAccess, Func<IPickSchemasDialog> pickSchemasDialogFactory)
        {
            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);

            Configurations = new ObservableCollection<ConfigModel>();

            Configurations.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(Configurations));
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ObservableCollection<ConfigModel> Configurations { get; }

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

        private void Loaded_Executed()
        {
            // Database first
            if (Configurations.Any())
            {
                SelectedConfiguration = Configurations[0];
            }
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => SelectedConfiguration != null;

        private void Cancel_Executed()
        {
            SelectedConfiguration = null;
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }
    }
}