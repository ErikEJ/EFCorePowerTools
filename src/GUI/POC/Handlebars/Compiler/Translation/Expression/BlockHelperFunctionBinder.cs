using System.Linq.Expressions;
using System.Reflection;

namespace HandlebarsDotNet.Compiler
{
    internal class BlockHelperFunctionBinder : HandlebarsExpressionVisitor
    {
        public static Expression Bind(Expression expr, CompilationContext context)
        {
            return new BlockHelperFunctionBinder(context).Visit(expr);
        }

        private BlockHelperFunctionBinder(CompilationContext context)
            : base(context)
        {
        }

        protected override Expression VisitStatementExpression(StatementExpression sex)
        {
            if (sex.Body is BlockHelperExpression)
            {
                return Visit(sex.Body);
            }
            else
            {
                return sex;
            }
        }

        protected override Expression VisitBlockHelperExpression(BlockHelperExpression bhex)
        {
            var isInlinePartial = bhex.HelperName == "#*inline";

            var fb = new FunctionBuilder(CompilationContext.Configuration);


            var bindingContext = isInlinePartial ? (Expression)CompilationContext.BindingContext :
                            Expression.Property(
                                CompilationContext.BindingContext,
                                typeof(BindingContext).GetProperty("Value"));

            var body = fb.Compile(((BlockExpression)bhex.Body).Expressions, CompilationContext.BindingContext);
            var inversion = fb.Compile(((BlockExpression)bhex.Inversion).Expressions, CompilationContext.BindingContext);
            var helper = CompilationContext.Configuration.BlockHelpers[bhex.HelperName.Replace("#", "")];
            var arguments = new Expression[]
            {
                Expression.Property(
                    CompilationContext.BindingContext,
                    typeof(BindingContext).GetProperty("TextWriter")),
                Expression.New(
                        typeof(HelperOptions).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0],
                        body,
                        inversion),
                //this next arg is usually data, like { first: "Marc" } 
                //but for inline partials this is the complete BindingContext.
                bindingContext,
                Expression.NewArrayInit(typeof(object), bhex.Arguments)
            };


            if (helper.Target != null)
            {
                return Expression.Call(
                    Expression.Constant(helper.Target),
#if netstandard
                    helper.GetMethodInfo(),
#else
                    helper.Method,
#endif
                    arguments);
            }
            else
            {
                return Expression.Call(
#if netstandard
                    helper.GetMethodInfo(),
#else
                    helper.Method,
#endif
                    arguments);
            }
        }
    }
}

