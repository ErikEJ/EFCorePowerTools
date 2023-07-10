using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.Models;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EFCorePowerTools.ViewModels
{
    public class PickProjectViewModel : ViewModelBase, IPickProjectViewModel
    {
        private ProjectModel selectedProject;

        public PickProjectViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);

            Projects = new ObservableCollection<ProjectModel>();

            Projects.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(Projects));
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<ProjectModel> Projects { get; }

        public ProjectModel SelectedProject
        {
            get => selectedProject;
            set
            {
                if (Equals(value, selectedProject))
                {
                    return;
                }

                selectedProject = value;
                RaisePropertyChanged();
            }
        }

        private void Loaded_Executed()
        {
            if (Projects.Any())
            {
                SelectedProject = Projects[0];
            }
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => SelectedProject != null;

        private void Cancel_Executed()
        {
            SelectedProject = null;
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }
    }
}
