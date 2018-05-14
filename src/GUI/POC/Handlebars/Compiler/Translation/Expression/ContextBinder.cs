using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
#if netstandard
using System.Reflection;
#endif

namespace HandlebarsDotNet.Compiler
{
    internal class ContextBinder : HandlebarsExpressionVisitor
    {
        private ContextBinder()
            : base(null)
        {
        }

        public static Expression Bind(Expression body, CompilationContext context, Expression parentContext, string templatePath)
        {
            var writerParameter = Expression.Parameter(typeof(TextWriter), "buffer");
            var objectParameter = Expression.Parameter(typeof(object), "data");
            if (parentContext == null)
            {
                parentContext = Expression.Constant(null, typeof(BindingContext));
            }

            var encodedWriterExpression = ResolveEncodedWriter(writerParameter, context.Configuration.TextEncoder);
            var templatePathExpression = Expression.Constant(templatePath, typeof(string));
            var newBindingContext = Expression.New(
                            typeof(BindingContext).GetConstructor(
                                new[] { typeof(object), typeof(EncodedTextWriter), typeof(BindingContext), typeof(string) }),
                            new[] { objectParameter, encodedWriterExpression, parentContext, templatePathExpression });
            return Expression.Lambda<Action<TextWriter, object>>(
                Expression.Block(
                    new[] { context.BindingContext },
                    new Expression[]
                    {
                        Expression.IfThenElse(
                            Expression.TypeIs(objectParameter, typeof(BindingContext)),
                            Expression.Assign(context.BindingContext, Expression.TypeAs(objectParameter, typeof(BindingContext))),
                            Expression.Assign(context.BindingContext, newBindingContext))
                    }.Concat(
                        ((BlockExpression)body).Expressions
                    )),
                new[] { writerParameter, objectParameter });
        }

        private static Expression ResolveEncodedWriter(ParameterExpression writerParameter, ITextEncoder textEncoder)
        {
            var outputEncoderExpression = Expression.Constant(textEncoder, typeof(ITextEncoder));

#if netstandard
            var encodedWriterFromMethod = typeof(EncodedTextWriter).GetRuntimeMethod("From", new[] { typeof(TextWriter), typeof(ITextEncoder) });
#else
            var encodedWriterFromMethod = typeof(EncodedTextWriter).GetMethod("From", new[] { typeof(TextWriter), typeof(ITextEncoder) });
#endif

            return Expression.Call(encodedWriterFromMethod, writerParameter, outputEncoderExpression);
        }
    }
}