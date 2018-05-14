using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HandlebarsDotNet.Compiler.Lexer;

namespace HandlebarsDotNet.Compiler
{
    internal class HandlebarsCompiler
    {
        private Tokenizer _tokenizer;
        private FunctionBuilder _functionBuilder;
        private ExpressionBuilder _expressionBuilder;
        private HandlebarsConfiguration _configuration;

        public HandlebarsCompiler(HandlebarsConfiguration configuration)
        {
            _configuration = configuration;
            _tokenizer = new Tokenizer(configuration);
            _expressionBuilder = new ExpressionBuilder(configuration);
            _functionBuilder = new FunctionBuilder(configuration);
        }

        public Action<TextWriter, object> Compile(TextReader source)
        {
            var tokens = _tokenizer.Tokenize(source).ToList();
            var expressions = _expressionBuilder.ConvertTokensToExpressions(tokens);
            return _functionBuilder.Compile(expressions);
        }

        internal Action<TextWriter, object> CompileView(string templatePath)
        {
            var fs = _configuration.FileSystem;
            if (fs == null) throw new InvalidOperationException("Cannot compile view when configuration.FileSystem is not set");
            var template = fs.GetFileContent(templatePath);
            if (template == null) throw new InvalidOperationException("Cannot find template at '" + templatePath + "'");
            IEnumerable<object> tokens = null;
            using (var sr = new StringReader(template))
            {
                tokens = _tokenizer.Tokenize(sr).ToList();
            }
            var layoutToken = tokens.OfType<LayoutToken>().SingleOrDefault();

            var expressions = _expressionBuilder.ConvertTokensToExpressions(tokens);
            var compiledView = _functionBuilder.Compile(expressions, templatePath);
            if (layoutToken == null) return compiledView;

            var layoutPath = fs.Closest(templatePath, layoutToken.Value + ".hbs");
            if (layoutPath == null) throw new InvalidOperationException("Cannot find layout '" + layoutPath + "' for template '" + templatePath + "'");

            var compiledLayout = CompileView(layoutPath);

            return (tw, vm) =>
            {
                var sb = new StringBuilder();
                using (var innerWriter = new StringWriter(sb))
                {
                    compiledView(innerWriter, vm);
                }
                var inner = sb.ToString();
                compiledLayout(tw, new DynamicViewModel(new object[] { new { body = inner }, vm }));
            };
        }


        internal class DynamicViewModel : DynamicObject
        {
            private readonly object[] _objects;
            private static readonly BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

            public DynamicViewModel(params object[] objects)
            {
                _objects = objects;
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _objects.Select(o => o.GetType())
                    .SelectMany(t => t.GetMembers(BindingFlags))
                    .Select(m => m.Name);
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = null;
                foreach (var target in _objects)
                {
                    var member = target.GetType().GetMember(binder.Name, BindingFlags);
                    if (member.Length > 0)
                    {
                        if (member[0] is PropertyInfo)
                        {
                            result = ((PropertyInfo)member[0]).GetValue(target, null);
                            return true;
                        }
                        if (member[0] is FieldInfo)
                        {
                            result = ((FieldInfo)member[0]).GetValue(target);
                            return true;
                        }
                    }
                }
                return false;
            }
        }

    }


}

