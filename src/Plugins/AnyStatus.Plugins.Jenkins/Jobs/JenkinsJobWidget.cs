using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Jenkins.Jobs
{
    [Category("Jenkins")]
    [DisplayName("Jenkins Job Status")]
    [Description("View the status of jobs on Jenkins server")]
    public class JenkinsJobWidget : StatusWidget, IRequireEndpoint<JenkinsEndpoint>, IStandardWidget, IPollable, IProgress
    {
        [Required]
        [EndpointSource]
        [Refresh(nameof(Job))]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }

        [Required]
        [DisplayName("Job")]
        [Description("Required")]
        [AsyncItemsSource(typeof(JenkinsJobsSource))]
        public string Job { get; set; }
    }
}
