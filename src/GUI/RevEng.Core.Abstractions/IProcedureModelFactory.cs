using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Abstractions.Model
{
    public interface IProcedureModelFactory
    {
        ProcedureModel Create(string connectionString, ModuleModelFactoryOptions options);
    }
}
