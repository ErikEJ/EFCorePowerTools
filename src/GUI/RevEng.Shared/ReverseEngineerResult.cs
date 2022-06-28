using System.Collections.Generic;

namespace RevEng.Common
{
    public class ReverseEngineerResult
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<string> EntityTypeFilePaths { get; set; }
        public string ContextFilePath { get; set; }
        public IList<string> ContextConfigurationFilePaths { get; set; }
        public List<string> EntityErrors { get; set; }
        public List<string> EntityWarnings { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
