using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    /// <summary>
    /// Base object for functions and stored procedures
    /// </summary>
    public class Routine : SqlObjectBase
    {
        public bool HasValidResultSet { get; set; }

        public bool SupportsMultipleResultSet
        {
            get
            { 
                return Results.Count > 1; 
            }
        }

        public List<ModuleParameter> Parameters { get; set; } = new List<ModuleParameter>();
        public List<List<ModuleResultElement>> Results { get; set; } = new List<List<ModuleResultElement>>();
    }
}
