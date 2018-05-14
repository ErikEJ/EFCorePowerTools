using System;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class CommentExpression : HandlebarsExpression
    {
        public string Value { get; private set; }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType) HandlebarsExpressionType.CommentExpression; }
        }

        public override Type Type
        {
            get { return typeof (void); }
        }

        public CommentExpression(string value)
        {
            Value = value;
        }
    }
}