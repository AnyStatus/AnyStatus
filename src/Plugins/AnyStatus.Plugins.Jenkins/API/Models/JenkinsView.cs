using System.Collections.Generic;

namespace AnyStatus.Plugins.Jenkins.API.Models
{
    public class JenkinsView
    {
        public string Name { get; set; }

        public string URL { get; set; }

        public IEnumerable<JenkinsJob> Jobs { get; set; }

        public IEnumerable<JenkinsView> Views { get; set; }
    }
}