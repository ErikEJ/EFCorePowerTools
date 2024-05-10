using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SqlServer.Rules.Report
{
    [Serializable]
    public class ReportInfo
    {
        public ReportInfo()
        {
        }

        public ReportInfo(string solutionName, List<IssueType> issueTypes, string projectName, List<Issue> problems)
        {
            ToolsVersion = typeof(ReportInfo).Assembly.GetName().Version.ToString();
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
}
