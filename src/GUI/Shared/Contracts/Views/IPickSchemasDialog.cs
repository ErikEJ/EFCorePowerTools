using System.Collections.Generic;
using RevEng.Common;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IPickSchemasDialog : IDialog<SchemaInfo[]>
    {
        void AddSchemas(IEnumerable<SchemaInfo> schemas);
    }
}
