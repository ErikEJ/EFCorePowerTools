using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO")]
    public class RoutineModel
    {
        public List<Routine> Routines { get; set; }
        public List<string> Errors { get; set; }
    }
}
