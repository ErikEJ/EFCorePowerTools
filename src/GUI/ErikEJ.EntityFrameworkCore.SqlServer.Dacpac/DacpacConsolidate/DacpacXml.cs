using System;
using System.Globalization;
using System.IO;
using System.IO.Packaging;

namespace GOEddie.Dacpac.References
{
    public sealed class DacpacXml : IDisposable
    {
        private readonly Package package;

        public DacpacXml(string dacPath)
        {
            package = Package.Open(dacPath, FileMode.Open, FileAccess.ReadWrite);
        }

        public void Dispose()
        {
            Close();
        }

        public string GetXml(string fileName)
        {
            var part = package.GetPart(new Uri(string.Format(CultureInfo.InvariantCulture, "/{0}", fileName), UriKind.Relative));
            var stream = part.GetStream();

            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public void Close()
        {
            package.Close();
        }
    }
}
