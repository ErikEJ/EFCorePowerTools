using System;

namespace HandlebarsDotNet.Compiler.Lexer
{
    internal class EndSubExpressionToken : ExpressionScopeToken
    {
        public EndSubExpressionToken()
        {
        }

        public override string Value
        {
            get { return ")"; }
        }

        public override TokenType Type
        {
            get { return TokenType.EndSubExpression; }
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}

