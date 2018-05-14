using System;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class DeferredSectionExpression : HandlebarsExpression
    {
        public DeferredSectionExpression(
            PathExpression path,
            BlockExpression body,
            BlockExpression inversion)
        {
            Path = path;
            Body = body;
            Inversion = inversion;
        }

        public BlockExpression Body { get; private set; }

        public BlockExpression Inversion { get; private set; }

        public PathExpression Path { get; private set; }

        public override Type Type
        {
            get { return typeof(void); }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.DeferredSection; }
        }
    }
}

