using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Abstractions.Model
{
    public interface IFunctionModelFactory
    {
        FunctionModel Create(string connectionString, ModuleModelFactoryOptions options);
    }
}
