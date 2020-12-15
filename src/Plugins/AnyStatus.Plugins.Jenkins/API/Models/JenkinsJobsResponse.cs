using System.Collections.Generic;

namespace AnyStatus.Plugins.Jenkins.API.Models
{
    public class JenkinsViewsResponse
    {
        public IEnumerable<JenkinsView> Views { get; set; }
    }
}
