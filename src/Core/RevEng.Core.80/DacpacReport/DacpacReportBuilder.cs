using System;
using System.IO;
using SqlServer.Rules.Report;

namespace RevEng.Core.DacpacReport
{
    public class DacpacReportBuilder
    {
        private readonly FileInfo dacpac;

        public DacpacReportBuilder(FileInfo dacpac)
        {
            ArgumentNullException.ThrowIfNull(dacpac);
            this.dacpac = dacpac;
        }

        public string BuildReport()
        {
            var request = new ReportRequest
            {
                Solution = dacpac.Name,
                InputPath = dacpac.FullName,
                OutputDirectory = Path.GetDirectoryName(dacpac.FullName),
                ////Suppress = p => Regex.IsMatch(p.Problem.RuleId, @"Microsoft\.Rules.*(SR0001|SR0016|SR0005|SR0007)", RegexOptions.IgnoreCase),
            };

            var factory = new ReportFactory();

            factory.Create(request);

            var fileName = dacpac.Name.Replace(".dacpac", ".html", StringComparison.OrdinalIgnoreCase);

            return Path.Join(Path.GetDirectoryName(dacpac.FullName), fileName);
        }
    }
}
