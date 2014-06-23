using System;
using Quartz;

namespace QuartzNet.WebConsole.VewModels.QuartzConsole
{
    public class ScheduleLineViewModel
    {
        public DateTimeOffset Time { get; set; }

        public TriggerKey Name { get; set; }
    }

}