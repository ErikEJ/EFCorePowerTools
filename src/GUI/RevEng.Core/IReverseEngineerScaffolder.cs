using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Common;

namespace RevEng.Core
{
    public interface IReverseEngineerScaffolder
    {
        SavedModelFiles GenerateDbContext(ReverseEngineerCommandOptions options, List<string> schemas, string outputContextDir, string modelNamespace, string contextNamespace);
        SavedModelFiles GenerateFunctions(ReverseEngineerCommandOptions options, ref List<string> errors, string outputContextDir, string modelNamespace, string contextNamespace, bool supportsFunction);
        SavedModelFiles GenerateStoredProcedures(ReverseEngineerCommandOptions options, ref List<string> errors, string outputContextDir, string modelNamespace, string contextNamespace, bool supportsProcedures);
    }
}
