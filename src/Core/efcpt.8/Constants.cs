using RevEng.Common;

namespace ErikEJ.EFCorePowerTools;

internal static class Constants
{
    public const string ConfigFileName = RevEng.Common.Constants.ConfigFileName;
    public const string RenamingFileName = RevEng.Common.Constants.RenamingFileName;

#if CORE80
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore8;
    public const int Version = 8;
#elif CORE90
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore9;
    public const int Version = 9;
#elif CORE100
    public const CodeGenerationMode CodeGeneration = CodeGenerationMode.EFCore10;
    public const int Version = 10;
#endif
}