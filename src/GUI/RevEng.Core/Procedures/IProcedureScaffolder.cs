using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using System.Collections.Generic;

namespace RevEng.Core.Procedures.Scaffolding
{
    public interface IProcedureScaffolder
    {
        ScaffoldedModel ScaffoldModel(string connectionString, ProcedureScaffolderOptions procedureOptions, ProcedureModelFactoryOptions procedureModelFactoryOptions, ref List<string> errors);
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace);
    }
}