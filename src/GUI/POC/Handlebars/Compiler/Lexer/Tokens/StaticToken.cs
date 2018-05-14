using System;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class StaticToken : Token
    {
        private readonly string _value;
        private readonly string _original;

        private StaticToken(string value, string original)
        {
            _value = value;
            _original = original;
        }

        internal StaticToken(string value)
            : this(value, value)
        {
        }

        public override TokenType Type
        {
            get { return TokenType.Static; }
        }

        public override string Value
        {
            get { return _value; }
        }

        public string Original
        {
            get { return _original; }
        }

        public StaticToken GetModifiedToken(string value)
        {
            return new StaticToken(value, _original);
        }
    }
}