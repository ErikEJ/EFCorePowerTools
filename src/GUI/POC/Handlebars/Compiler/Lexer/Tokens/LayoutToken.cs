namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class LayoutToken : Token
    {
        private readonly string _layout;

        public LayoutToken(string layout)
        {
            _layout = layout.Trim('-', ' ');
        }

        public override TokenType Type
        {
            get { return TokenType.Layout; }
        }

        public override string Value
        {
            get { return _layout; }
        }
    }
}