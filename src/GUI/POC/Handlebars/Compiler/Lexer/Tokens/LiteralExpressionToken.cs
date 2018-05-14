using System;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class LiteralExpressionToken : ExpressionToken
    {
        private readonly string _value;
        private readonly string _delimiter;

        public LiteralExpressionToken(string value, string delimiter = null)
        {
            _value = value;
            _delimiter = delimiter;
        }

        public bool IsDelimitedLiteral
        {
            get { return _delimiter == null; }
        }

        public string Delimiter
        {
            get { return _delimiter; }
        }

        public override TokenType Type
        {
            get { return TokenType.Literal; }
        }

        public override string Value
        {
            get { return _value; }
        }
    }
}

