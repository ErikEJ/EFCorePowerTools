﻿[assembly: System.CLSCompliant(true)]

namespace RevEng.Common
{
#pragma warning disable CA1008 // Enums should have zero value
    public enum CodeGenerationMode
#pragma warning restore CA1008 // Enums should have zero value
    {
        // Do not re-use 0!
        EFCore3 = 1,
        EFCore6 = 2,
        EFCore7 = 3,
    }
}
