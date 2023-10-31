using System.Collections.Generic;

namespace RevEng.Common.Cli.VsCode
{
    internal class TaskItem
    {
        public string label { get; set; }
        public string command { get; set; }
        public string type { get; set; }
        public List<string> args { get; set; }
        public string group { get; set; }
        public Presentation presentation { get; set; }
        public string problemMatcher { get; set; }
    }
}
