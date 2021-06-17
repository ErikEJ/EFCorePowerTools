using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Abstractions;

namespace RevEng.Core.Modules
{
    public interface IModuleScaffolder
    {
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace);
        ScaffoldedModel ScaffoldModel(ModuleModel model, ModuleScaffolderOptions scaffolderOptions, ref List<string> errors);
    }
}
