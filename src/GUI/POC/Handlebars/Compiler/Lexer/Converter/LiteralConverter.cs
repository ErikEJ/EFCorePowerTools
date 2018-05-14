using System;
using System.Collections.Generic;
using HandlebarsDotNet.Compiler.Lexer;
using System.Linq.Expressions;
using System.Linq;

namespace HandlebarsDotNet.Compiler
{
    internal class LiteralConverter : TokenConverter
    {
        public static IEnumerable<object> Convert(IEnumerable<object> sequence)
        {
            return new LiteralConverter().ConvertTokens(sequence).ToList();
        }

        private LiteralConverter()
        {
        }

        public override IEnumerable<object> ConvertTokens(IEnumerable<object> sequence)
        {
            foreach (var item in sequence)
            {
                if (item is LiteralExpressionToken)
                {
                    yield return Expression.Constant(((LiteralExpressionToken)item).Value);
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
}

