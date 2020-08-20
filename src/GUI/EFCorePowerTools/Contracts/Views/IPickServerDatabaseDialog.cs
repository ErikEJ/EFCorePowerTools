namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using Shared.Models;
    using ReverseEngineer20.ReverseEngineer;

    public interface IPickServerDatabaseDialog : IDialog<(DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, bool IncludeViews, bool FilterSchemas, SchemaInfo[] Schemas)>
    {
        void PublishConnections(IEnumerable<DatabaseConnectionModel> connections);
        void PublishDefinitions(IEnumerable<DatabaseDefinitionModel> definitions);
        void PublishSchemas(IEnumerable<SchemaInfo> schemas);
    }
}