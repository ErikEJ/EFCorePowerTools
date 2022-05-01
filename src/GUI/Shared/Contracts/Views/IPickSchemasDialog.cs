namespace EFCorePowerTools.Contracts.Views
{
    using RevEng.Common;
    using System.Collections.Generic;

    public interface IPickSchemasDialog : IDialog<SchemaInfo[]>
    {
        void AddSchemas(IEnumerable<SchemaInfo> schemas);
    }
}
