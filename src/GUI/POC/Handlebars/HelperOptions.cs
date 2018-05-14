using System;
using System.IO;

namespace HandlebarsDotNet
{
    public sealed class HelperOptions
    {
        private readonly Action<TextWriter, object> _template;
        private readonly Action<TextWriter, object> _inverse;

        internal HelperOptions(
            Action<TextWriter, object> template,
            Action<TextWriter, object> inverse)
        {
            _template = template;
            _inverse = inverse;
        }

        public Action<TextWriter, object> Template
        {
            get { return _template; }
        }

        public Action<TextWriter, object> Inverse
        {
            get { return _inverse; }
        }
    }
}

