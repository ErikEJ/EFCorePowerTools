using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class CommentVisitor : HandlebarsExpressionVisitor
    {
        public static Expression Visit(Expression expr, CompilationContext compilationContext)
        {
            return new CommentVisitor(compilationContext).Visit(expr);
        }

        private CommentVisitor(CompilationContext compilationContext) 
            : base(compilationContext)
        {
        }

        protected override Expression VisitStatementExpression(StatementExpression sex)
        {
            if (sex.Body is CommentExpression)
            {
                return Expression.Empty();
            }

            return sex;
        }
    }
}