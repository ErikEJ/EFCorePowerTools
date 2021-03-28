namespace EFCorePowerTools.Contracts.Views
{
    using RevEng.Shared;
    using System.Collections.Generic;

    public interface IPickSchemasDialog : IDialog<SchemaInfo[]>
    {
        void AddSchemas(IEnumerable<SchemaInfo> schemas);
    }
}
