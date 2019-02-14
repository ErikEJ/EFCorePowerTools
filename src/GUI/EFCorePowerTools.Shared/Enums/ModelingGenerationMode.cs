namespace EFCorePowerTools.Shared.Enums
{
    using System;

    [Flags]
    public enum ModelingGenerationMode
    {
        None = 0,
        DbContext = 1,
        EntityTypes = 2,
        All = 3
    }
}