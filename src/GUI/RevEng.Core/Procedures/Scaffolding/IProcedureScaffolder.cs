using Microsoft.EntityFrameworkCore.Scaffolding;

namespace RevEng.Core.Procedures.Scaffolding
{
    public interface IProcedureScaffolder
    {
        ScaffoldedModel ScaffoldModel(string connectionString, ProcedureScaffolderOptions procedureOptions);
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace);
    }
}