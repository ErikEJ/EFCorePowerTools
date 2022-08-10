using System.Collections.Generic;
using EFCorePowerTools.Common.Models;
using RevEng.Common;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IPickServerDatabaseDialog : IDialog<(DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint)>
    {
        void PublishConnections(IEnumerable<DatabaseConnectionModel> connections);
        void PublishDefinitions(IEnumerable<DatabaseDefinitionModel> definitions);
        void PublishSchemas(IEnumerable<SchemaInfo> schemas);
        void PublishCodeGenerationMode(CodeGenerationMode codeGenerationMode, IEnumerable<KeyValuePair<int, string>> allowedVersions);
        void PublishUiHint(string uiHint);
    }
}
