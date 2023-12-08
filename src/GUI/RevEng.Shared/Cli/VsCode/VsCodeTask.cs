using System.Collections.Generic;

namespace RevEng.Common.Cli.VsCode
{
    internal sealed class VsCodeTask
    {
        public string version { get; set; }
        public List<TaskItem> tasks { get; set; }
        public List<InputItem> inputs { get; set; }
    }
}
