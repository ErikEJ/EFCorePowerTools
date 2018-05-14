using System;
using System.Collections.Generic;
using System.Linq;
using HandlebarsDotNet.Compiler.Lexer;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class HelperConverter : TokenConverter
    {
        private static readonly string[] builtInHelpers = new [] { "else", "each" };

        public static IEnumerable<object> Convert(
            IEnumerable<object> sequence,
            HandlebarsConfiguration configuration)
        {
            return new HelperConverter(configuration).ConvertTokens(sequence).ToList();
        }

        private readonly HandlebarsConfiguration _configuration;

        private HelperConverter(HandlebarsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override IEnumerable<object> ConvertTokens(IEnumerable<object> sequence)
        {
            var enumerator = sequence.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item is StartExpressionToken)
                {
                    yield return item;
                    item = GetNext(enumerator);
                    if (item is Expression)
                    {
                        yield return item;
                        continue;
                    }
                    if (item is WordExpressionToken word)
                    {
                        if (IsRegisteredHelperName(word.Value))
                        {
                            yield return HandlebarsExpression.Helper(word.Value);
                        }
                        else if (IsRegisteredBlockHelperName(word.Value))
                        {
                            yield return HandlebarsExpression.Helper(word.Value);
                        }
                        else
                        {
                            yield return item;
                        }
                    }
                    else
                    {
                        yield return item;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }

        private bool IsRegisteredHelperName(string name)
        {
            return _configuration.Helpers.ContainsKey(name) || builtInHelpers.Contains(name);
        }

        private bool IsRegisteredBlockHelperName(string name)
        {
            if (name[0] != '#') return false;
            name = name.Replace("#", "");
            return _configuration.BlockHelpers.ContainsKey(name) || builtInHelpers.Contains(name);
        }

        private static object GetNext(IEnumerator<object> enumerator)
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
    }
}

