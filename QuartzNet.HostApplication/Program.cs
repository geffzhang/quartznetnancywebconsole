#region

using System;
using System.Threading;
using Nancy.Hosting.Self;
using Quartz;
using Quartz.Impl;
using QuartzNet.WebConsole;

#endregion

namespace QuartzNet.HostApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var nancyHost = QuartzConsoleBootstrapper.StartDefault(GetFactory());
            
            Console.ReadLine();
            nancyHost.Stop();
        }

        private static ISchedulerFactory GetFactory()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            var sched = schedFact.GetScheduler();
            sched.Start();

            CreateJob(sched, "10-13-Wen-Fri-Job", (TriggerBuilder t) => t.WithCronSchedule  ("0 30 10-13 ? * WED,FRI"));
            CreateJob(sched, "8-10-5th-20th-Job", (TriggerBuilder t) => t.WithCronSchedule  ("0 0/30 8-9 5,20 * ?"));
            CreateJob(sched, "11-16-Job", (TriggerBuilder t) => t.WithCronSchedule          ("0 0 11,16 * * ?"));
            CreateJob(sched, "9-18-Mon-Fri-Job", (TriggerBuilder t) => t.WithCronSchedule   ("0 0 09-18 ? * 1-5"));

            return schedFact;
        }

        private static void CreateJob(IScheduler sched, string name, Func<TriggerBuilder, TriggerBuilder> triggerBuilder)
        {
            var job = JobBuilder.Create().OfType<HelloJob>().WithIdentity(name).Build();
            var trigger =
                TriggerBuilder.Create()
                              .ForJob(job);
            sched.ScheduleJob(job, triggerBuilder(trigger).Build());
        }
    }

    internal class HelloJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello!");
            Thread.Sleep(5000);
        }
    }
}