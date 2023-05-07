namespace ErikEJ.EFCorePowerTools;

internal static class Constants
{
    public const string ConfigFileName = "efcpt-config.json";

#if CORE60
    public const int EfCoreVersion = 6;
#else
    public const int EfCoreVersion = 7;
#endif
}
