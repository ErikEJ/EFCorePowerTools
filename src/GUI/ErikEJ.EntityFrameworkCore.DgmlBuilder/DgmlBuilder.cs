using System;

namespace DgmlBuilder
{
    public class DgmlBuilder
    {
        public string Build(string debugView, string contextName, string template)
        {
            var parser = new DebugViewParser();
            var result = parser.Parse(debugView.Split(new [] { Environment.NewLine }, StringSplitOptions.None), contextName);

            var nodes = string.Join(Environment.NewLine, result.Nodes);
            var links = string.Join(Environment.NewLine, result.Links);

            return template.Replace("{Links}", links).Replace("{Nodes}", nodes);
        }

    }
}
