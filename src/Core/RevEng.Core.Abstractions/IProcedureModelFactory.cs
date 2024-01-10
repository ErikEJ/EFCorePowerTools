using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Abstractions.Model
{
    public interface IProcedureModelFactory
    {
        RoutineModel Create(string connectionString, ModuleModelFactoryOptions options);
    }
}
