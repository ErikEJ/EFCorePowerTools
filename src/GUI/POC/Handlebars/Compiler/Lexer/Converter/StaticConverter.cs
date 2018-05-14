using System;
using System.Collections.Generic;
using HandlebarsDotNet.Compiler.Lexer;
using System.Linq;

namespace HandlebarsDotNet.Compiler
{
    internal class StaticConverter : TokenConverter
    {
        public static IEnumerable<object> Convert(IEnumerable<object> sequence)
        {
            return new StaticConverter().ConvertTokens(sequence).ToList();
        }

        private StaticConverter()
        {
        }

        public override IEnumerable<object> ConvertTokens(IEnumerable<object> sequence)
        {
            foreach (var item in sequence)
            {
                if (item is StaticToken)
                {
                    if (((StaticToken)item).Value != string.Empty)
                    {
                        yield return HandlebarsExpression.Static(((StaticToken)item).Value);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
}

