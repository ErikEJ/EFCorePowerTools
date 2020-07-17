using System.Collections.Generic;

namespace RevEng.Core.Procedures.Model
{
    public class StoredProcedure
    {
        public string Name { get; set; }
        public string Schema { get; set; }

        public List<StoredProcedureParameter> Parameters = new List<StoredProcedureParameter>();
        public List<StoredProcedureResultElement> ResultElements = new List<StoredProcedureResultElement>();
    }
}
