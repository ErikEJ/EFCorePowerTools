using System.Collections.Generic;
using HandlebarsDotNet.Compiler.Lexer;
using System.Linq;

namespace HandlebarsDotNet.Compiler
{
    internal class HashParametersConverter : TokenConverter
    {
        public static IEnumerable<object> Convert(IEnumerable<object> sequence)
        {
            return new HashParametersConverter().ConvertTokens(sequence).ToList();
        }

        private HashParametersConverter() { }

        public override IEnumerable<object> ConvertTokens(IEnumerable<object> sequence)
        {
            var enumerator = sequence.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;

                if (item is HashParameterToken)
                {
                    var parameters = AccumulateParameters(enumerator);

                    if (parameters.Any())
                    {
                        yield return HandlebarsExpression.HashParametersExpression(parameters);
                    }

                    yield return enumerator.Current;
                }
                else
                {
                    yield return item;
                }
            }
        }

        private static Dictionary<string, object> AccumulateParameters(IEnumerator<object> enumerator)
        {
            var parameters = new Dictionary<string, object>();

            var item = enumerator.Current;

            while ((item is EndExpressionToken) == false)
            {
                var parameter = item as HashParameterToken;

                if (parameter != null)
                {
                    var segments = parameter.Value.Split('=');
                    var value = ParseValue(segments[1]);
                    parameters.Add(segments[0], value);
                }

                if (item is EndSubExpressionToken)
                {
                    break;
                }

                item = GetNext(enumerator);
            }

            return parameters;
        }

        private static object ParseValue(string value)
        {
            if (value.StartsWith("'") || value.StartsWith("\""))
            {
                return value.Trim('\'', '"');
            }

            bool boolValue;

            if (bool.TryParse(value, out boolValue))
            {
                return boolValue;
            }

            int intValue;

            if (int.TryParse(value, out intValue))
            {
                return intValue;
            }

            return HandlebarsExpression.Path(value);
        }

        private static object GetNext(IEnumerator<object> enumerator)
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
    }
}

