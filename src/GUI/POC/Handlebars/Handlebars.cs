using System;
using System.IO;

namespace HandlebarsDotNet
{
    public delegate void HandlebarsHelper(TextWriter output, dynamic context, params object[] arguments);
    public delegate void HandlebarsBlockHelper(TextWriter output, HelperOptions options, dynamic context, params object[] arguments);

    public sealed partial class Handlebars
    {
        // Lazy-load Handlebars environment to ensure thread safety.  See Jon Skeet's excellent article on this for more info. http://csharpindepth.com/Articles/General/Singleton.aspx
        private static readonly Lazy<IHandlebars> lazy = new Lazy<IHandlebars>(() => new HandlebarsEnvironment(new HandlebarsConfiguration()));

        private static IHandlebars Instance { get { return lazy.Value; } }

        public static IHandlebars Create(HandlebarsConfiguration configuration = null)
        {
            configuration = configuration ?? new HandlebarsConfiguration();
            return new HandlebarsEnvironment(configuration);
        }

        public static Action<TextWriter, object> Compile(TextReader template)
        {
            return Instance.Compile(template);
        }

        public static Func<object, string> Compile(string template)
        {
            return Instance.Compile(template);
        }
        
        public static Func<object, string> CompileView(string templatePath)
        {
            return Instance.CompileView(templatePath);
        }

        public static void RegisterTemplate(string templateName, Action<TextWriter, object> template)
        {
            Instance.RegisterTemplate(templateName, template);
        }

        public static void RegisterTemplate(string templateName, string template)
        {
            Instance.RegisterTemplate(templateName, template);
        }

        public static void RegisterHelper(string helperName, HandlebarsHelper helperFunction)
        {
            Instance.RegisterHelper(helperName, helperFunction);
        }

        public static void RegisterHelper(string helperName, HandlebarsBlockHelper helperFunction)
        {
            Instance.RegisterHelper(helperName, helperFunction);
        }

        /// <summary>
        /// Expose the configuration on order to have access in all Helpers and Templates.
        /// </summary>
        public static HandlebarsConfiguration Configuration
        {
            get { return Instance.Configuration; }
        }
    }
}