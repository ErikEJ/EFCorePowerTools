using RevEng.Common;

namespace ErikEJ.EFCorePowerTools;

internal static class Constants
{
    public const string ConfigFileName = RevEng.Common.Constants.ConfigFileName;
    public const string RenamingFileName = RevEng.Common.Constants.RenamingFileName;

#if CORE60
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore6;
    public const int Version = 6;
#elif CORE90
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore9;
    public const int Version = 9;
#else
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore8;
    public const int Version = 8;
#endif
}
