namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using ReverseEngineer20.ReverseEngineer;

    public interface IPickSchemasDialog : IDialog<SchemaInfo[]>
    {
        void AddSchemas(IEnumerable<SchemaInfo> schemas);
    }
}
