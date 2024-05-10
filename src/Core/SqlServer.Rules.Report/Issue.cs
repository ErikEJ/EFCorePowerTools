using System;
using System.Xml.Serialization;

namespace SqlServer.Rules.Report
{
    [XmlType("Issue")]
    [Serializable]
    public class Issue
    {
        [XmlAttribute]
        public string Message { get; set; }

        [XmlAttribute]
        public int Line { get; set; }

        [XmlAttribute]
        public string Offset { get; set; }

        [XmlAttribute]
        public string File { get; set; }

        [XmlAttribute]
        public string TypeId { get; set; }
    }
}