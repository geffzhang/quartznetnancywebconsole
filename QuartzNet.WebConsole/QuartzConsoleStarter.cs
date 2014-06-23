using System;
using System.Collections.Generic;
using System.Configuration;
using Nancy.Hosting.Self;
using Quartz;

namespace QuartzNet.WebConsole
{
    public class QuartzConsoleStarter
    {
        private ISchedulerFactory _factory;
        private Uri _uri;
        private readonly List<string> _ignoredInSchedule = new List<string>();

        private QuartzConsoleStarter ()
        {
            _ignoredInSchedule.Add("XMLSchedulingDataProcessorPlugin.XMLSchedulingDataProcessorPlugin_xml_quartz_jobs_xml");
        }

        public static QuartzConsoleStarter Configure
        {
            get
            {
                return new QuartzConsoleStarter();
            }
        }

        public QuartzConsoleStarter UsingSchedulerFactory(ISchedulerFactory factory)
        {
            _factory = factory;
            return this;
        }

        public QuartzConsoleStarter HostedOn(Uri uri)
        {
            _uri = uri;
            return this;
        }
        public QuartzConsoleStarter HostedOn(string uri)
        {
            _uri = new Uri(uri);
            return this;
        }
        public QuartzConsoleStarter HostedOnDefault()
        {
            _uri = new Uri(ConfigurationManager.AppSettings["quartznet.webconsole.host"] ?? "http://localhost:1234");
            return this;
        }

        public QuartzConsoleStarter IgnoreInScheduleView(params string[] ignored)
        {
            _ignoredInSchedule.AddRange(ignored);
            return this;
        }

        public NancyHost Start()
        {
            if(_uri == null)
                throw new InvalidOperationException("Uri to host on is not specified");
            if(_factory == null)
                throw new InvalidOperationException("Scheduler factory is not specified");
            return QuartzConsoleBootstrapper.Start(_factory, _uri, new HashSet<string>(_ignoredInSchedule));
        }
        
    }
}