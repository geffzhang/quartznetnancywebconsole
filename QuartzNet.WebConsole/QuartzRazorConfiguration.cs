#region

using System.Collections.Generic;
using Nancy.Helpers;
using Nancy.ViewEngines.Razor;
using Quartz;
using QuartzNet.WebConsole.Modules;
using QuartzNet.WebConsole.Views;

#endregion

namespace QuartzNet.WebConsole
{
    public class QuartzRazorConfiguration : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return typeof (QuartzConsoleModule).Assembly.FullName;
            yield return typeof (JobKey).Assembly.FullName;
            yield return typeof(HttpUtility).Assembly.FullName;
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return typeof (QuartzConsoleModule).Namespace;
            yield return typeof (HtmlHelpers).Namespace;
            yield return typeof (HttpUtility).Namespace;
        }


        public bool AutoIncludeModelNamespace
        {
            get { return false; }
        }
    }
}