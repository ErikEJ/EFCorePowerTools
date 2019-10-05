using System.Collections.Generic;

namespace ReverseEngineer20
{
    public class ReverseEngineerResult
    {
        public IList<string> EntityTypeFilePaths { get; set; }
        public string ContextFilePath { get; set; }
        public List<string> EntityErrors { get; set; }
        public List<string> EntityWarnings { get; set; }
    }
}
