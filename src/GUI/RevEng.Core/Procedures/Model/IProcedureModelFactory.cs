using RevEng.Core.Procedures.Model.Metadata;

namespace RevEng.Core.Procedures.Model
{
    public interface IProcedureModelFactory
    {
        ProcedureModel Create(string connectionString);
    }
}
