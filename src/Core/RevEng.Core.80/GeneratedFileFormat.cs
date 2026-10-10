using System;
using RevEng.Common;

namespace RevEng.Core
{
    public sealed record GeneratedFileFormat(string LineEndingStyle, string Charset)
    {
        public static GeneratedFileFormat From(ReverseEngineerCommandOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            return new GeneratedFileFormat(options.FileLineEndingStyle, options.FileCharset);
        }
    }
}
