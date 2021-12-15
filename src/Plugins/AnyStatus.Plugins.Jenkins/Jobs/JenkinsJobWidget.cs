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
        [AsyncItemsSource(typeof(JenkinsJobsSource), autoload: true)]
        public string Job { get; set; }
    }
}
