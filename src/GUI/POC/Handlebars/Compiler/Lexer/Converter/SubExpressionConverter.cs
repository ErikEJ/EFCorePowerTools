using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HandlebarsDotNet.Compiler.Lexer;

namespace HandlebarsDotNet.Compiler
{
    internal class SubExpressionConverter : TokenConverter
    {
        public static IEnumerable<object> Convert(IEnumerable<object> sequence)
        {
            return new SubExpressionConverter().ConvertTokens(sequence).ToList();
        }

        private SubExpressionConverter()
        {
        }

        public override IEnumerable<object> ConvertTokens(IEnumerable<object> sequence)
        {
            var enumerator = sequence.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item is StartSubExpressionToken)
                {
                    yield return BuildSubExpression(enumerator);
                }
                else
                {
                    yield return item;
                }
            }
        }

        private static SubExpressionExpression BuildSubExpression(IEnumerator<object> enumerator)
        {
            object item = GetNext(enumerator);
            var path = item as PathExpression;
            if (path == null)
            {
                throw new HandlebarsCompilerException("Found a sub-expression that does not contain a path expression");
            }
            var helperArguments = AccumulateSubExpression(enumerator);
            return HandlebarsExpression.SubExpression(
                HandlebarsExpression.Helper(
                    path.Path,
                    helperArguments));
        }

        private static IEnumerable<Expression> AccumulateSubExpression(IEnumerator<object> enumerator)
        {
            var item = GetNext(enumerator);
            List<Expression> helperArguments = new List<Expression>();
            while ((item is EndSubExpressionToken) == false)
            {
                if (item is StartSubExpressionToken)
                {
                    item = BuildSubExpression(enumerator);
                }
                else if ((item is Expression) == false)
                {
                    throw new HandlebarsCompilerException(string.Format("Token '{0}' could not be converted to an expression", item));
                }
                helperArguments.Add((Expression)item);
                item = GetNext(enumerator);
            }
            return helperArguments;
        }



        private static object GetNext(IEnumerator<object> enumerator)
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
    }
}

