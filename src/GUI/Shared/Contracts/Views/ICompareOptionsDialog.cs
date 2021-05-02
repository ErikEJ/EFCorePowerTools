namespace EFCorePowerTools.Contracts.Views
{
    using Shared.Models;
    using System.Collections.Generic;

    public interface ICompareOptionsDialog : IDialog<(DatabaseConnectionModel Connection, IEnumerable<string> ContextTypes)>
    {
        void AddConnections(IEnumerable<DatabaseConnectionModel> connections);
        void AddContextTypes(IEnumerable<string> contextTypes);
    }
}