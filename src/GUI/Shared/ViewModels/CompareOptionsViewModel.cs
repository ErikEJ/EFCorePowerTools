namespace EFCorePowerTools.ViewModels
{
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using EFCorePowerTools.Locales;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Shared.DAL;
    using Shared.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class CompareOptionsViewModel : ViewModelBase, ICompareOptionsViewModel
    {
        private readonly IVisualStudioAccess _visualStudioAccess;
        private DatabaseConnectionModel _selectedDatabaseConnection;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand AddDatabaseConnectionCommand
        {
            get;
        }
        public ICommand RemoveDatabaseConnectionCommand
        {
            get;
        }
        public ICommand OkCommand
        {
            get;
        }
        public ICommand CancelCommand
        {
            get;
        }

        public ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; } = new ObservableCollection<DatabaseConnectionModel>();
        public ObservableCollection<ContextTypeItemViewModel> ContextTypes { get; } = new ObservableCollection<ContextTypeItemViewModel>();

        public DatabaseConnectionModel SelectedDatabaseConnection
        {
            get => _selectedDatabaseConnection;
            set => Set(ref _selectedDatabaseConnection, value);
        }

        public CompareOptionsViewModel(IVisualStudioAccess visualStudioAccess)
        {
            _visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));

            AddDatabaseConnectionCommand = new RelayCommand(AddDatabaseConnection_Executed);
            RemoveDatabaseConnectionCommand = new RelayCommand(RemoveDatabaseConnection_Executed, RemoveDatabaseConnection_CanExecute);
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
        }

        private void AddDatabaseConnection_Executed()
        {
            DatabaseConnectionModel newDatabaseConnection;
            try
            {
                newDatabaseConnection = _visualStudioAccess.PromptForNewDatabaseConnection();
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowMessage($"{CompareLocale.UnableToAddConnection}: {e.Message}");
                return;
            }

            if (newDatabaseConnection == null)
                return;

            DatabaseConnections.Add(newDatabaseConnection);
            SelectedDatabaseConnection = newDatabaseConnection;
        }

        private void RemoveDatabaseConnection_Executed()
        {
            if (SelectedDatabaseConnection == null)
            {
                return;
            }

            try
            {
                _visualStudioAccess.RemoveDatabaseConnection(SelectedDatabaseConnection.DataConnection);
                DatabaseConnections.Remove(SelectedDatabaseConnection);
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowMessage($"{CompareLocale.UnableToRemoveConnection}: {e.Message}");
                return;
            }

            SelectedDatabaseConnection = null;
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => SelectedDatabaseConnection != null && ContextTypes.Any(c => c.Selected);

        private bool RemoveDatabaseConnection_CanExecute() => SelectedDatabaseConnection != null;

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        public void AddDatabaseConnections(IEnumerable<DatabaseConnectionModel> connections)
        {
            foreach (var item in connections)
            {
                DatabaseConnections.Add(item);
            }
        }

        public void AddContextTypes(IEnumerable<string> contextTypes)
        {
            foreach (var item in contextTypes)
            {
                ContextTypes.Add(new ContextTypeItemViewModel(true, item));
            }
        }

        public (DatabaseConnectionModel Connection, IEnumerable<string> ContextTypes) GetSelection()
        {
            return (SelectedDatabaseConnection, ContextTypes.Where(c => c.Selected).Select(c => c.Name).ToList());
        }
    }

    public class ContextTypeItemViewModel : ViewModelBase
    {
        private bool _selected;

        public ContextTypeItemViewModel(bool selected, string name)
        {
            Name = name;
            Selected = selected;
        }

        public string Name
        {
            get; set;
        }

        public bool Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }
    }
}