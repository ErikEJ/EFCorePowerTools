using System;
using System.Data;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using SqlServer.Rules.Report;

namespace RevEng.Core
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
                Solution = dacpac.FullName,
                InputPath = dacpac.FullName,

                //// Suppress = p => Regex.IsMatch(p.Problem.RuleId, @"Microsoft\.Rules.*(SR0001|SR0016|SR0005|SR0007)", RegexOptions.IgnoreCase),
            };

            var factory = new ReportFactory();

            factory.Notify += Factory_Notify;

            factory.Create(request);

            var fileName = dacpac.Name.Replace(".dacpac", ".html", StringComparison.OrdinalIgnoreCase);

            return Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
        }

        private static void Factory_Notify(string notificationMessage, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case NotificationType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }

            Console.WriteLine(notificationMessage);
            Console.ResetColor();
        }
    }
}
