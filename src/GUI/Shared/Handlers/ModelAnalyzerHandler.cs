using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
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

        public async System.Threading.Tasks.Task GenerateAsync(string outputPath, Project project, GenerationType generationType)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                if (string.IsNullOrEmpty(outputPath))
                {
                    throw new ArgumentException(outputPath, nameof(outputPath));
                }

                if (await project.GetAttributeAsync("TargetFrameworkMoniker") == null)
                {
                    VSHelper.ShowError(SharedLocale.SelectedProjectTypeNoTargetFrameworkMoniker);
                    return;
                }

                if (!await project.IsNetCore31OrHigherAsync())
                {
                    VSHelper.ShowError($"{SharedLocale.SupportedFramework}: {await project.GetAttributeAsync("TargetFrameworkMoniker")}");
                    return;
                }

                var result = await project.ContainsEfCoreDesignReferenceAsync();
                if (string.IsNullOrEmpty(result.Item2))
                {
                    VSHelper.ShowError(SharedLocale.EFCoreVersionNotFound);
                    return;
                }

                if (!Version.TryParse(result.Item2, out Version version))
                {
                    VSHelper.ShowError(string.Format(ModelAnalyzerLocale.CurrentEFCoreVersion, result.Item2));
                }

                if (!result.Item1)
                {
                    var nugetHelper = new NuGetHelper();
                    await nugetHelper.InstallPackageAsync("Microsoft.EntityFrameworkCore.Design", project, version);
                    VSHelper.ShowError(string.Format(SharedLocale.InstallingEfCoreDesignPackage, version));
                    return;
                }

                var processLauncher = new ProcessLauncher(project);

                var processResult = await processLauncher.GetOutputAsync(outputPath, generationType, null);

                if (string.IsNullOrEmpty(processResult))
                {
                    throw new InvalidOperationException(ModelAnalyzerLocale.UnableToCollectModelInformation);
                }

                if (processResult.Contains("Error:"))
                {
                    throw new InvalidOperationException(processResult);
                }

                var modelResult = processLauncher.BuildModelResult(processResult);

                switch (generationType)
                {
                    case GenerationType.Dgml:
                        await GenerateDgmlAsync(modelResult, project);
                        Telemetry.TrackEvent("PowerTools.GenerateModelDgml");
                        break;
                    case GenerationType.Ddl:
                        var files = project.GenerateFiles(modelResult, ".sql");
                        foreach (var file in files)
                        {
                            await VS.Documents.OpenAsync(file);
                        }
                        Telemetry.TrackEvent("PowerTools.GenerateSqlCreate");
                        break;
                    case GenerationType.DebugView:
                        var views = project.GenerateFiles(modelResult, ".txt");
                        foreach (var file in views)
                        {
                            await VS.Documents.OpenAsync(file);
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

        private async System.Threading.Tasks.Task GenerateDgmlAsync(List<Tuple<string, string>> modelResult, Project project)
        {
            var dgmlBuilder = new DgmlBuilder.DgmlBuilder();
            string target = null;

            foreach (var info in modelResult)
            {
                var dgmlText = dgmlBuilder.Build(info.Item2, info.Item1, GetTemplate());

                if (info.Item1.IndexOfAny(Path.GetInvalidPathChars()) >= 0
                    || info.Item1.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    VSHelper.ShowError($"{SharedLocale.InvalidName}: {info.Item1}");
                    return;
                }

                var path = Path.Combine(Path.GetTempPath(), info.Item1 + ".dgml");

                File.WriteAllText(path, dgmlText, Encoding.UTF8);

                target = Path.Combine(Path.GetDirectoryName(project.FullPath), Path.GetFileName(path));

                File.Copy(path, target, true);
            }
            if (target != null)
            {
                await VS.Documents.OpenAsync(target);
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