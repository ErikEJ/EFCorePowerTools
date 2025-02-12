using System;
using System.IO;
using Microsoft.SqlServer.Dac.CodeAnalysis;

namespace SqlServer.Rules.Report
{
    public class ReportRequest
    {
        private string outputFileName;

        public string Solution { get; set; }

        public string InputPath { get; set; }

        public string SolutionName
        {
            get { return Path.GetFileNameWithoutExtension(Solution); }
        }

        public string OutputDirectory { get; set; } = string.Empty;

        public string OutputFileName
        {
            get
            {
                if (string.IsNullOrEmpty(outputFileName))
                {
                    OutputFileName = $"{FileName}.xml";
                }

                return outputFileName;
            }

            set
            {
                outputFileName = value;
            }
        }

        public Predicate<SqlRuleProblemSuppressionContext> Suppress { get; set; }

        public Func<RuleDescriptor, bool> SuppressIssueTypes { get; set; }

        public string FileName
        {
            get { return Path.GetFileNameWithoutExtension(InputPath); }
        }

        public ReportOutputType ReportOutputType { get; set; } = ReportOutputType.XML;
    }
}