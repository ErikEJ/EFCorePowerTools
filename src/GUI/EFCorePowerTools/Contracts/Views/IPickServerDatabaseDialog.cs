namespace EFCorePowerTools.Contracts.Views
{
    using RevEng.Shared;
    using Shared.Models;
    using System.Collections.Generic;

    public interface IPickServerDatabaseDialog : IDialog<(DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, bool IncludeViews, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint)>
    {
        void PublishConnections(IEnumerable<DatabaseConnectionModel> connections);
        void PublishDefinitions(IEnumerable<DatabaseDefinitionModel> definitions);
        void PublishSchemas(IEnumerable<SchemaInfo> schemas);
        void PublishCodeGenerationMode(CodeGenerationMode codeGenerationMode);
        void PublishUiHint(string uiHint);
    }
}