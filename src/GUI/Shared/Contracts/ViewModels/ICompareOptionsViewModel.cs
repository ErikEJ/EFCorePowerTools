using System;
using System.Collections.Generic;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface ICompareOptionsViewModel : IViewModel
    {
        event EventHandler<CloseRequestedEventArgs> CloseRequested;

        void AddDatabaseConnections(IEnumerable<DatabaseConnectionModel> connections);
        void AddContextTypes(IEnumerable<string> contextTypes);

        (DatabaseConnectionModel Connection, IEnumerable<string> ContextTypes) GetSelection();
    }
}