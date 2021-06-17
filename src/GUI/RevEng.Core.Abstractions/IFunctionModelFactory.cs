using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Abstractions.Model
{
    public interface IFunctionModelFactory
    {
        ModuleModel Create(string connectionString, ModuleModelFactoryOptions options);
    }
}
