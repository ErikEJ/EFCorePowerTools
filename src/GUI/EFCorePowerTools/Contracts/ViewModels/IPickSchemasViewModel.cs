namespace EFCorePowerTools.Contracts.ViewModels
{
    using EventArgs;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using RevEng.Shared;

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