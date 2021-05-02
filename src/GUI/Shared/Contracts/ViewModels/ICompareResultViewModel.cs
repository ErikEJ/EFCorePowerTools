using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Handlers.Compare;
using EFCorePowerTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface ICompareResultViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        void AddComparisonResult(IEnumerable<CompareLogModel> connections);

        ObservableCollection<CompareLogItemViewModel> Logs { get; }

        bool ShowDifferencesOnly { get; set; }

        ICommand CloseCommand { get; }

        ICommand SetVisibilityCommand { get; }
    }
}