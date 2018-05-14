using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using HandlebarsDotNet.Compiler;

namespace HandlebarsDotNet
{
    internal static class BuiltinHelpers
    {
        [Description("with")]
        public static void With(TextWriter output, HelperOptions options, dynamic context, params object[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new HandlebarsException("{{with}} helper must have exactly one argument");
            }

            if (HandlebarsUtils.IsTruthyOrNonEmpty(arguments[0]))
            {
                options.Template(output, arguments[0]);
            }
            else
            {
                options.Inverse(output, context);
            }
        }

        [Description("*inline")]
        public static void Inline(TextWriter output, HelperOptions options, dynamic context, params object[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new HandlebarsException("{{*inline}} helper must have exactly one argument");
            }

            //This helper needs the "context" var to be the complete BindingContext as opposed to just the
            //data { firstName: "todd" }. The full BindingContext is needed for registering the partial templates.
            //This magic happens in BlockHelperFunctionbinder.VisitBlockHelperExpression

            if (context as BindingContext == null)
            {
                throw new HandlebarsException("{{*inline}} helper must receiving the full BindingContext");
            }

            var key = arguments[0] as string;
            
            //Inline partials cannot use the Handlebars.RegisterTemplate method
            //because it is static and therefore app-wide. To prevent collisions
            //this helper will add the compiled partial to a dicionary
            //that is passed around in the context without fear of collisions.
            context.InlinePartialTemplates.Add(key, options.Template);
        }

        public static IEnumerable<KeyValuePair<string, HandlebarsHelper>> Helpers
        {
            get
            {
                return GetHelpers<HandlebarsHelper>();
            }
        }

        public static IEnumerable<KeyValuePair<string, HandlebarsBlockHelper>> BlockHelpers
        {
            get
            {
                return GetHelpers<HandlebarsBlockHelper>();
            }
        }

        private static IEnumerable<KeyValuePair<string, T>> GetHelpers<T>()
        {
            var builtInHelpersType = typeof(BuiltinHelpers);
            foreach (var method in builtInHelpersType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public))
            {
                Delegate possibleDelegate;
                try
                {
#if netstandard
                        possibleDelegate = method.CreateDelegate(typeof(T));
#else
                    possibleDelegate = Delegate.CreateDelegate(typeof(T), method);
#endif
                }
                catch
                {
                    possibleDelegate = null;
                }
                if (possibleDelegate != null)
                {
#if netstandard
                    yield return new KeyValuePair<string, T>(
                        method.GetCustomAttribute<DescriptionAttribute>().Description,
                        (T)(object)possibleDelegate);
#else
                    yield return new KeyValuePair<string, T>(
                        ((DescriptionAttribute)Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute))).Description,
                        (T)(object)possibleDelegate);
#endif
                }
            }
        }
    }
}

