using System;
using System.Xml.Serialization;

namespace SqlServer.Rules.Report;

[Serializable]
public class IssueType
{
    [XmlAttribute]
    public string Severity { get; set; }

    [XmlAttribute]
    public string Description { get; set; }

    [XmlAttribute]
    public string Category { get; set; }

    [XmlAttribute]
    public string Id { get; set; }
}