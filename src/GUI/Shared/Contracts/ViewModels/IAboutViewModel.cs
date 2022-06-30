using System;
using System.Windows.Input;

namespace EFCorePowerTools.Contracts.ViewModels
{
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