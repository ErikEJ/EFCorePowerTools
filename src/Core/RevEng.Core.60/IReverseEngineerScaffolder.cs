using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Common;

namespace RevEng.Core
{
    public interface IReverseEngineerScaffolder
    {
        SavedModelFiles GenerateDbContext(ReverseEngineerCommandOptions options, List<string> schemas, string outputContextDir, string modelNamespace, string contextNamespace, string projectPath, string outputPath);
        SavedModelFiles GenerateFunctions(ReverseEngineerCommandOptions options, List<string> schemas, ref List<string> errors, string outputContextDir, string modelNamespace, string contextNamespace, bool supportsFunctions);
        SavedModelFiles GenerateStoredProcedures(ReverseEngineerCommandOptions options, List<string> schemas, ref List<string> errors, string outputContextDir, string modelNamespace, string contextNamespace, bool supportsProcedures);
    }
}
