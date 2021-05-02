namespace EFCorePowerTools.Contracts.ViewModels
{
    using System;
    using System.Windows.Input;

    public interface IAboutViewModel : IViewModel
    {
        event EventHandler CloseRequested;

        ICommand LoadedCommand { get; }
        ICommand OkCommand { get; }
        ICommand OpenSourcesCommand { get; }
        ICommand OpenMarketplaceCommand { get; }

        string Version { get; }
    }
}