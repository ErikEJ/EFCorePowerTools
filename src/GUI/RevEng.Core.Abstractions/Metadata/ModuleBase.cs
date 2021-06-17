﻿using System.Collections.Generic;

namespace RevEng.Core.Abstractions.Metadata
{
    /// <summary>
    /// Base object for functions and stored procedures
    /// </summary>
    public class ModuleBase : SqlObjectBase
    {
        public bool HasValidResultSet { get; set; }

        public List<ModuleParameter> Parameters = new List<ModuleParameter>();
        public List<ProcedureResultElement> ResultElements = new List<ProcedureResultElement>();
    }
}