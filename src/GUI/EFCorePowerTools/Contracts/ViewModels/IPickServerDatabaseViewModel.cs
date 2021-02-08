namespace EFCorePowerTools.Contracts.ViewModels
{
    using EventArgs;
    using RevEng.Shared;
    using Shared.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public interface IPickServerDatabaseViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand LoadedCommand { get; }
        ICommand AddDatabaseConnectionCommand { get; }
        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        ObservableCollection<DatabaseConnectionModel> DatabaseConnections { get; }
        ObservableCollection<DatabaseDefinitionModel> DatabaseDefinitions { get; }

        List<SchemaInfo> Schemas { get; }

        DatabaseConnectionModel SelectedDatabaseConnection { get; set; }
        DatabaseDefinitionModel SelectedDatabaseDefinition { get; set; }

        string UiHint { get; set; }
        bool IncludeViews { get; set; }
        bool FilterSchemas { get; set; }
    }
}