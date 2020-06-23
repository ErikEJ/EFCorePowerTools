using EntityFrameworkCore.Scaffolding.Handlebars;
using System.IO;

namespace ReverseEngineer20.ReverseEngineer
{
    public class CustomTemplateFileService : FileSystemTemplateFileService
    {
        public string RootDirectory { get; }

        public CustomTemplateFileService(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }

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
