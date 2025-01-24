using System.Collections.Generic;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using RevEng.Common;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IPickServerDatabaseDialog : IDialog<(DatabaseConnectionModel Connection, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint, WizardEventArgs WizardArgs)>
    {
        void PublishConnections(IEnumerable<DatabaseConnectionModel> connections);
        void PublishDefinitions(IEnumerable<DatabaseConnectionModel> definitions);
        void PublishSchemas(IEnumerable<SchemaInfo> schemas);
        void PublishCodeGenerationMode(CodeGenerationMode codeGenerationMode, IList<CodeGenerationItem> allowedVersions);
        void PublishUiHint(string uiHint);
        (DatabaseConnectionModel Connection, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint, WizardEventArgs WizardArgs) GetResults();
    }
}
