namespace EFCorePowerTools.Contracts.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using EventArgs;
    using Shared.Models;

    public interface IPickServerDatabaseViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand LoadedCommand { get; }
        ICommand AddDatabaseConnectionCommand { get; }
        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; }
        ObservableCollection<DatabaseDefinitionModel> DatabaseDefinitions { get; }

        DatabaseConnectionModel SelectedDatabaseConnection { get; set; }
        DatabaseDefinitionModel SelectedDatabaseDefinition { get; set; }

        bool IncludeViews { get; set; }
    }
}