using System.Collections.Generic;

namespace RevEng.Shared
{
    public class ReverseEngineerResult
    {
        public IList<string> EntityTypeFilePaths { get; set; }
        public string ContextFilePath { get; set; }
        public IList<string> ContextConfigurationFilePaths { get; set; }
        public List<string> EntityErrors { get; set; }
        public List<string> EntityWarnings { get; set; }
    }
}
