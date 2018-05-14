using System.Collections.Generic;

namespace DgmlBuilder
{
    public class DebugViewParserResult
    {
        public DebugViewParserResult()
        {
            Nodes = new List<string>();
            Links = new List<string>();
        }

        public List<string> Nodes { get; }

        public List<string> Links { get; }
    }
}
