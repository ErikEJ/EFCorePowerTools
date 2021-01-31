using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class FunctionModel
    {
        public List<Function> Functions { get; set; }
        public List<string> Errors { get; set; }
    }
}
