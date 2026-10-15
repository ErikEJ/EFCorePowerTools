using System;
using System.IO;
using Microsoft.SqlServer.Dac.CodeAnalysis;

namespace SqlServer.Rules.Report;

public class ReportRequest
{
    public string Solution { get; set; }

    public string InputPath { get; set; }

    public string SolutionName => Path.GetFileNameWithoutExtension(Solution);

    public string OutputDirectory { get; set; } = string.Empty;

    public string OutputFileName
    {
        get
        {
            if (string.IsNullOrEmpty(field))
            {
                OutputFileName = $"{FileName}.xml";
            }

            return field;
        }

        set;
    }

    public Predicate<SqlRuleProblemSuppressionContext> Suppress { get; set; }

    public Func<RuleDescriptor, bool> SuppressIssueTypes { get; set; }

    public string FileName => Path.GetFileNameWithoutExtension(InputPath);

    public ReportOutputType ReportOutputType { get; set; } = ReportOutputType.XML;
}