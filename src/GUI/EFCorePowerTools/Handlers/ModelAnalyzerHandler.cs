using EFCorePowerTools.Extensions;
using EnvDTE;
using ErikEJ.SqlCeToolbox.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EFCorePowerTools.Handlers
{
    internal class ModelAnalyzerHandler
    {
        private readonly EFCorePowerToolsPackage _package;

        public ModelAnalyzerHandler(EFCorePowerToolsPackage package)
        {
            _package = package;
        }

        public async System.Threading.Tasks.Task GenerateAsync(string outputPath, Project project, GenerationType generationType)
        {
            try
            {
                if (string.IsNullOrEmpty(outputPath))
                {
                    throw new ArgumentException(outputPath, nameof(outputPath));
                }

                if (project.Properties.Item("TargetFrameworkMoniker") == null)
                {
                    EnvDteHelper.ShowError("The selected project type has no TargetFrameworkMoniker");
                    return;
                }

                if (!project.IsNetCore30OrHigher())
                {
                    EnvDteHelper.ShowError("Only .NET Core 3.0+ projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                var result = await project.ContainsEfCoreDesignReferenceAsync();
                if (string.IsNullOrEmpty(result.Item2))
                {
                    EnvDteHelper.ShowError("EF Core 3.1 or later not found in project");
                    return;
                }

                if (!result.Item1)
                {
                    if (!Version.TryParse(result.Item2, out Version version))
                    {
                        EnvDteHelper.ShowError($"Cannot support version {result.Item2}, notice that previews have limited supported. You can try to manually install Microsoft.EntityFrameworkCore.Design preview.");
                        return;
                    }
                    var nugetHelper = new NuGetHelper();
                    nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, version);
                    EnvDteHelper.ShowError($"Installing EFCore.Design version {version}, please retry the command");
                    return;
                }

                var processLauncher = new ProcessLauncher(project);

                var processResult = await processLauncher.GetOutputAsync(outputPath, generationType, null);

                if (string.IsNullOrEmpty(processResult))
                {
                    throw new ArgumentException("Unable to collect model information", nameof(processResult));
                }

                if (processResult.StartsWith("Error:"))
                {
                    throw new ArgumentException(processResult, nameof(processResult));
                }

                var modelResult = processLauncher.BuildModelResult(processResult);

                switch (generationType)
                {
                    case GenerationType.Dgml:
                        GenerateDgml(modelResult, project);
                        Telemetry.TrackEvent("PowerTools.GenerateModelDgml");
                        break;
                    case GenerationType.Ddl:
                        var files = project.GenerateFiles(modelResult, ".sql");
                        foreach (var file in files)
                        {
                            _package.Dte2.ItemOperations.OpenFile(file);
                        }
                        Telemetry.TrackEvent("PowerTools.GenerateSqlCreate");
                        break;
                    case GenerationType.DebugView:
                        var views = project.GenerateFiles(modelResult, ".txt");
                        foreach (var file in views)
                        {
                            _package.Dte2.ItemOperations.OpenFile(file);
                        }
                        Telemetry.TrackEvent("PowerTools.GenerateDebugView");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                _package.LogError(new List<string>(), exception);
            }
        }

        private void GenerateDgml(List<Tuple<string, string>> modelResult, Project project)
        {
            var dgmlBuilder = new DgmlBuilder.DgmlBuilder();
            ProjectItem item = null;

            foreach (var info in modelResult)
            {
                var dgmlText = dgmlBuilder.Build(info.Item2, info.Item1, GetTemplate());

                if (info.Item1.IndexOfAny(Path.GetInvalidPathChars()) >= 0
                    || info.Item1.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    EnvDteHelper.ShowError("Invalid name: " + info.Item1);
                    return;
                }

                var path = Path.Combine(Path.GetTempPath(), info.Item1 + ".dgml");

                File.WriteAllText(path, dgmlText, Encoding.UTF8);
                item = project.ProjectItems.GetItem(Path.GetFileName(path));
                if (item != null)
                {
                    item.Delete();
                }
                item = project.ProjectItems.AddFromFileCopy(path);
            }

            if (item != null)
            {
                var window = item.Open();
                window.Document.Activate();
            }
        }

        private string GetTemplate()
        {
            var resourceName = "EFCorePowerTools.DgmlBuilder.template.dgml";

            Stream stream = null;
            try
            {
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                using (StreamReader reader = new StreamReader(stream))
                {
                    stream = null;
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }
    }
}