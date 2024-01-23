using System;

[assembly: CLSCompliant(false)]

namespace Dgml
{
    public static class DgmlBuilder
    {
        public static string Build(string debugView, string contextName, string template)
        {
#pragma warning disable CA1510 // Use ArgumentNullException throw helper
            if (debugView == null)
            {
                throw new ArgumentNullException(nameof(debugView));
            }

            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
#pragma warning restore CA1510 // Use ArgumentNullException throw helper

            var result = DebugViewParser.Parse(debugView.Split(new[] { Environment.NewLine }, StringSplitOptions.None), contextName);

            var nodes = string.Join(Environment.NewLine, result.Nodes);
            var links = string.Join(Environment.NewLine, result.Links);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            return template.Replace("{Links}", links, StringComparison.OrdinalIgnoreCase).Replace("{Nodes}", nodes, StringComparison.OrdinalIgnoreCase);
#else
            return template.Replace("{Links}", links).Replace("{Nodes}", nodes);
#endif
        }
    }
}
