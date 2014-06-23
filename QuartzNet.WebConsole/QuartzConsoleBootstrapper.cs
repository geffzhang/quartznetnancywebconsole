#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Logging;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Hosting.Self;
using Nancy.Json;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;
using Quartz;
using QuartzNet.WebConsole.Modules;
using QuartzNet.WebConsole.Views;

#endregion

namespace QuartzNet.WebConsole
{
    public class QuartzConsoleBootstrapper : DefaultNancyBootstrapper
    {
        private static readonly ILog Log = LogManager.GetLogger<QuartzConsoleBootstrapper>();
        public static NancyHost StartDefault(ISchedulerFactory factory)
        {
            return QuartzConsoleStarter.Configure.UsingSchedulerFactory(factory).HostedOnDefault().Start();
        }

        internal static NancyHost Start(ISchedulerFactory factory, Uri hostUrl, ISet<string> ignoredScheduleSet)
        {
            Factory = factory;
            IgnoredScheduleSet = ignoredScheduleSet;
            var defaultNancyBootstrapper = new QuartzConsoleBootstrapper();
            var nancyHost = new NancyHost(hostUrl, defaultNancyBootstrapper);
            nancyHost.Start();
            Log.InfoFormat("QuartzNet Console succesfully started on {0}", new Uri(hostUrl, "quartzconsole"));
            return nancyHost;
        }

        internal static ISet<string> IgnoredScheduleSet { get; set; }

        internal static ISchedulerFactory Factory;


        private static readonly Assembly QuartzAssembly;

        static QuartzConsoleBootstrapper()
        {
            QuartzAssembly = typeof (QuartzConsoleModule).Assembly;
            ResourceViewLocationProvider
                .Ignore.Add(typeof (RazorViewEngine).Assembly);
            ResourceViewLocationProvider
                .RootNamespaces
                .Add(QuartzAssembly, typeof (ViewsPointer).Namespace);

            JsonSettings.MaxJsonLength = int.MaxValue;
            StaticConfiguration.EnableRequestTracing = true;
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                var configuration = NancyInternalConfiguration.Default;
                configuration.ViewLocationProvider = typeof (ResourceViewLocationProvider);
                return configuration;
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IRazorConfiguration>(new QuartzRazorConfiguration());
            container.Register<RazorViewEngine>();

            base.ConfigureApplicationContainer(container);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(
                EmbeddedStaticContentConventionBuilder.AddDirectory("/Content", QuartzAssembly));
        }

#if DEBUG 
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration(){Password = "quartz"}; }
        }
#endif        
    }
}