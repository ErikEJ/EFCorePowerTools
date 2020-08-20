namespace EFCorePowerTools.Contracts.ViewModels
{
    using EventArgs;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using ReverseEngineer20.ReverseEngineer;

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