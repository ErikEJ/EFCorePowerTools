using System;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class BoolishExpression : HandlebarsExpression
    {
        private readonly Expression _condition;

        public BoolishExpression(Expression condition)
        {
            _condition = condition;
        }

        public Expression Condition
        {
            get { return _condition; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.BoolishExpression; }
        }

        public override Type Type
        {
            get { return typeof(bool); }
        }
    }
}

