using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Modules;
using System.Collections.Generic;

namespace RevEng.Core.Functions
{
    public interface IFunctionScaffolder : IModuleScaffolder
    {
        ScaffoldedModel ScaffoldModel(FunctionModel model, ModuleScaffolderOptions procedureOptions, ref List<string> errors);
    }
}
