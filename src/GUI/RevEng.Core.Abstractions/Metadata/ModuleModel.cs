using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class ModuleModel
    {
        public List<ModuleBase> Routines { get; set; }
        public List<string> Errors { get; set; }
    }
}
