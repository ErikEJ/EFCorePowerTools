using System;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class StatementExpression : HandlebarsExpression
    {
        public StatementExpression(Expression body, bool isEscaped, bool trimBefore, bool trimAfter)
        {
            Body = body;
            IsEscaped = isEscaped;
            TrimBefore = trimBefore;
            TrimAfter = trimAfter;
        }

        public Expression Body { get; private set; }

        public bool IsEscaped { get; private set; }

        public bool TrimBefore { get; private set; }

        public bool TrimAfter { get; private set; }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.StatementExpression; }
        }

        public override Type Type
        {
            get { return typeof(void); }
        }
    }
}