namespace EFCorePowerTools.Contracts.ViewModels
{
    using System;
    using EventArgs;

    public interface IModelingOptionsViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;
    }
}