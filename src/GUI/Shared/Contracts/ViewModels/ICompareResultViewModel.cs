using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Handlers.Compare;
using EFCorePowerTools.ViewModels;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface ICompareResultViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        bool ShowDifferencesOnly { get; set; }

        ICommand CloseCommand { get; }

        ICommand SetVisibilityCommand { get; }

        ObservableCollection<CompareLogItemViewModel> Logs { get; }

        void AddComparisonResult(IEnumerable<CompareLogModel> logs);
    }
}
