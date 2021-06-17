using Microsoft.EntityFrameworkCore.Scaffolding;

namespace RevEng.Core.Modules
{
    public interface IModuleScaffolder
    {
        SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, string nameSpace);
    }
}
