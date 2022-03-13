using System.Collections.Generic;

namespace Dgml
{
    public class DebugViewParserResult
    {
        public DebugViewParserResult()
        {
            Nodes = new List<string>();
            Links = new List<string>();
        }

#pragma warning disable CA1002 // Do not expose generic lists
        public List<string> Nodes { get; }
        public List<string> Links { get; }
#pragma warning restore CA1002 // Do not expose generic lists
    }
}
