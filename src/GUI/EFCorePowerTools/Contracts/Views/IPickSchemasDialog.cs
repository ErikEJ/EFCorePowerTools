namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using RevEng.Shared;

    public interface IPickSchemasDialog : IDialog<SchemaInfo[]>
    {
        void AddSchemas(IEnumerable<SchemaInfo> schemas);
    }
}
