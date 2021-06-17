using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    /// <summary>
    /// Base object for functions and stored procedures
    /// </summary>
    public class Module : SqlObjectBase
    {
        public bool HasValidResultSet { get; set; }

        public List<ModuleParameter> Parameters = new List<ModuleParameter>();
        public List<ModuleResultElement> ResultElements = new List<ModuleResultElement>();
    }
}
