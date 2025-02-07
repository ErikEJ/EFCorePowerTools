using System;
using System.Linq;
using Microsoft.SqlServer.Dac.CodeAnalysis;

namespace SqlServer.Rules.Report
{
    public static class ProblemExtensions
    {
        public static string Rule(this SqlRuleProblem problem)
        {
            ArgumentNullException.ThrowIfNull(problem);

#pragma warning disable S6608 // Prefer indexing instead of "Enumerable" methods on types implementing "IList"
            return problem.RuleId.Split('.').Last();
#pragma warning restore S6608 // Prefer indexing instead of "Enumerable" methods on types implementing "IList"
        }
    }
}