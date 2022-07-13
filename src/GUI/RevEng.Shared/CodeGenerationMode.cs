[assembly: System.CLSCompliant(true)]

namespace RevEng.Common
{
    public enum CodeGenerationMode
    {
        EFCore5 = 0,
        EFCore3 = 1,
        EFCore6 = 2,
        EFCore7 = 3,
    }
}
