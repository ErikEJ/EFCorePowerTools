using EntityFrameworkCore.Scaffolding.Handlebars;
using System.IO;

namespace RevEng.Core
{
    public class CustomTemplateFileService : FileSystemTemplateFileService
    {
        public CustomTemplateFileService(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }

        public string RootDirectory { get; }

        public override string RetrieveTemplateFileContents(
            string relativeDirectory,
            string fileName,
            string altRelativeDirectory = null)
        {
            var localDirectory = Path.GetFullPath(Path.Combine(RootDirectory, relativeDirectory));
            var contents = RetrieveFileContents(localDirectory, fileName);
            return contents;
        }
    }
}
