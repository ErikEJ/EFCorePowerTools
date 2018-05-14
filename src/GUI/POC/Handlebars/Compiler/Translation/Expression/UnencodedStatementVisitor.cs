using System;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class UnencodedStatementVisitor : HandlebarsExpressionVisitor
    {
        public static Expression Visit(Expression expr, CompilationContext context)
        {
            return new UnencodedStatementVisitor(context).Visit(expr);
        }

        private UnencodedStatementVisitor(CompilationContext context)
            : base(context)
        {
        }

        protected override Expression VisitStatementExpression(StatementExpression sex)
        {
            if (sex.IsEscaped == false)
            {
                return Expression.Block(
                    typeof(void),
                    Expression.Assign(
                        Expression.Property(CompilationContext.BindingContext, "SuppressEncoding"),
                        Expression.Constant(true)),
                    sex,
                    Expression.Assign(
                        Expression.Property(CompilationContext.BindingContext, "SuppressEncoding"),
                        Expression.Constant(false)),
                    Expression.Empty());
            }
            else
            {
                return sex;
            }
        }
    }
}

