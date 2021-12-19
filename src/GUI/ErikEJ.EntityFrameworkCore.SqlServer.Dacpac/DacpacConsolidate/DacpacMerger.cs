using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;

namespace GOEddie.Dacpac.References
{
    public class DacpacMerger
    {
        private readonly string[] _sources;
        private readonly TSqlModel _first;
        private readonly string _targetPath;
        private readonly TSqlModel _target;

        /// <summary>
        /// Merges the specified .dacpac files into the target .dacpac (which is created)
        /// </summary>
        public DacpacMerger(string target, string[] sources)
        {
            _sources = sources;
            _first = new TSqlModel(sources.First());
            var options = _first.CopyModelOptions();

            _target = new TSqlModel(_first.Version, options);
            _targetPath = target;
        }

        public void Merge()
        {
            var pre = string.Empty;
            var post = string.Empty;

            foreach (var source in _sources)
            {
                if (!File.Exists(source))
                {
                    continue;
                }

                var model = getModel(source);
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

                        if (!string.IsNullOrWhiteSpace(name) && !name.EndsWith(".xsd"))
                        {
                            _target.AddOrUpdateObjects(ast, name, new TSqlObjectOptions());    //WARNING throwing away ansi nulls and quoted identifiers!
                        }
                    }
                }

                using (var package = DacPackage.Load(source))
                {
                    if (!(package.PreDeploymentScript is null))
                    {
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                        pre += new StreamReader(package.PreDeploymentScript).ReadToEnd();
                    }
                    if (!(package.PostDeploymentScript is null))
                    {
                        post += new StreamReader(package.PostDeploymentScript).ReadToEnd();
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
                    }
                }
            }

            WriteFinalDacpac(_target, pre, post);
        }

        private void WriteFinalDacpac(TSqlModel model, string preScript, string postScript)
        {
            var metadata = new PackageMetadata();
            metadata.Name = "dacpac";

            DacPackageExtensions.BuildPackage(_targetPath, model, metadata);
            AddScripts(preScript, postScript, _targetPath);
        }

        TSqlModel getModel(string source)
        {
            if (source == _sources.FirstOrDefault<string>())
            {
                return _first;
            }

            try
            {
                return new TSqlModel(source);
            }
            catch (DacModelException e) when (e.Message.Contains("Required references are missing."))
            {
                throw new DacModelException("Failed to load model from DACPAC. "
                    + "A reason might be that the \"SuppressMissingDependenciesErrors\" isn't set to 'true' consistently. ",
                    e);
            }
        }

        private void AddScripts(string pre, string post, string dacpacPath)
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
    }
}
