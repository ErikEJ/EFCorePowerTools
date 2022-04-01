using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions;

namespace RevEng.Core.Modules
{
    public interface IRoutineScaffolder
    {
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpaceValue);
        ScaffoldedModel ScaffoldModel(RoutineModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors);
    }
}
