using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Abstractions.Model
{
    public interface IProcedureModelFactory
    {
        ModuleModel Create(string connectionString, ModuleModelFactoryOptions options);
    }
}
