using System;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class IteratorExpression : HandlebarsExpression
    {
        private readonly Expression _sequence;
        private readonly Expression _template;
        private readonly Expression _ifEmpty;


        public IteratorExpression(Expression sequence, Expression template)
            : this(sequence, template, Expression.Empty())
        {
        }

        public IteratorExpression(Expression sequence, Expression template, Expression ifEmpty)
        {
            _sequence = sequence;
            _template = template;
            _ifEmpty = ifEmpty;
        }

        public Expression Sequence
        {
            get { return _sequence; }
        }

        public Expression Template
        {
            get { return _template; }
        }

        public Expression IfEmpty
        {
            get { return _ifEmpty; }
        }

        public override Type Type
        {
            get { return typeof(void); }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.IteratorExpression; }
        }
    }
}

