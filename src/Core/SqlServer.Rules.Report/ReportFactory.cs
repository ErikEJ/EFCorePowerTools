using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SqlServer.Rules.Report.Properties;

namespace SqlServer.Rules.Report
{
    public class ReportFactory
    {
        public delegate void NotifyHandler(string notificationMessage, NotificationType type);

#pragma warning disable CA1003 // Use generic event handler instances
        public event NotifyHandler Notify;
#pragma warning restore CA1003 // Use generic event handler instances

        public void Create(ReportRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            SendNotification($"Loading {request.FileName}.dacpac");
            var sw = Stopwatch.StartNew();

            TSqlModel model = TSqlModel.LoadFromDacpac(
                    request.InputPath,
                    new ModelLoadOptions()
                    {
                        LoadAsScriptBackedModel = true,
                        ModelStorageType = Microsoft.SqlServer.Dac.DacSchemaModelStorageType.Memory,
                    });
            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
            CodeAnalysisService service = factory.CreateAnalysisService(model);

            service.SetProblemSuppressor(request.Suppress);
            sw.Stop();
            SendNotification($"Loading {request.FileName}.dacpac complete, elapsed: {sw.Elapsed:hh\\:mm\\:ss}");

            SendNotification("Running rules");
            sw = Stopwatch.StartNew();

            var result = service.Analyze(model);

            if (!result.AnalysisSucceeded)
            {
                foreach (var err in result.InitializationErrors)
                {
                    SendNotification(err.Message, NotificationType.Error);
                }

                foreach (var err in result.SuppressionErrors)
                {
                    SendNotification(err.Message, NotificationType.Error);
                }

                foreach (var err in result.AnalysisErrors)
                {
                    SendNotification(err.Message, NotificationType.Error);
                }

                return;
            }
            else
            {
                foreach (var err in result.Problems)
                {
                    SendNotification(err.ErrorMessageString, NotificationType.Warning);
                }

                result.SerializeResultsToXml(GetOutputFileName(request, ReportOutputType.XML));
            }

            sw.Stop();
            SendNotification($"Running rules complete, elapsed: {sw.Elapsed:hh\\:mm\\:ss}");

            var report = new Report(
                request.Solution,
                GetIssueTypes(service.GetRules(), request.SuppressIssueTypes).ToList(),
                request.FileName,
                GetProblems(result.Problems).ToList());

            SendNotification("Writing report");
            sw = Stopwatch.StartNew();

            switch (request.ReportOutputType)
            {
                case ReportOutputType.XML:
                    var outFileName = GetOutputFileName(request, request.ReportOutputType);
                    SerializeReport(report, outFileName);
                    var outDir = GetOutputDirectory(request);
                    var xlstPath = Path.Combine(outDir, "RulesTransform.xslt");
                    if (!File.Exists(xlstPath))
                    {
                        File.WriteAllText(xlstPath, Resources.RulesTransform);
                    }

#pragma warning disable CA5372 // Use XmlReader for XPathDocument constructor
                    var xPathDoc = new XPathDocument(outFileName);
#pragma warning restore CA5372 // Use XmlReader for XPathDocument constructor
                    var xslTransform = new XslCompiledTransform();
                    using (var xmlWriter = new XmlTextWriter(Path.Combine(outDir, $"{request.FileName}.html"), null))
                    {
                        xslTransform.Load(xlstPath);
                        xslTransform.Transform(xPathDoc, null, xmlWriter);
                    }

                    break;
                default:
                    SendNotification($"Invalid report type: {request.ReportOutputType}");
                    break;
            }

            sw.Stop();
            SendNotification($"Writing report complete, elapsed: {sw.Elapsed:hh\\:mm\\:ss}");

            SendNotification($"Done with {request.FileName}.dacpac");
        }

        private static string GetOutputDirectory(ReportRequest request)
        {
            var currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var outDir = string.IsNullOrWhiteSpace(request.OutputDirectory) ? currentDir : request.OutputDirectory;

            // not sure where this " is coming from, but it throws an exception trying to use the path
            outDir = outDir.Replace("\"", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();

            if (!Path.IsPathRooted(outDir))
            {
                outDir = Path.Combine(currentDir, outDir);
            }

            return outDir;
        }

        private static string GetOutputFileName(ReportRequest request, ReportOutputType outputType)
        {
            string ext = outputType == ReportOutputType.XML ? ".xml" : ".csv";
            var outDir = GetOutputDirectory(request);
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            return Path.Combine(outDir, Path.GetFileNameWithoutExtension(request.OutputFileName) + ext);
        }

        private static IEnumerable<IssueType> GetIssueTypes(IList<RuleDescriptor> rules, Func<RuleDescriptor, bool> suppressIssueTypes)
        {
            return (from r in rules
                    where suppressIssueTypes == null || !suppressIssueTypes.Invoke(r)
                    select new IssueType()
                    {
                        Severity = r.Severity.ToString(),
                        Description = r.DisplayDescription,
                        Category = $"{r.Namespace}.{r.Metadata.Category}",

                        // as we are including msft rules now too, we need to include the namespace in the category
                        Id = r.ShortRuleId,
                    }).Distinct(new IssueTypeComparer());
        }

        private static IEnumerable<Issue> GetProblems(IEnumerable<SqlRuleProblem> problems)
        {
            return from p in problems
                   select new Issue
                   {
                       File = !string.IsNullOrWhiteSpace(p.SourceName) ? p.SourceName : GetName(p.ModelElement.Name),
                       Line = p.StartLine,
                       Message = p.Description,
                       Offset = p.StartColumn.ToString(CultureInfo.InvariantCulture),
                       TypeId = p.Rule(),
                   };
        }

        private static string GetName(ObjectIdentifier identifier)
        {
            return "[" + string.Join("].[", identifier.Parts.Select((string x) => x)) + "]";
        }

        private static void SerializeReport(Report report, string outputPath)
        {
            var serializer = new XmlSerializer(typeof(Report));
            var ns = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, string.Empty) });
            var xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
            };
            using (var writer = XmlWriter.Create(outputPath, xmlSettings))
            {
                writer.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='RulesTransform.xslt'");
                serializer.Serialize(writer, report, ns);
                writer.Close();
            }
        }

        private void SendNotification(string message, NotificationType type = NotificationType.Information)
        {
            Notify?.Invoke(message, type);
        }
    }
}