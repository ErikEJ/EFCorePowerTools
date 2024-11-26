using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace GOEddie.Dacpac.References
{
    public sealed class DacpacMerger : IDisposable
    {
        private readonly string[] sources;
        private readonly TSqlModel first;
        private readonly string targetPath;
        private readonly TSqlModel target;

        public DacpacMerger(string target, string[] sources)
        {
            ArgumentNullException.ThrowIfNull(target);
            ArgumentNullException.ThrowIfNull(sources);

            this.sources = sources;
            first = new TSqlModel(sources[0]);
            var options = first.CopyModelOptions();

            this.target = new TSqlModel(first.Version, options);
            targetPath = target;
        }

        public void Merge()
        {
            var pre = new StringBuilder();
            var post = new StringBuilder();

            foreach (var source in sources)
            {
                if (!File.Exists(source))
                {
                    continue;
                }

                using (var model = GetModel(source))
                {
                    foreach (var obj in model.GetObjects(DacQueryScopes.UserDefined))
                    {
                        TSqlScript ast;
                        if (obj.TryGetAst(out ast))
                        {
                            var name = obj.Name.ToString();
                            var info = obj.GetSourceInformation();
                            if (info != null && !string.IsNullOrWhiteSpace(info.SourceName))
                            {
                                name = info.SourceName;
                            }

                            if (!string.IsNullOrWhiteSpace(name) && !name.EndsWith(".xsd", StringComparison.OrdinalIgnoreCase))
                            {
                                target.AddOrUpdateObjects(ast, name, new TSqlObjectOptions()); // WARNING throwing away ansi nulls and quoted identifiers!
                            }
                        }
                    }
                }

                using (var package = DacPackage.Load(source))
                {
                    if (!(package.PreDeploymentScript is null))
                    {
                        using (var stream = new StreamReader(package.PreDeploymentScript))
                        {
                            pre.Append(stream.ReadToEnd());
                        }
                    }

                    if (!(package.PostDeploymentScript is null))
                    {
                        using (var stream = new StreamReader(package.PostDeploymentScript))
                        {
                            post.Append(stream.ReadToEnd());
                        }
                    }
                }
            }

            WriteFinalDacpac(target, pre.ToString(), post.ToString());
        }

        public void Dispose()
        {
            first?.Dispose();
            target?.Dispose();
        }

        private static void AddScripts(string pre, string post, string dacpacPath)
        {
            using (var package = Package.Open(dacpacPath, FileMode.Open, FileAccess.ReadWrite))
            {
                if (!string.IsNullOrEmpty(pre))
                {
                    var part = package.CreatePart(new Uri("/predeploy.sql", UriKind.Relative), "text/plain");

                    using (var stream = part.GetStream())
                    {
                        stream.Write(Encoding.UTF8.GetBytes(pre), 0, pre.Length);
                    }
                }

                if (!string.IsNullOrEmpty(post))
                {
                    var part = package.CreatePart(new Uri("/postdeploy.sql", UriKind.Relative), "text/plain");

                    using (var stream = part.GetStream())
                    {
                        stream.Write(Encoding.UTF8.GetBytes(post), 0, post.Length);
                    }
                }

                package.Close();
            }
        }

        private void WriteFinalDacpac(TSqlModel model, string preScript, string postScript)
        {
            var metadata = new PackageMetadata();
            metadata.Name = "dacpac";

            DacPackageExtensions.BuildPackage(targetPath, model, metadata);
            AddScripts(preScript, postScript, targetPath);
        }

        private TSqlModel GetModel(string source)
        {
            if (source == sources.FirstOrDefault<string>())
            {
                return first;
            }

            try
            {
                return new TSqlModel(source);
            }
            catch (DacModelException e) when (e.Message.Contains("Required references are missing.", StringComparison.OrdinalIgnoreCase))
            {
                throw new DacModelException(
                    "Failed to load model from DACPAC. "
                    + "A reason might be that the \"SuppressMissingDependenciesErrors\" isn't set to 'true' consistently. ",
                    e);
            }
        }
    }
}
