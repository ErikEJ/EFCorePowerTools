using System;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace HandlebarsDotNet.Compiler
{
    internal class IteratorBinder : HandlebarsExpressionVisitor
    {
        public static Expression Bind(Expression expr, CompilationContext context)
        {
            return new IteratorBinder(context).Visit(expr);
        }

        private IteratorBinder(CompilationContext context)
            : base(context)
        {
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            return Expression.Block(
                node.Type,
                node.Variables,
                node.Expressions.Select(n => Visit(n)));
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            return Expression.Condition(
                Visit(node.Test),
                Visit(node.IfTrue),
                Visit(node.IfFalse));
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return Expression.MakeUnary(
                node.NodeType,
                Visit(node.Operand),
                node.Type);
        }

        protected override Expression VisitIteratorExpression(IteratorExpression iex)
        {
            var iteratorBindingContext = Expression.Variable(typeof(BindingContext), "context");
            return Expression.Block(
                new ParameterExpression[]
                {
                    iteratorBindingContext
                },
                Expression.IfThenElse(
                    Expression.TypeIs(iex.Sequence, typeof(IEnumerable)),
                    Expression.IfThenElse(
#if netstandard
                        Expression.Call(new Func<object, bool>(IsNonListDynamic).GetMethodInfo(), new[] { iex.Sequence }),
#else
                        Expression.Call(new Func<object, bool>(IsNonListDynamic).Method, new[] { iex.Sequence }),
#endif
                        GetDynamicIterator(iteratorBindingContext, iex),
                        Expression.IfThenElse(
#if netstandard
                            Expression.Call(new Func<object, bool>(IsGenericDictionary).GetMethodInfo(), new[] { iex.Sequence }),
#else
                            Expression.Call(new Func<object, bool>(IsGenericDictionary).Method, new[] { iex.Sequence }),
#endif
                            GetDictionaryIterator(iteratorBindingContext, iex),
                            GetEnumerableIterator(iteratorBindingContext, iex))),
                    GetObjectIterator(iteratorBindingContext, iex))
            );
        }

        private Expression GetEnumerableIterator(Expression contextParameter, IteratorExpression iex)
        {
            var fb = new FunctionBuilder(CompilationContext.Configuration);
            return Expression.Block(
                Expression.Assign(contextParameter,
                    Expression.New(
                        typeof(IteratorBindingContext).GetConstructor(new[] { typeof(BindingContext) }),
                        new Expression[] { CompilationContext.BindingContext })),
                Expression.Call(
#if netstandard
                    new Action<IteratorBindingContext, IEnumerable, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).GetMethodInfo(),
#else
                    new Action<IteratorBindingContext, IEnumerable, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).Method,
#endif
                    new Expression[]
                    {
                        Expression.Convert(contextParameter, typeof(IteratorBindingContext)),
                        Expression.Convert(iex.Sequence, typeof(IEnumerable)),
                        fb.Compile(new [] { iex.Template }, contextParameter),
                        fb.Compile(new [] { iex.IfEmpty }, CompilationContext.BindingContext)
                    }));
        }

        private Expression GetObjectIterator(Expression contextParameter, IteratorExpression iex)
        {
            var fb = new FunctionBuilder(CompilationContext.Configuration);
            return Expression.Block(
                Expression.Assign(contextParameter,
                    Expression.New(
                        typeof(ObjectEnumeratorBindingContext).GetConstructor(new[] { typeof(BindingContext) }),
                        new Expression[] { CompilationContext.BindingContext })),
                Expression.Call(
#if netstandard
                    new Action<ObjectEnumeratorBindingContext, object, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).GetMethodInfo(),
#else
                    new Action<ObjectEnumeratorBindingContext, object, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).Method,
#endif
                    new Expression[]
                    {
                        Expression.Convert(contextParameter, typeof(ObjectEnumeratorBindingContext)),
                        iex.Sequence,
                        fb.Compile(new [] { iex.Template }, contextParameter),
                        fb.Compile(new [] { iex.IfEmpty }, CompilationContext.BindingContext)
                    }));
        }

        private Expression GetDictionaryIterator(Expression contextParameter, IteratorExpression iex)
        {
            var fb = new FunctionBuilder(CompilationContext.Configuration);
            return Expression.Block(
                Expression.Assign(contextParameter,
                    Expression.New(
                        typeof(ObjectEnumeratorBindingContext).GetConstructor(new[] { typeof(BindingContext) }),
                        new Expression[] { CompilationContext.BindingContext })),
                Expression.Call(
#if netstandard
                    new Action<ObjectEnumeratorBindingContext, IEnumerable, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).GetMethodInfo(),
#else
                    new Action<ObjectEnumeratorBindingContext, IEnumerable, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).Method,
#endif
                    new Expression[]
                    {
                        Expression.Convert(contextParameter, typeof(ObjectEnumeratorBindingContext)),
                        Expression.Convert(iex.Sequence, typeof(IEnumerable)),
                        fb.Compile(new [] { iex.Template }, contextParameter),
                        fb.Compile(new [] { iex.IfEmpty }, CompilationContext.BindingContext)
                    }));
        }

        private Expression GetDynamicIterator(Expression contextParameter, IteratorExpression iex)
        {
            var fb = new FunctionBuilder(CompilationContext.Configuration);
            return Expression.Block(
                Expression.Assign(contextParameter,
                    Expression.New(
                        typeof(ObjectEnumeratorBindingContext).GetConstructor(new[] { typeof(BindingContext) }),
                        new Expression[] { CompilationContext.BindingContext })),
                Expression.Call(
#if netstandard
                    new Action<ObjectEnumeratorBindingContext, IDynamicMetaObjectProvider, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).GetMethodInfo(),
#else
                    new Action<ObjectEnumeratorBindingContext, IDynamicMetaObjectProvider, Action<TextWriter, object>, Action<TextWriter, object>>(Iterate).Method,
#endif
                    new Expression[]
                    {
                        Expression.Convert(contextParameter, typeof(ObjectEnumeratorBindingContext)),
                        Expression.Convert(iex.Sequence, typeof(IDynamicMetaObjectProvider)),
                        fb.Compile(new [] { iex.Template }, contextParameter),
                        fb.Compile(new [] { iex.IfEmpty }, CompilationContext.BindingContext)
                    }));
        }

        private static bool IsNonListDynamic(object target)
        {
            var interfaces = target.GetType().GetInterfaces();
            return interfaces.Contains(typeof(IDynamicMetaObjectProvider))
                && ((IDynamicMetaObjectProvider)target).GetMetaObject(Expression.Constant(target)).GetDynamicMemberNames().Any();
        }

        private static bool IsGenericDictionary(object target)
        {
            return
                target.GetType()
#if netstandard
                    .GetInterfaces()
                    .Where(i => i.GetTypeInfo().IsGenericType)

#else
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
#endif
                    .Any(i => i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private static void Iterate(
            ObjectEnumeratorBindingContext context,
            object target,
            Action<TextWriter, object> template,
            Action<TextWriter, object> ifEmpty)
        {
            if (HandlebarsUtils.IsTruthy(target))
            {
                context.Index = 0;
                foreach (MemberInfo member in target.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public).OfType<MemberInfo>()
                    .Concat(
                        target.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
                    ))
                {
                    context.Key = member.Name;
                    var value = AccessMember(target, member);
                    context.First = (context.Index == 0);
                    template(context.TextWriter, value);
                    context.Index++;
                }
                if (context.Index == 0)
                {
                    ifEmpty(context.TextWriter, context.Value);
                }
            }
            else
            {
                ifEmpty(context.TextWriter, context.Value);
            }
        }

        private static void Iterate(
            ObjectEnumeratorBindingContext context,
            IEnumerable target,
            Action<TextWriter, object> template,
            Action<TextWriter, object> ifEmpty)
        {
            if (HandlebarsUtils.IsTruthy(target))
            {
                context.Index = 0;
#if netstandard
                var keysProperty = target.GetType().GetRuntimeProperty("Keys");
#else
                var keysProperty = target.GetType().GetProperty("Keys");
#endif
                if (keysProperty != null)
                {
                    var keys = keysProperty.GetGetMethod().Invoke(target, null) as IEnumerable;
                    if (keys != null)
                    {
                        foreach (var key in keys)
                        {
                            context.Key = key.ToString();
                            var value = target.GetType().GetMethod("get_Item").Invoke(target, new[] { key });
                            context.First = (context.Index == 0);
                            template(context.TextWriter, value);
                            context.Index++;
                        }
                    }
                }
                if (context.Index == 0)
                {
                    ifEmpty(context.TextWriter, context.Value);
                }
            }
            else
            {
                ifEmpty(context.TextWriter, context.Value);
            }
        }

        private static void Iterate(
            ObjectEnumeratorBindingContext context,
            IDynamicMetaObjectProvider target,
            Action<TextWriter, object> template,
            Action<TextWriter, object> ifEmpty)
        {
            if (HandlebarsUtils.IsTruthy(target))
            {
                context.Index = 0;
                var meta = target.GetMetaObject(Expression.Constant(target));
                foreach (var name in meta.GetDynamicMemberNames())
                {
                    context.Key = name;
                    var value = GetProperty(target, name);
                    context.First = (context.Index == 0);
                    template(context.TextWriter, value);
                    context.Index++;
                }
                if (context.Index == 0)
                {
                    ifEmpty(context.TextWriter, context.Value);
                }
            }
            else
            {
                ifEmpty(context.TextWriter, context.Value);
            }
        }

        private static void Iterate(
            IteratorBindingContext context,
            IEnumerable sequence,
            Action<TextWriter, object> template,
            Action<TextWriter, object> ifEmpty)
        {
            context.Index = 0;

            var iter = sequence.GetEnumerator();
            using (iter as IDisposable)
            {
                if (iter.MoveNext())
                {
                    var item = iter.Current;
                    while (!context.Last)
                    {
                        context.Last = !iter.MoveNext();
                        context.First = (context.Index == 0);
                        template(context.TextWriter, item);
                        context.Index++;

                        if (!context.Last)
                        {
                            item = iter.Current;
                        }
                    }
                }
            }

            if (context.Index == 0)
            {
                ifEmpty(context.TextWriter, context.Value);
            }
        }

        private static object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(
                Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }

        private class IteratorBindingContext : BindingContext
        {
            public IteratorBindingContext(BindingContext context)
                : base(context.Value, context.TextWriter, context.ParentContext, context.TemplatePath)
            {
            }

            public int Index { get; set; }

            public bool First { get; set; }

            public bool Last { get; set; }
        }

        private class ObjectEnumeratorBindingContext : BindingContext
        {
            public ObjectEnumeratorBindingContext(BindingContext context)
                : base(context.Value, context.TextWriter, context.ParentContext, context.TemplatePath)
            {
            }

            public string Key { get; set; }

            public int Index { get; set; }

            public bool First { get; set; }
        }

        private static object AccessMember(object instance, MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return ((PropertyInfo)member).GetValue(instance, null);
            }
            if (member is FieldInfo)
            {
                return ((FieldInfo)member).GetValue(instance);
            }
            throw new InvalidOperationException("Requested member was not a field or property");
        }
    }
}

