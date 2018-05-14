using System;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal abstract class Token
    {
        public abstract TokenType Type { get; }

        public abstract string Value { get; }

        public static StaticToken Static(string value)
        {
            return new StaticToken(value);
        }

        public static LiteralExpressionToken Literal(string value, string delimiter = null)
        {
            return new LiteralExpressionToken(value, delimiter);
        }

        public static WordExpressionToken Word(string word)
        {
            return new WordExpressionToken(word);
        }

        public static StartExpressionToken StartExpression(bool isEscaped, bool trimWhitespace)
        {
            return new StartExpressionToken(isEscaped, trimWhitespace);
        }

        public static EndExpressionToken EndExpression(bool isEscaped, bool trimWhitespace)
        {
            return new EndExpressionToken(isEscaped, trimWhitespace);
        }

        public static CommentToken Comment(string comment)
        {
            return new CommentToken(comment);
        }

        public static PartialToken Partial()
        {
            return new PartialToken();
        }

        public static LayoutToken Layout(string layout)
        {
            return new LayoutToken(layout);
        }

        public static StartSubExpressionToken StartSubExpression()
        {
            return new StartSubExpressionToken();
        }

        public static EndSubExpressionToken EndSubExpression()
        {
            return new EndSubExpressionToken();
        }

        public static HashParameterToken HashParameter(string parameter)
        {
            return new HashParameterToken(parameter);
        }
    }
}

