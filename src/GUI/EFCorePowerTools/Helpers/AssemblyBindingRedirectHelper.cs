using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
// ReSharper disable once CheckNamespace
namespace ErikEJ.SqlCeToolbox.Helpers
{
    public static class AssemblyBindingRedirectHelper
    {

        ///<summary>
                /// Reads the "BindingRedirecs" field from the app settings and applies the redirection on the
                /// specified assemblies
                /// </summary>

        public static void ConfigureBindingRedirects()
        {
            var redirects = GetBindingRedirects();
            redirects.ForEach(RedirectAssembly);
        }

        private static List<BindingRedirect> GetBindingRedirects()
        {
            //var result = new List<BindingRedirect>
            //{
            //    new BindingRedirect
            //    {
            //        ShortName = "Microsoft.EntityFrameworkCore",
            //        RedirectToVersion = "2.0.1.0",
            //        PublicKeyToken = "adb9793829ddae60"
            //    },
            //    new BindingRedirect
            //    {
            //        ShortName = "Microsoft.EntityFrameworkCore.Relational",
            //        RedirectToVersion = "2.0.1.0",
            //        PublicKeyToken = "adb9793829ddae60"
            //    }
            //};

            //System.Diagnostics.DiagnosticSource, Version=4.0.2.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'

            var result = new List<BindingRedirect>
            {
                new BindingRedirect
                {
                    ShortName = "System.Diagnostics.DiagnosticSource",
                    RedirectToVersion = "4.0.2.0",
                    PublicKeyToken = "cc7b13ffcd2ddd51"
                }
            };

            return result;
        }

        private static void RedirectAssembly(BindingRedirect bindingRedirect)
        {
            ResolveEventHandler handler = null;
            handler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                if (requestedAssembly.Name != bindingRedirect.ShortName)
                {
                    return null;
                }
                var targetPublicKeyToken = new AssemblyName("x, PublicKeyToken=" + bindingRedirect.PublicKeyToken).GetPublicKeyToken();
                requestedAssembly.SetPublicKeyToken(targetPublicKeyToken);
                requestedAssembly.Version = new Version(bindingRedirect.RedirectToVersion);
                requestedAssembly.CultureInfo = CultureInfo.InvariantCulture;
                AppDomain.CurrentDomain.AssemblyResolve -= handler;
                return Assembly.Load(requestedAssembly);
            };
            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }

        public class BindingRedirect
        {
            public string ShortName { get; set; }
            public string PublicKeyToken { get; set; }
            public string RedirectToVersion { get; set; }
        }
    }
}