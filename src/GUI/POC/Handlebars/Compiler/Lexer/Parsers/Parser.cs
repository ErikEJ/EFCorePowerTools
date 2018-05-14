using System;
using System.IO;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal abstract class Parser
    {
        public abstract Token Parse(TextReader reader);
    }
}

