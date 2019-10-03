namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using Shared.Models;

    public interface IPickServerDatabaseDialog : IDialog<(DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, bool IncludeViews)>
    {
        void PublishConnections(IEnumerable<DatabaseConnectionModel> connections);
        void PublishDefinitions(IEnumerable<DatabaseDefinitionModel> definitions);
    }
}