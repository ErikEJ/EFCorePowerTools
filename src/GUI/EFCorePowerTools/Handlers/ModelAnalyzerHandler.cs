using EFCorePowerTools.Extensions;
using EnvDTE;
using ErikEJ.SqlCeToolbox.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void Generate(string outputPath, Project project, GenerationType generationType)
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

                if (!project.Properties.Item("TargetFrameworkMoniker").Value.ToString().Contains(".NETFramework")
                    && !project.IsNetCore())
                {
                    EnvDteHelper.ShowError("Currently only .NET Framework and .NET Core 2.0 projects are supported - TargetFrameworkMoniker: " + project.Properties.Item("TargetFrameworkMoniker").Value);
                    return;
                }

                if (project.IsNetCore())
                {
                    var result = project.ContainsEfCoreDesignReference();
                    if (string.IsNullOrEmpty(result.Item2))
                    {
                        EnvDteHelper.ShowError("EF Core 2.1 or later not found in project");
                        return;
                    }

                    if (!result.Item1 && result.Item2 != "3.0.0.0")
                    {
                        var version = new Version(result.Item2);
                        var nugetHelper = new NuGetHelper();
                        nugetHelper.InstallPackage("Microsoft.EntityFrameworkCore.Design", project, version);
                        EnvDteHelper.ShowError($"Installing EFCore.Design version {version}, please retry the command");
                        return;
                    }
                }

                var processLauncher = new ProcessLauncher(project);

                var processResult = processLauncher.GetOutput(outputPath, generationType, null);

                if (processResult.StartsWith("Error:"))
                {
                    throw new ArgumentException(processResult, nameof(processResult));
                }

                var modelResult = processLauncher.BuildModelResult(processResult);

                switch (generationType)
                {
                    case GenerationType.Dgml:
                        GenerateDgml(processResult, project);
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

        private void GenerateDgml(string processResult, Project project)
        {
            var dgmlBuilder = new DgmlBuilder.DgmlBuilder();
            var processLauncher = new ProcessLauncher(project);

            var result = processLauncher.BuildModelResult(processResult);
            ProjectItem item = null;

            foreach (var info in result)
            {
                var dgmlText = dgmlBuilder.Build(info.Item2, info.Item1, GetTemplate());

                var path = Path.GetTempPath() + info.Item1 + ".dgml";
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