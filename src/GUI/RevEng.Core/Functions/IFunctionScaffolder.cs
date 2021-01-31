using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using System.Collections.Generic;

namespace RevEng.Core.Functions
{
    public interface IFunctionScaffolder
    {
        ScaffoldedModel ScaffoldModel(FunctionModel model, ModuleScaffolderOptions procedureOptions, ref List<string> errors);
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace);
    }
}