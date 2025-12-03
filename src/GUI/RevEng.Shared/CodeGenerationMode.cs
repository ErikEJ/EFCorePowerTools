[assembly: System.CLSCompliant(true)]

namespace RevEng.Common
{
#pragma warning disable CA1008 // Enums should have zero value
    public enum CodeGenerationMode
#pragma warning restore CA1008 // Enums should have zero value
    {
        // Do not re-use 0, 1, 2 or 3!
        EFCore8 = 4,
        EFCore9 = 5,
        EFCore10 = 6,
    }
}