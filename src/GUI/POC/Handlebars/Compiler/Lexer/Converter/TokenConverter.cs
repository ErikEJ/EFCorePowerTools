using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using HandlebarsDotNet.Compiler.Lexer;

namespace HandlebarsDotNet.Compiler
{
    internal abstract class TokenConverter
    {
        public abstract IEnumerable<object> ConvertTokens(IEnumerable<object> sequence);
    }
}

