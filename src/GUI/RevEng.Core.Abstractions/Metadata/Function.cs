namespace RevEng.Core.Abstractions.Metadata
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public class Function : Routine
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        public bool IsScalar { get; set; }
    }
}
