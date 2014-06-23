using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nancy.Hosting.Self;
using Quartz;
using Quartz.Impl;
using Topshelf;

namespace QuartzNet.NugetTest
{
    internal class HelloJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello!");
            Thread.Sleep(5000);
        }
    }


    class Scheduler
    {
        private ISchedulerFactory _s;
        private NancyHost _host;

        public ISchedulerFactory GetScheduler()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            var sched = schedFact.GetScheduler();
            sched.Start();

            for (var i = 1; i < 10; i++)
            {
                var job = JobBuilder.Create().OfType<HelloJob>().WithIdentity("myJob-" + i).Build();
                var trigger =
                    TriggerBuilder.Create()
                                  .ForJob(job)
                                  .WithDailyTimeIntervalSchedule(s => s.OnEveryDay().WithIntervalInMinutes(i * 5))
                                  .Build();
                sched.ScheduleJob(job, trigger);
            }
            return schedFact;
        }
        public void Start()
        {
            _s = GetScheduler();
            _host = QuartzNet.WebConsole.QuartzConsoleBootstrapper.StartDefault(_s);
        }

        public void Stop()
        {
            _host.Stop();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<Scheduler>(s =>                        //2
                {
                    s.ConstructUsing(name => new Scheduler());     //3
                    s.WhenStarted(tc => tc.Start());              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Topshelf Host");        //7
                x.SetDisplayName("Stuff");                       //8
                x.SetServiceName("stuff");                       //9
            });                                                  //10

        }
    }
}
