using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SqlServer.Rules.Report
{
#pragma warning disable CA1724 // Type names should not match namespaces
    [Serializable]
    public class Report
    {
        public Report()
        {
        }

        public Report(string solutionName, List<IssueType> issueTypes, string projectName, List<Issue> problems)
        {
            ToolsVersion = typeof(Report).Assembly.GetName().Version.ToString();
            Information = new Information() { Solution = $"{solutionName}.sln" };
            IssueTypes = issueTypes;
            Issues = new List<RulesProject> { new Rules.Report.RulesProject() { Name = projectName, Issues = problems } };
        }

        [XmlAttribute]
        public string ToolsVersion { get; set; }

        public Information Information { get; set; }

        public List<IssueType> IssueTypes { get; set; }

        public List<RulesProject> Issues { get; set; }
    }
#pragma warning restore CA1724 // Type names should not match namespaces
}