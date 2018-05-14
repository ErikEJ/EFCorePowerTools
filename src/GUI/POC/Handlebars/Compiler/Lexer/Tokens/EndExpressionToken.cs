using System;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class EndExpressionToken : ExpressionScopeToken
    {
        private readonly bool _isEscaped;
        private readonly bool _trimWhitespace;

        public EndExpressionToken(bool isEscaped, bool trimWhitespace)
        {
            _isEscaped = isEscaped;
            _trimWhitespace = trimWhitespace;
        }

        public bool IsEscaped
        {
            get { return _isEscaped; }
        }

        public bool TrimTrailingWhitespace
        {
            get { return _trimWhitespace; }
        }

        public override string Value
        {
            get { return IsEscaped ? "}}" : "}}}"; }
        }

        public override TokenType Type
        {
            get { return TokenType.EndExpression; }
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}

