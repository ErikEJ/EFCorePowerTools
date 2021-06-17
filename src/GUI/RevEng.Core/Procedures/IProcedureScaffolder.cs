using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Modules;
using System.Collections.Generic;

namespace RevEng.Core.Procedures
{
    public interface IProcedureScaffolder : IModuleScaffolder
    {
        ScaffoldedModel ScaffoldModel(ProcedureModel model, ModuleScaffolderOptions procedureOptions, ref List<string> errors);
    }
}
