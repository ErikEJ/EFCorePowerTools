using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    /// <summary>
    /// Base object for functions and stored procedures.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO")]
    public class Routine : SqlObjectBase
    {
        public bool HasValidResultSet { get; set; }

        public int UnnamedColumnCount { get; set; }

        public bool SupportsMultipleResultSet
        {
            get
            {
                return Results.Count > 1;
            }
        }

        public bool NoResultSet
        {
            get
            {
                return Results.Count == 1 && Results[0].Count == 0 && HasValidResultSet;
            }
        }

        public string MappedType { get; set; }

        public List<ModuleParameter> Parameters { get; set; } = new List<ModuleParameter>();
        public List<List<ModuleResultElement>> Results { get; set; } = new List<List<ModuleResultElement>>();
    }
}
