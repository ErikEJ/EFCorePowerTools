using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class RoutineModel
    {
        public List<Routine> Routines { get; set; }
        public List<string> Errors { get; set; }
    }
}
