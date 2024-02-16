using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

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
        public int DatabaseEdition { get; set; }
        public long DatabaseEditionId { get; set; }
        public int DatabaseVersion { get; set; }
        public int DatabaseLevel { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        [IgnoreDataMember]
        public bool HasIssues => EntityErrors.Count != 0 || EntityWarnings.Count != 0;
    }
}
