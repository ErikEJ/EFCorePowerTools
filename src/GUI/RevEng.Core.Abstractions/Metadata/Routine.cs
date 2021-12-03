using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    /// <summary>
    /// Base object for functions and stored procedures
    /// </summary>
    public class Routine : SqlObjectBase
    {
        public bool HasValidResultSet { get; set; }

        public List<ModuleParameter> Parameters = new List<ModuleParameter>();
        public List<List<ModuleResultElement>> Results = new List<List<ModuleResultElement>>();

        public bool SupportsMultipleResultSet
        {
            get
            { 
                return Results.Count > 1; 
            }
        }
    }
}
