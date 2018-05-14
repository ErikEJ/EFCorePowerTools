using System;
using HandlebarsDotNet.Compiler;
using System.Linq.Expressions;

namespace HandlebarsDotNet
{
    internal class SubExpressionExpression : HandlebarsExpression
    {
        private readonly Expression _expression;

        public SubExpressionExpression(Expression expression)
        {
            _expression = expression;
        }

        public override Type Type
        {
            get { return typeof(object); }
        }

        public Expression Expression
        {
            get { return _expression; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.SubExpression; }
        }
    }
}

