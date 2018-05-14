using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Compiler.Lexer;

namespace HandlebarsDotNet.Compiler
{
    internal class HelperArgumentAccumulator : TokenConverter
    {
        public static IEnumerable<object> Accumulate(IEnumerable<object> sequence)
        {
            return new HelperArgumentAccumulator().ConvertTokens(sequence).ToList();
        }

        private HelperArgumentAccumulator()
        {
        }

        public override IEnumerable<object> ConvertTokens(IEnumerable<object> sequence)
        {
            var enumerator = sequence.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item is HelperExpression)
                {
                    var helper = item as HelperExpression;
                    var helperArguments = AccumulateArguments(enumerator);
                    yield return HandlebarsExpression.Helper(
                        helper.HelperName,
                        helperArguments);
                    yield return enumerator.Current;
                }
                else if (item is PathExpression)
                {
                    var helperArguments = AccumulateArguments(enumerator);
                    if (helperArguments.Count > 0)
                    {
                        var path = item as PathExpression;
                        yield return HandlebarsExpression.Helper(
                            path.Path,
                            helperArguments);
                        yield return enumerator.Current;
                    }
                    else
                    {
                        yield return item;
                        yield return enumerator.Current;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }

        private static List<Expression> AccumulateArguments(IEnumerator<object> enumerator)
        {
            var item = GetNext(enumerator);
            List<Expression> helperArguments = new List<Expression>();
            while ((item is EndExpressionToken) == false)
            {
                if ((item is Expression) == false)
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

