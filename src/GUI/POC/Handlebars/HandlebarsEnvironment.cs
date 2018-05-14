using System;
using System.IO;
using System.Text;
using HandlebarsDotNet.Compiler;

namespace HandlebarsDotNet
{
    public partial class Handlebars
    {
        private class HandlebarsEnvironment : IHandlebars
        {
            private readonly HandlebarsConfiguration _configuration;
            private readonly HandlebarsCompiler _compiler;

            public HandlebarsEnvironment(HandlebarsConfiguration configuration)
            {
                if (configuration == null)
                {
                    throw new ArgumentNullException("configuration");
                }

                _configuration = configuration;
                _compiler = new HandlebarsCompiler(_configuration);
                RegisterBuiltinHelpers();
            }

            public Func<object, string> CompileView(string templatePath)
            {
                var compiledView = _compiler.CompileView(templatePath);
                return (vm) =>
                {
                    var sb = new StringBuilder();
                    using (var tw = new StringWriter(sb))
                    {
                        compiledView(tw, vm);
                    }
                    return sb.ToString();
                };
            }

            public HandlebarsConfiguration Configuration
            {
                get
                {
                    return this._configuration;
                }
            }

            public Action<TextWriter, object> Compile(TextReader template)
            {
                return _compiler.Compile(template);
            }

            public Func<object, string> Compile(string template)
            {
                using (var reader = new StringReader(template))
                {
                    var compiledTemplate = Compile(reader);
                    return context =>
                    {
                        var builder = new StringBuilder();
                        using (var writer = new StringWriter(builder))
                        {
                            compiledTemplate(writer, context);
                        }
                        return builder.ToString();
                    };
                }
            }

            public void RegisterTemplate(string templateName, Action<TextWriter, object> template)
            {
                lock (_configuration)
                {
                    _configuration.RegisteredTemplates.AddOrUpdate(templateName, template);
                }
            }

            public void RegisterTemplate(string templateName, string template)
            {
                using (var reader = new StringReader(template))
                {
                    RegisterTemplate(templateName, Compile(reader));
                }
            }

            public void RegisterHelper(string helperName, HandlebarsHelper helperFunction)
            {
                lock (_configuration)
                {
                    _configuration.Helpers.AddOrUpdate(helperName, helperFunction);
                }
            }

            public void RegisterHelper(string helperName, HandlebarsBlockHelper helperFunction)
            {
                lock (_configuration)
                {
                    _configuration.BlockHelpers.AddOrUpdate(helperName, helperFunction);
                }
            }

            private void RegisterBuiltinHelpers()
            {
                foreach (var helperDefinition in BuiltinHelpers.Helpers)
                {
                    RegisterHelper(helperDefinition.Key, helperDefinition.Value);
                }
                foreach (var helperDefinition in BuiltinHelpers.BlockHelpers)
                {
                    RegisterHelper(helperDefinition.Key, helperDefinition.Value);
                }
            }
        }
    }
}
