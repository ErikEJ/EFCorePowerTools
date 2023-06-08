using RevEng.Common;

namespace ErikEJ.EFCorePowerTools;

internal static class Constants
{
    public const string ConfigFileName = "efcpt-config.json";

#if CORE60
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore6;
    public const int Version = 6;
#else
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore7;
    public const int Version = 7;
#endif
}
