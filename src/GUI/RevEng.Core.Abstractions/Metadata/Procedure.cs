using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    public class Procedure
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public bool HasValidResultSet { get; set; }

        public List<ProcedureParameter> Parameters = new List<ProcedureParameter>();
        public List<ProcedureResultElement> ResultElements = new List<ProcedureResultElement>();
    }
}
