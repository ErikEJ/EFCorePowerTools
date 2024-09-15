using RevEng.Common;

namespace ErikEJ.EFCorePowerTools;

internal static class Constants
{
    public const string ConfigFileName = RevEng.Common.Constants.ConfigFileName;

#if CORE60
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore6;
    public const int Version = 6;
#elif CORE80
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore8;
    public const int Version = 8;
#else
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore7;
    public const int Version = 7;
#endif
}
