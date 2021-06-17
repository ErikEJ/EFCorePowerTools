using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class ModuleModel
    {
        public List<Module> Routines { get; set; }
        public List<string> Errors { get; set; }
    }
}
