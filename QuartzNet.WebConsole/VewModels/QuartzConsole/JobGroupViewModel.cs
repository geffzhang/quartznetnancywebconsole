#region

using System.Collections.Generic;
using QuartzNet.WebConsole.Modules;

#endregion

namespace QuartzNet.WebConsole.VewModels.QuartzConsole
{
    public class JobGroupViewModel
    {
        public List<JobViewModel> JobDetails { get; set; }
    }
}