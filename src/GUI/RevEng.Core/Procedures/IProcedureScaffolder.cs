using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using System.Collections.Generic;

namespace RevEng.Core.Procedures.Scaffolding
{
    public interface IProcedureScaffolder
    {
        ScaffoldedModel ScaffoldModel(ProcedureModel model, ProcedureScaffolderOptions procedureOptions, ref List<string> errors);
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace);
    }
}