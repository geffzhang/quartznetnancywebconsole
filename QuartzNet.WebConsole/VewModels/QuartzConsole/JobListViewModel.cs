#region

using System.Collections.Generic;

#endregion

namespace QuartzNet.WebConsole.VewModels.QuartzConsole
{
    public class JobListViewModel
    {
        public List<JobGroupViewModel> Groups { get; set; }

        public string Machine { get; set; }
    }
}