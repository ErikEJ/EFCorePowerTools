using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class ModuleModel<T>
        where T : ModuleBase
    {
        public List<T> Routines { get; set; }
        public List<string> Errors { get; set; }
    }
}
