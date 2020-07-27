using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Procedures.Model;

namespace RevEng.Core.Procedures.Scaffolding
{
    public interface IProcedureScaffolder
    {
        ScaffoldedModel ScaffoldModel(string connectionString, ProcedureScaffolderOptions procedureOptions, ProcedureModelFactoryOptions procedureModelFactoryOptions);
    }
}