using RevEng.Core.Procedures.Model;

namespace RevEng.Core.Procedures.Model
{
    public interface IProcedureModelFactory
    {
        ProcedureModel Create(string connectionString, ProcedureModelFactoryOptions options);
    }
}
