using System;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class WordExpressionToken : ExpressionToken
    {
        private readonly string _word;

        public WordExpressionToken(string word)
        {
            _word = word;
        }

        public override TokenType Type
        {
            get { return TokenType.Word; }
        }

        public override string Value
        {
            get { return _word; }
        }
    }
}

