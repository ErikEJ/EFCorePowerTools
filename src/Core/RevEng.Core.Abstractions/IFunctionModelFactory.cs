using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Abstractions.Model
{
    public interface IFunctionModelFactory
    {
        RoutineModel Create(string connectionString, ModuleModelFactoryOptions options);
    }
}
