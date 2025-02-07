using System;

namespace SqlServer.Rules.Report
{
    [Serializable]
    public class InspectionScope
    {
        public InspectionScope()
        {
            Element = "Project";
        }

        public string Element { get; set; }
    }
}