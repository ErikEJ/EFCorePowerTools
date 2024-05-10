using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SqlServer.Rules.Report
{
    [XmlType("Project")]
    [Serializable]
    public class RulesProject
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement(ElementName = "Issue")]
        public List<Issue> Issues { get; set; }
    }
}