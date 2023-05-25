using RevEng.Common;

namespace ErikEJ.EFCorePowerTools;

internal static class Constants
{
    public const string ConfigFileName = "efcpt-config.json";

#if CORE60
    public const CodeGenerationMode EFCoreVersion = CodeGenerationMode.EFCore6;
#else
    public const CodeGenerationMode EFCoreVersion = CodeGenerationMode.EFCore7;
#endif
}
