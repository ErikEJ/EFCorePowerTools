using Microsoft.EntityFrameworkCore.Scaffolding;
using System.Collections.Generic;

namespace RevEng.Core.Procedures.Scaffolding
{
    public interface IProcedureScaffolder
    {
        ScaffoldedModel ScaffoldModel(string connectionString, ProcedureScaffolderOptions procedureOptions, ref List<string> errors);
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace);
    }
}