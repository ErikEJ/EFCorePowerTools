namespace EFCorePowerTools.Contracts.ViewModels
{
    using EventArgs;
    using RevEng.Shared;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public interface IPickSchemasViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }
        ICommand AddCommand { get; }
        ICommand RemoveCommand { get; }

        ObservableCollection<SchemaInfo> Schemas { get; }
    }
}