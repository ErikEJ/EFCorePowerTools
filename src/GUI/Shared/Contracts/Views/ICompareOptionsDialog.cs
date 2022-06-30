using System.Collections.Generic;
using EFCorePowerTools.Common.Models;

namespace EFCorePowerTools.Contracts.Views
{
    public interface ICompareOptionsDialog : IDialog<(DatabaseConnectionModel Connection, IEnumerable<string> ContextTypes)>
    {
        void AddConnections(IEnumerable<DatabaseConnectionModel> connections);
        void AddContextTypes(IEnumerable<string> contextTypes);
    }
}